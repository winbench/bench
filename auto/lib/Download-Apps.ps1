param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$downloadDir = Safe-Dir $(Get-ConfigPathValue DownloadDir)

function Get-ProxyUrl([uri]$uri) {
    if ($uri.Scheme -eq "https") {
        return Get-ConfigValue HttpsProxy
    } else {
        return Get-ConfigValue HttpProxy
    }
}

function Get-Proxy([uri]$url) {
    if (Get-ConfigValue UseProxy) {
        $proxyUrl = Get-ProxyUrl $url
        return New-Object System.Net.WebProxy $proxyUrl
    } else {
        return $null
    }
}

function Get-WebClient([string]$url, [string]$appName = $null) {
    $wc = New-Object System.Net.WebClient
    $proxy = Get-Proxy $url
    if ($proxy) {
        $wc.Proxy = $proxy
    }
    if ($appName) {
        $headers = App-DownloadHeaders $appName
        if ($headers) {
            foreach($k in $headers.Keys) {
                Debug "Adding Header: $k = $($headers[$k])"
                $wc.Headers.Add($k, $headers[$k])
            }
        }
        $cookies = App-DownloadCookies $appName
        if ($cookies) {
            $val = [string]::Join("; ", [array]($cookies | % { "$($_.Name)=$($_.Value)" }))
            Debug "Adding Cookies: $val"
            $wc.Headers.Add([System.Net.HttpRequestHeader]::Cookie, $val)
        }
    }
    return $wc
}

function Download-String([string]$url) {
    Write-Host "Downloading page: $url ..."
    $attempt = 1
    $wc = Get-WebClient $url
    while ($attempt -le (Get-ConfigValue DownloadAttempts)) {
        try {
            if ($attempt -gt 1) { Debug "Download attempt $attempt ..." }
            return $wc.DownloadString($url)
        } catch {
            Debug "Download failed $_"
            $attempt++
        }
    }
    return $null
}

function Download-File([string]$name, [string]$url, [string]$target) {
    Write-Host "Downloading file: $url ..."
    $attempt = 1
    $wc = Get-WebClient $url $name
    while ($attempt -le (Get-ConfigValue DownloadAttempts)) {
        try {
            if ($attempt -gt 1) { Debug "Download attempt $attempt ..." }
            $wc.DownloadFile($url, $target)
            return $True
        } catch {
            Debug "Download failed $_"
            $attempt++
        }
    }
    return $False
}

function Get-MetaRefreshUrl([string]$url) {
    $http = Download-String $url
    if (!$http) {
        Write-Warning "Download failed: $url"
        return $null
    }
    Debug "Extracting refresh URL from download page ..."
    $p1 = [regex]"<meta\s[^\>]*?http-equiv=`"refresh`"[^\>]*?\scontent=`"\d+;\s+url=(?<url>.*?)`""
    $p2 = [regex]"<meta\s[^\>]*?content=`"\d+;\s+url=(?<url>.*?)`"[^\>]*?\shttp-equiv=`"refresh`""
    $m = $p1.Match($http)
    if (!$m.Success) { $m = $p2.Match($http) }
    if ($m.Success) {
        return $m.Groups["url"].Value.Replace("&amp;", "&")
    } else {
        Write-Warning "Extracting direct URL from download page failed"
        return $null
    }
}

function Get-FirstSurroundedLinkUrl([string]$url, [regex]$pattern) {
    $http = Download-String $url
    if (!$http) {
        Write-Warning "Download failed: $url"
        return $null
    }
    Debug "Extracting surrounded link URL from download page ..."
    $m = $pattern.Match($http)
    $linkUrl = $null
    if ($m.Success) {
        $linkP = [regex]"\<a\s[^\>]*?href=`"(?<url>[^`"]+)`""
        $m = $linkP.Match($m.Groups[1].Value)
        if ($m.Success) {
            $linkUrl = $m.Groups["url"].Value
        }
    }
    if ($linkUrl) {
        $linkUri = New-Object System.Uri([uri]$url, $linkUrl)
        return [string]$linkUri
    } else {
        Write-Warning "Extracting link URL from download page failed"
        return $null
    }
}

function Get-FirstMatchingLinkUrl([string]$url, [regex]$pattern) {
    $http = Download-String $url
    if (!$http) {
        Write-Warning "Download failed: $url"
        return $null
    }
    Debug "Extracting link URL from download page ..."
    $linkP = [regex]"\<a\s[^\>]*?href=`"(?<url>[^`"]+)`""
    $matchUrl = $null
    $ms = $linkP.Matches($http)
    Debug "  Found $($ms.Count) links"
    for ($i = 0; $i -lt $ms.Count; $i++) {
        $linkUrl = $ms[$i].Groups["url"].Value
        if ($linkUrl -match $pattern) {
            Debug "  First matching URL: $linkUrl"
            $matchUrl = $linkUrl
            break
        }
    }
    if ($matchUrl) {
        $matchUri = New-Object System.Uri([uri]$url, $matchUrl)
        $resolvedUrl = [string]$matchUri
        Debug "  Resolved URL: $resolvedUrl"
        return $resolvedUrl
    } else {
        Write-Warning "Extracting link URL from download page failed"
        return $null
    }
}

function Resolve-Url([string]$url) {
    if ($url -match "^https?://www\.eclipse\.org/downloads/download\.php\?file=(.*)&mirror_id=\d+`$") {
        Debug "URL matches Eclipse mirror download page"
        $url = Get-FirstSurroundedLinkUrl $url "\<span\s[^\>]*class=`"direct-link`"[^\>]*\>(.*?)\</span\>"
        return Resolve-Url $url
    }

    if ($url -match "^https?://www\.eclipse\.org/downloads/download\.php\?file=") {
        Debug "URL matches Eclipse download page"
        $url = Get-FirstMatchingLinkUrl $url "^download\.php\?file=(.*)&mirror_id=\d+`$"
        return Resolve-Url $url
    }

    return $url
}

function Extract-FileName($url) {
    [regex]$ex = '(?<name>[^/]+\.[a-zA-Z0-9]{2,3})(?:\?.*)?$'
    $m = $ex.Match($url)
    if ($m.Success) {
        Debug "Extracted filename: $($m.Groups['name'])"
        return [Uri]::UnescapeDataString($m.Groups['name'].Value)
    } else {
        return $null
    }
}

function Get-FileNameFromUrl($url) {
    $fileName = Extract-FileName $url
    if ($fileName) { return $fileName }

    $request = [System.Net.WebRequest]::Create($url)
    $request.Method = "HEAD"
    $request.Proxy = Get-Proxy $url
    $request.AllowAutoRedirect = $false
    try {
        $response = $request.GetResponse()
        if ($response.StatusCode -eq "Found") {
            $response.Close()
            return Get-FileNameFromUrl $response.GetResponseHeader("Location")
        }
        $disposition = $response.GetResponseHeader("Content-Disposition")
        if ($disposition) {
            $disposition = New-Object System.Net.Mime.ContentDisposition $disposition
            $response.Close()
            return $disposition.FileName
        }
        $response.Close()
        return $null
    } catch {
        Write-Warning $_.Exception.Message
        return $false
    }
}

function Check-AppResourceExists([string]$name) {
    $downloadDir = Get-ConfigPathValue DownloadDir
    $searchPattern = App-ResourceFile $name
    if (!$searchPattern) {
        $searchPattern = App-ResourceArchive $name
    }
    $path = Find-File $downloadDir $searchPattern
    if ($path) { return $true } else { return $false }
}

function Download([string]$name) {
    if (Check-AppResourceExists $name) {
        Debug "Resource for app $name allready exists."
        return
    }
    Debug "Downloading app resources for $name ..."
    $url = App-Url $name
    Debug "Downloading from URL $url"
    $url2 = Resolve-Url $url
    if ($url2 -ne $url) {
        Debug "Downloading from resolved URL $url2"
        $url = $url2
    }
    $fileName = Get-FileNameFromUrl $url
    if (!$fileName) {
        $fileName = (App-ResourceArchive $name).Replace('?', '_').Replace('*', '')
        Debug "Building filename from archive pattern..."
    }
    Debug "Downloading to file $fileName"
    $file = [IO.Path]::Combine((Get-ConfigPathValue DownloadDir), $fileName)
    if ($url -and $fileName) {
        if (!(Download-File $name $url $file)) {
            Write-Warning "Download failed: $url"
        }
    } else {
        Write-Warning "Could not resolve URL or filename."
    }
}

foreach ($name in $Script:apps) {
    if (!$name) { continue }
    if ((App-Typ $name) -eq "default") {
        try {
            Download $name
        } catch {
            Write-Warning "Failed to download app resources for ${name}: $($_.Exception.Message)"
            Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
        }
    }
}

Debug "Finished downloading app resources."

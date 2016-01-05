param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

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

function Download-String([string]$url) {
    Write-Host "Downloading page: $url ..."
    $attempt = 1
    while ($attempt -le (Get-ConfigValue DownloadAttempts)) {
        try {
            if ($attempt -gt 1) { Debug "Download attempt $attempt ..." }
            if (Get-ConfigValue UseProxy) {
                $proxyUrl = Get-ProxyUrl $url
                $data = Invoke-WebRequest -Uri $url -Proxy $proxyUrl
            } else {
                $data = Invoke-WebRequest -Uri $url
            }
            return $data
        } catch {
            Debug "Download failed $_"
            $attempt++
        }
    }
    return $null
}

function Download-File([string]$url, [string]$target) {
    Write-Host "Downloading file: $url ..."
    $attempt = 1
    while ($attempt -le (Get-ConfigValue DownloadAttempts)) {
        try {
            if ($attempt -gt 1) { Debug "Download attempt $attempt ..." }
            if (Get-ConfigValue UseProxy) {
                $proxyUrl = Get-ProxyUrl $url
                Invoke-WebRequest -Uri $url -OutFile $target -Proxy $proxyUrl
            } else {
                Invoke-WebRequest -Uri $url -OutFile $target
            }
            return $True
        } catch {
            Debug "Download failed $_"
            $attempt++
        }
    }
    return $False
}

function Is-RedirectPage($url) {
    return $url -match "^https?://downloads\.sourceforge\.net/"
}

function Get-RefreshUrl($url) {
    $http = Download-String $url
    if (!$http) {
        Write-Warning "Download failed: $url"
        return $null
    }
    Debug "Extracting refresh URL from download page ..."
    $p1 = [regex]"\<noscript\>[^\<]*\<meta\s[^\>]*?http-equiv=`"refresh`"[^\>]*?\scontent=`"\d+;\s+url=(?<url>.*?)`""
    $p2 = [regex]"\<noscript\>[^\<]*\<meta\s[^\>]*?content=`"\d+;\s+url=(?<url>.*?)`"[^\>]*?\shttp-equiv=`"refresh`""
    $m = $p1.Match($http)
    if (!$m.Success) { $m = $p2.Match($http) }
    if ($m.Success) {
        return $m.Groups["url"].Value.Replace("&amp;", "&")
    } else {
        Write-Warning "Extracting direct URL from download page failed"
        return $null
    }
}

function Extract-FileName($url) {
    [regex]$ex = "[^=/]+\.[a-zA-Z0-9]{2,3}(?:\?.*)?$"
    $m = $ex.Match($url)
    if ($m.Success) {
        return $m.Value
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

function Download($url) {
    $fileName = Get-FileNameFromUrl $url
    $file = [IO.Path]::Combine($Script:downloadDir, $fileName)
    if ($fileName -and ![IO.File]::Exists($file)) {
        if (Is-RedirectPage $url) {
            $url = Get-RefreshUrl $url
            if ($url) {
                if (!(Download-File $url $file)) {
                    Write-Warning "Download failed: $url"
                }
            }
        } elseif (!(Download-File $url $file)) {
            Write-Warning "Download failed: $url"
        }
    }
}

foreach ($name in $Script:apps) {
    $typ = Get-AppConfigValue $name Typ
    if ($typ -ieq "node-package") { continue }

    $url = Get-AppConfigValue $name Url
    if (!$url) {
        Debug "No URL for app $name"
        continue
    }
    Download $url
}

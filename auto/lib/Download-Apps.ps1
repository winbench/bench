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

function Get-Proxy($url) {
    if (Get-ConfigValue UseProxy) {
        $proxyUrl = Get-ProxyUrl $url
        return New-Object System.Net.WebProxy $proxyUrl
    } else {
        return $null
    }
}

function Extract-FileName($url) {
    [regex]$ex = "[^=/]+\.[a-zA-Z0-9]{2,3}$"
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

function Download-File($url, $target) {
    Write-Host "Downloading $url ..."
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

foreach ($name in $Script:apps) {
    $typ = Get-AppConfigValue $name Typ
    if ($typ -ieq "npm") { continue }

    $url = Get-AppConfigValue $name Url
    if (!$url) {
        Debug "No URL for app $name"
        continue
    }
    $fileName = Get-FileNameFromUrl $url
    $file = [IO.Path]::Combine($Script:downloadDir, $fileName)
    if ($fileName -and ![IO.File]::Exists($file)) {
        if (!(Download-File $url $file)) {
            Write-Warning "Download failed: $url"
        }
    }
}

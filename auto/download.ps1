param ([switch]$debug)

$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$autoDir\common.lib.ps1"
. "$autoDir\config.lib.ps1"
. "$autoDir\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

$apps = Get-ConfigValue Apps

$rootDir = Resolve-Path "$autoDir\.."
$downloadDir = Safe-Dir "$rootDir\$(Get-ConfigValue DownloadDir)"

function Get-FileNameFromUrl($url) {
    Debug "Resolving URL: $url"
    $request = [System.Net.WebRequest]::Create($url)
    $request.AllowAutoRedirect=$false
    $response = $request.GetResponse()

    if ($response.StatusCode -eq "Found")
    {
        $location = $response.GetResponseHeader("Location")
        Debug "Resolved to location: $location"
        return [IO.Path]::GetFileName($location)
    } else {
        return [IO.Path]::GetFileName($url)
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
    Debug "Downloading $url ..."
    $attempt = 1
    while ($attempt -le (Get-ConfigValue DownloadAttempts)) {
        try {
            if ($attempt -gt 1) { Debug "Download attempt $attempt ..." }
            Invoke-WebRequest -Uri $url -OutFile $target
            return $True
        } catch {
            Debug "Download failed $_"
            $attempt++
        }
    }
    return $False
}

foreach ($name in $apps) {
    $url = Get-ConfigValue "${name}Url"
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

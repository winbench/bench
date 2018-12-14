param (
    [switch]$CheckVersion,
    [string[]]$Libraries = @("core", "default"),
    [string]$ReportFile = "app-report.txt"
)

$Script:scriptsDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsDir\config.lib.ps1"

$DebugPreference = "SilentlyContinue"
$VerbosePreference = "SilentlyContinue"
$InformationPreference = "Continue"

$GitHubUserName = Read-Host "GitHub Username"
if ($GitHubUserName) {
  $GitHubPassword = Read-Host "GitHub Password" -AsSecureString
  $GitHubPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($GitHubPassword))
}

$_ = [Reflection.Assembly]::LoadFrom("$Script:scriptsDir\..\bin\HtmlAgilityPack.dll")

$Script:web = New-Object HtmlAgilityPack.HtmlWeb
[Net.ServicePointManager]::SecurityProtocol = 'Tls11,Tls12'

function AuthHeaders($user, $pass) {
    if ($user) {
        $pair = "${user}:${pass}"
        $encodedCreds = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($pair))
        return @{ Authorization = "Basic $encodedCreds" }
    } else {
        return @{}
    }
}

function findVersionsInNode($app, $node, $attr, $re) {
    if ($attr) {
        $text = $node.Attributes[$attr].Value
    } else {
        $text = $node.InnerText
    }
    foreach ($m in $re.Matches($text)) {
        $m.Groups["Version"].Value
    }
}

function findVersionsInDoc($app, $doc, $xpath, $re) {
    $attrMatch = ([regex]"@\w+$").Match($xpath)
    if ($attrMatch.Success) {
        $attr = $attrMatch.Value.Substring(1)
        $xpath = $xpath.Substring(0, $attrMatch.Index - 1)
    }
    if ($xpath) {
        $nodes = $doc.DocumentNode.SelectNodes($xpath)
        foreach ($n in $nodes) {
            findVersionsInNode $app $n $attr $re
        }
    } else {
        findVersionsInNode $app $doc.DocumentNode $re
    }
}

function normalizeVersion($version) {
    $parts = $version.Split('.')
    $norms = $parts | % { $_.PadLeft(6, '0') }
    return [string]::Join(".", $norms)
}

function findHighestAppVersion($app) {
    $checkUrl = Get-AppConfigValue $app.ID "VersionCheckUrl"
    if (!$checkUrl) { return $false }
    $checkPattern = Get-AppConfigValue $app.ID "VersionCheckPattern"
    if (!$checkPattern) { return $false }
    $checkXPath = Get-AppConfigValue $app.ID "VersionCheckXPath"
    Write-Host ""
    Write-Host "Checking $($app.AppLibrary.ID):$($app.ID) ..."
    [regex]$checkRe = $checkPattern
    $doc = $Script:web.Load($checkUrl)
    $versions = findVersionsInDoc $app $doc $checkXPath $checkRe `
        | sort -Descending -Property { normalizeVersion $_ }
    if ($versions -is [string]) {
        $version = $versions
        Write-Information "Found one version: $version"
        return $version
    }
    if ($versions) {
        Write-Information "Found versions: $([string]::Join(", ", $versions))"
        return $versions[0]
    }
    return $null
}

function findLatestGitHubRelease($app) {
    $url = Get-AppConfigValue $app.ID "Url"
    if (!$url) { return $false }
    [regex]$p = "^https?\://github\.com/(?<Owner>[^/]+)/(?<Project>[^/]+)/releases/download/"
    $m = $p.Match($url)
    if (!$m.Success) { return $false }
    $owner = $m.Groups["Owner"].Value
    $project = $m.Groups["Project"].Value
    Write-Host ""
    Write-Host "Checking $($app.AppLibrary.ID):$($app.ID) on GitHub $owner/$project ..."
    $apiUrl = "https://api.github.com/repos/$owner/$project/releases"
    $authHeaders = AuthHeaders $Script:GitHubUserName $Script:GitHubPassword
    $releases = Invoke-WebRequest $apiUrl -Headers $authHeaders | ConvertFrom-Json
    $tags = $releases `
        | ? { !$_.draft -and !$_.prerelease } `
        | % { $_.tag_name } `
        | % { if ($_.StartsWith("v")) { $_.Substring(1) } else { $_ } } `
        | sort -Descending -Property { normalizeVersion $_ }
    if ($tags -is [string]) {
        $tag = $tags
        Write-Information "Found one release: $tag"
        return $tag
    }
    if ($tags) {
        Write-Information "Found releases: $([string]::Join(", ", $tags))"
        return $tags[0]
    }
    return $null
}

function report($msg) {
    $msg | Out-File $Script:ReportFile -Append
}

# ---------------------------------------------------------------------------------------

report "[$([DateTime]::Now.ToString("yyyy-MM-dd HH:mm:ss"))]"
report "Check Version: $CheckVersion"

foreach ($app in $global:BenchConfig.Apps) {
    if ($app.AppLibrary.ID -notin $Libraries) { continue }

    if ($CheckVersion -and $app.IsVersioned) {
        $currentVersion = findHighestAppVersion $app
        if ($currentVersion -eq $false) {
            $currentVersion = findLatestGitHubRelease $app
        }
        if ($currentVersion -eq $null) {
            Write-Warning "Version: $($app.Version) -> ???"
            report "Version: $($app.AppLibrary.ID) $($app.ID) $($app.Version) -> ???"
        } elseif ($currentVersion) {
            if ($app.Version -ne $currentVersion) {
                Write-Warning "$($app.Version) -> $currentVersion"
                report "Version: $($app.AppLibrary.ID) $($app.ID) $($app.Version) -> $currentVersion"
            } else {
                Write-Host "$currentVersion (unchanged)"
            }
        }
    }

}

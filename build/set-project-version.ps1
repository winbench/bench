param (
    $Version = $null
)

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$RootDir = Resolve-Path "$myDir\.."
$utf8 = New-Object System.Text.UTF8Encoding($false) # UTF8 without BOM

#
# Update version.txt
#

$versionFile = "$RootDir\res\version.txt"
$currentVersion = [IO.File]::ReadAllText($versionFile).Trim()
echo "Current Version: $currentVersion"
if ($Version) {
    $newVersion = $Version
} else {
    $newVersion = Read-Host "New Version"
}
echo ""
echo "- version.txt"
[IO.File]::WriteAllText($versionFile, $newVersion)

#
# Update bench-install.bat
#

echo "- bench-install.bat"
$installFile = "$RootDir\res\bench-install.bat"
$versionPattern = "^SET VERSION=[\d\.]+\s*$"
$versionRegex = New-Object System.Text.RegularExpressions.Regex($versionPattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)

$installCode = [IO.File]::ReadAllText($installFile)
$installCode = $versionRegex.Replace($installCode, "SET VERSION=${newVersion}")
[IO.File]::WriteAllText($installFile, $installCode)

#
# Update CHANGELOG.md
#

echo "- CHANGELOG.md"
$changelogFile = "$RootDir\CHANGELOG.md"
$repoUrl = "https://github.com/winbench/bench"
$changeLogText = [IO.File]::ReadAllText($changelogFile, $utf8)
$versionPattern = "^\[(?<version>[\d\.]+)\]: \S*\s*$"
$versionRegex = New-Object System.Text.RegularExpressions.Regex($versionPattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
$latestVersion = $versionRegex.Matches($changeLogText) `
    | % { New-Object System.Version $_.Groups["version"].Value } `
    | sort -Descending `
    | Select-Object -First 1
echo "    Last Entry: $latestVersion"

function insertReleaseIntoChangelog($latestVersion, $newVersion)
{
    begin
    {
        $insertFinished = $false
        $ts = [DateTime]::Now.ToString("yyyy-MM-dd")
    }
    process
    {
        if ($insertFinished) { $_ }
        else
        {
            if ($_.StartsWith("[Unreleased]: "))
            {
                $_
                ""
                "## [${newVersion}] - $ts"
                ""
                "[${newVersion}]: $repoUrl/compare/v${latestVersion}...v${newVersion}"
            }
            else { $_ }
        }
    }
}

if ($latestVersion -ne $newVersion)
{
    $changelogLines = [IO.File]::ReadAllLines($changelogFile, $utf8)
    $changeLogLines = $changeLogLines | insertReleaseIntoChangelog $latestVersion $newVersion
    [IO.File]::WriteAllLines($changeLogFile, $changeLogLines, $utf8)
}

#
# Update version in AssemblyInfo.cs files
#

$NamePattern = "AssemblyInfo.cs"

$files = Get-ChildItem -Path "$RootDir\BenchManager" -Directory -Recurse `
    | ? { $_.FullName -notlike "$RootDir\*\packages*" -and $_.FullName -notlike "$RootDir\*\BenchLib.Test*" } `
    | Get-ChildItem -File -Filter $NamePattern

$versionPattern = "^\s*\[assembly: (?<type>Assembly(?:File)?Version)\(`"(?<version>.+?)`"\)\]\s*`$"
$versionRegex = New-Object System.Text.RegularExpressions.Regex($versionPattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)

function Replace-Version($newVersion)
{
    process
    {
        $code = Get-Content -Path $_.FullName -Encoding UTF8 | Out-String
        "    - $($_.FullName):"
        $matches = $versionRegex.Matches($code)
        foreach ($m in $matches)
        {
            "        $($m.Groups["type"]): $($m.Groups["version"]) -> $newVersion"
        }
        $code = $versionRegex.Replace($code, "[assembly: `$1(`"$newVersion`")]")
        $code | Set-Content -Path $_.FullName -Encoding UTF8
    }
}

echo "- assemblies"
$files | Replace-Version "${newVersion}.0"

#
# Hint for changelog
#
echo ""
write-warning "Remember to check the CHANGELOG.md for correctness!"
echo ""
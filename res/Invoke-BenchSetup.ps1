param (
    $TargetDir = "bench",
    $BenchSetup = "BenchSetup.exe",
    $Configuration = @{},
    $ActivatedApps = @(),
    $DeactivatedApps = @()
)

$MyDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

if (![IO.Path]::IsPathRooted($TargetDir)) {
    $TargetDir = [IO.Path]::Combine((Get-Location), $TargetDir)
}
$BenchCLI = "$TargetDir\auto\bin\bench.exe"
if (![IO.Path]::IsPathRooted($BenchSetup)) {
    $BenchSetup = [IO.Path]::Combine((Get-Location), $BenchSetup)
}

$DefaultConfiguration = @{
    "OverrideHome" = "true"
    "OverrideTemp" = "true"
    "IgnoreSystemPath" = "true"
    "UseRegistryIsolation" = "true"
    "RegisterInUserProfile" = "false"
    "AppLibs" = @{
        "core" = "github:mastersign/bench-apps-core"
        "default" = "github:mastersign/bench-apps-default"
    }
    "QuickAccessCmd" = "true"
    "QuickAccessPowerShell" = "true"
    "QuickAccessBash" = "true"
    "AutoUpdateCheck" = "false"
}

$UserConfiguration = $DefaultConfiguration
foreach ($key in $Configuration.Keys)
{
    $UserConfiguration[$key] = $Configuration[$key]
}
$UserConfiguration["WizzardStartAutoSetup"] = "false"

function Out-Utf8File ($filePath)
{
    begin
    {
        $utf8WithoutBom = New-Object System.Text.UTF8Encoding ($false)
        $w = New-Object System.IO.StreamWriter ($filePath, $false, $utf8WithoutBom)
    }
    process
    {
        $w.WriteLine($_)
    }
    end
    {
        $w.Close()
    }
}

function Format-ConfigEntry ()
{
    process
    {
        [string]$key = $_.Key
        $value = $_.Value
        if ($value -is [Collections.Hashtable])
        {
            "* ${key}:"
            $value.Keys | % { "    + ``$_``: ``$($value[$_])``" }
        }
        elseif ($value -is [object[]] -and !($value -is [string]))
        {
            "* ${key}:"
            $value | % { "    + ``$_``" }
        }
        else
        {
            "* ${key}: ``$value``"
        }
    }
}

function Write-MarkdownConfig ($filePath, [Collections.Hashtable]$config)
{
    $config.GetEnumerator() `
        | Format-ConfigEntry `
        | Out-Utf8File $filePath
}

if (Test-Path $TargetDir)
{
    if ((Get-ChildItem $TargetDir).Count -gt 0)
    {
        Write-Error "The target directory is not empty"
        exit 1
    }
}
else
{
    mkdir $TargetDir | Out-Null
}

mkdir "$TargetDir\config" | Out-Null
Write-MarkdownConfig "$TargetDir\config\config.md" $UserConfiguration
$ActivatedApps | Out-Utf8File "$TargetDir\config\apps-activated.txt"
$DeactivatedApps | Out-Utf8File "$TargetDir\config\apps-deactivated.txt"

Start-Process -Wait -FilePath $BenchSetup -ArgumentList "--target-dir", "`"$TargetDir`"", "--extract-only"
if (!$?)
{
    Write-Error "Failed to extract Bench to the target directory."
    exit 1
}

& $BenchCLI --verbose --root $TargetDir manage load-app-libs
if (!$?)
{
    Write-Error "Failed to load the Bench app libraries."
    exit 1
}

& $BenchCLI --verbose --root $TargetDir manage setup
if (!$?)
{
    Write-Error "Failed to setup the Bench environment."
    exit 1
}

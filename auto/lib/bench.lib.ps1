$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\profile.ps1"
. "$myDir\config.lib.ps1"

function Purge-Dir ($path)
{
    [Mastersign.Bench.FileSystem]::PurgeDir($path)
}

function Safe-Dir ($path)
{
    return [Mastersign.Bench.FileSystem]::AsureDir($path)
}

function Empty-Dir ($path)
{
    return [Mastersign.Bench.FileSystem]::EmptyDir($path)
}

function Find-Files($dir, $pattern)
{
    if (![IO.Directory]::Exists($dir))
    {
        return @()
    }
    return [IO.Directory]::GetFiles($dir, $pattern)
}

function Find-File($dir, $pattern)
{
    $files = Find-Files $dir $pattern
    if ($files -is [string])
    { $files = @($files) }
    if ($files.Count -gt 0)
    {
        $file = $files[0]
        if ($files.Count -gt 1)
        {
            Debug "Choose $file from $($files.Count) choices"
        }
        else
        {
            Debug "Choose $file"
        }
        return $file
    }
    else
    {
        Debug "Found no file for pattern '$pattern'"
        return $null
    }
}

function Get-ProjectName ($name)
{
    if ([IO.Path]::IsPathRooted($name))
    {
        return [IO.Path]::GetFileName($name)
    }
    else
    {
        return $name
    }
}

function Get-ProjectPath ($name)
{
    Debug "Get-ProjectPath( $name )"
    if ([IO.Path]::IsPathRooted($name))
    {
        return Resolve-Path $name
    }
    else
    {
        Debug "Resolving project dir for: $name"
        $projectRoot = Safe-Dir (Get-ConfigValue ProjectRootDir)
        $path = Resolve-Path "$projectRoot\$name"
        Debug "Resolved to: $path"
        return $path
    }
}

function Get-ProjectVersion ($name)
{
    Debug "Get-ProjectVersion( $name )"
    $path = Get-ProjectPath $name
    $packageInfoFile = [IO.Path]::Combine($path, "package.json")
    if (Test-Path $packageInfoFile)
    {
        [System.Reflection.Assembly]::LoadWithPartialName("System.Web.Extensions") | Out-Null
        $json = [IO.File]::ReadAllText($packageInfoFile, [Text.Encoding]::UTF8)
        $ser = New-Object System.Web.Script.Serialization.JavaScriptSerializer
        $packageInfo = $ser.DeserializeObject($json)
        Debug "Found package.json with version $($packageInfo["version"])"
        return $packageInfo["version"]
    }
    else
    {
        Debug "No package.json found"
        return $null
    }
}

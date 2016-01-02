$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"
. "$myDir\config.lib.ps1"
. "$myDir\fs.lib.ps1"

function Get-ProjectName ($name) {
    if ([IO.Path]::IsPathRooted($name)) {
        return [IO.Path]::GetFileName($name)
    } else {
        return $name
    }
}

function Get-ProjectPath ($name) {
    Debug "Get-ProjectPath( $name )"
    if ([IO.Path]::IsPathRooted($name)) {
        return Resolve-Path $name
    } else {
        Debug "Resolving project dir for: $name"
        $projectRoot = Safe-Dir (Get-ConfigPathValue ProjectRootDir)
        $path = Resolve-Path "$projectRoot\$name"
        Debug "Resolved to: $path"
        return $path
    }
}

function Get-ProjectVersion ($name) {
    Debug "Get-ProjectVersion( $name )"
    $path = Get-ProjectPath $name
    $packageInfoFile = [IO.Path]::Combine($path, "package.json")
    if (Test-Path $packageInfoFile) {
        [System.Reflection.Assembly]::LoadWithPartialName("System.Web.Extensions") | Out-Null
        $json = [IO.File]::ReadAllText($packageInfoFile, [Text.Encoding]::UTF8)
        $ser = New-Object System.Web.Script.Serialization.JavaScriptSerializer
        $packageInfo = $ser.DeserializeObject($json)
        Debug "Found package.json with version $($packageInfo["version"])"
        return $packageInfo["version"]
    } else {
        Debug "No package.json found"
        return $null
    }
}

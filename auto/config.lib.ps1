$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

$config = @{}

function Set-ConfigValue($name, $value) {
    # Add-Member -InputObject $Script:cfg -MemberType NoteProperty -Name $name -Value $value
    if ($Script:debug) {
        Write-Host "[DEBUG] Config: $name = $value"
    }
    $Script:config[$name] = $value
}

function Get-ConfigValue($name, $def = $null) {
    if ($Script:config.ContainsKey($name)) {
        return $Script:config[$name]
    } else {
        return $def
    }
}

# Common
Set-ConfigValue DownloadDir "res\download"
Set-ConfigValue LibDir "lib"
Set-ConfigValue HomeDir "home"
Set-ConfigValue AppDataDir "$(Get-ConfigValue HomeDir)\AppData"
Set-ConfigValue LocalAppDataDir "$(Get-ConfigValue HomeDir)\LocalAppData"

# 7Zip
Set-ConfigValue SvZArchive "7za*.zip"
Set-ConfigValue SvZDir "7z"
Set-ConfigValue SvZPath "$(Get-ConfigValue SvZDir)"
Set-ConfigValue SvZExe "$(Get-ConfigValue SvZPath)\7za.exe"

#NodeJS
Set-ConfigValue NodeDir "NodeJS"
Set-ConfigValue NodePath "$(Get-ConfigValue NodeDir)"
Set-ConfigValue NodeExe "node.exe"
Set-ConfigValue NpmArchive "npm-*.zip"

# Git
Set-ConfigValue GitArchive "PortableGit-*-32-bit.7z.exe"
Set-ConfigValue GitDir "git"
Set-ConfigValue GitPath "$(Get-ConfigValue GitDir)\bin"
Set-ConfigValue GitExe "$(Get-ConfigValue GitPath)\git.exe"

param ($message = $null)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"

Clear-Host
Get-Content "$scriptsLib\banner.txt" -Encoding UTF8 `
    | % { $_.Replace("`$VERSION`$", [string]::Format("{0,9}", $(Get-ConfigValue Version "0.0.0"))) } `
    | Out-Default

if ($message) {
    $prefix = "====#### "
    $postfix = " ####===="
    $message = $message.Trim()
    $d = 60 - $prefix.Length - $postfix.Length - $message.Length
    if ($d -lt 0) {
        $text = $prefix + $message + $postfix
    } else {
        $l = "-" * [Math]::Floor($d / 2.0)
        $r = "-" * [Math]::Ceiling($d / 2.0)
        $text = $l + $prefix + $message + $postfix + $r
    }
    Write-Output $text
    Write-Output ""
}

powershell -NoLogo -NoProfile -NoExit

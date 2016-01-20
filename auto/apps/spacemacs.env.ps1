$homeDir = Get-ConfigPathValue HomeDir
$emacsInitFile = [IO.Path]::Combine($homeDir, ".emacs")
$emacsUserDir = [IO.Path]::Combine($homeDir, ".emacs.d")

$nl = [Environment]::NewLine
$preamble = ";; BENCH SPACEMACS"

if (Test-Path $emacsInitFile -PathType Leaf) {
    [string]$oldCode = [IO.File]::ReadAllText($emacsInitFile, [Text.Encoding]::UTF8)
    if (!$oldCode.StartsWith($preamble)) {
        Write-Warning "Could not setup Spacemacs initialization, because an unknown .emacs file exists in the home directory."
        return
    }
}

function formatPath ([string]$path) {
    return $path.Replace('\', '/')
}

$initCode = "$preamble$nl$nl"
$initCode += ";; Set path to Spacemacs explicitly, because the recognition$nl"
$initCode += ";; of the directory '%HOME%\.emacs.d' does not work correctly$nl"
$initCode += ";;  with overridden HOME environment variable.$nl$nl"
$initCode += "(setq user-emacs-directory `"$(formatPath $emacsUserDir)/`")$nl"
$initCode += "(setq user-init-file `"$(formatPath $emacsUserDir)/init.el`")$nl"
$initCode += "(load user-init-file)$nl"

Debug "Updating Emacs init file: $emacsInitFile"

[IO.File]::WriteAllText($emacsInitFile, $initCode, [Text.Encoding]::UTF8)

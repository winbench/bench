$homeDir = Get-ConfigValue HomeDir
$spacemacsConfig = [IO.Path]::Combine($homeDir, ".spacemacs")
$spacemacsConfigDir = [IO.Path]::Combine($homeDir, ".spacemacs.d")
$spacemacsDir = [IO.Path]::Combine($homeDir, ".emacs.d")

if ((Test-Path $spacemacsConfig) -or (Test-Path $spacemacsConfigDir))
{
    if (Test-Path $spacemacsDir)
    {
        Purge-Dir $spacemacsDir
    }
}

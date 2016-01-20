$spacemacsResourceDir = "$(Get-ConfigPathValue AppResourceBaseDir)\spacemacs"
$git = App-Exe Git

if (!$git) { throw "Git not found" }

$homeDir = Get-ConfigPathValue HomeDir
$emacsd = [IO.Path]::Combine($homeDir, ".emacs.d")
$spacemacsInit = [IO.Path]::Combine($homeDir, ".spacemacs")
$spacemacsInitTemplate = Resolve-Path "$spacemacsResourceDir\.spacemacs"

if (!(Test-Path $spacemacsInit -PathType Leaf)) {
    Write-Host "Copying Spacemacs default init file ..."
    cp $spacemacsInitTemplate $spacemacsInit
}
if (!(Test-Path $emacsd -PathType Container)) {
    Write-Host "Cloning Spacemacs ..."
    Start-Process -Wait -NoNewWindow $git @("clone", "https://github.com/syl20bnr/spacemacs.git", $emacsd)
    Write-Host ""
    Write-Host "Run 'emacs' once to initialize and start Spacemacs."
}

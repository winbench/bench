$spacemacsResourceDir = "$(Get-ConfigValue AppResourceBaseDir)\spacemacs"
$emacsDir = App-Path Emacs
$git = App-Exe Git

if (!$git) { throw "Git not found" }

function Run-Git ($arguments) {
    Start-Process $git -Wait -NoNewWindow $arguments
}

$homeDir = Get-ConfigValue HomeDir
$spacemacsConfig = [IO.Path]::Combine($homeDir, ".spacemacs")
$spacemacsConfigDir = [IO.Path]::Combine($homeDir, ".spacemacs.d")
$spacemacsInitFile = [IO.Path]::Combine($spacemacsConfigDir, "init.el")
$spacemacsInitTemplate = Resolve-Path "$spacemacsResourceDir\init.el"

if (!(Test-Path $spacemacsConfig -PathType Leaf) -and !(Test-Path $spacemacsConfigDir -PathType Container)) {
    Write-Host "Initializing Spacemacs configuration ..."
    mkdir $spacemacsConfigDir | Out-Null
    cp $spacemacsInitTemplate $spacemacsInitFile
    pushd $spacemacsConfigDir | Out-Null
    Run-Git @("init")
    Run-Git @("add", "-A", ":/")
    Run-Git @("commit", "-m", '"Default Spacemacs configuration from Bench template"')
    popd | Out-Null
}

$spacemacsDir = [IO.Path]::Combine($homeDir, ".emacs.d")

if (!(Test-Path $spacemacsDir -PathType Container)) {
    Write-Host "Cloning Spacemacs ..."
    Run-Git @("clone", "https://github.com/syl20bnr/spacemacs.git", $spacemacsDir)
    Write-Host ""
    Write-Host "Run 'emacs' once to initialize and start Spacemacs."
}

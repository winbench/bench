$python = App-Exe Python3
if (!$python) { throw "Python3 not found" }

$pythonDir = App-Dir Python3

$pythonWrapper = [IO.Path]::Combine($pythonDir, "python3.cmd")
if (!(Test-Path $pythonWrapper -PathType Leaf)) {
    Write-Host "Creating wrapper to call Python 3 via 'python3' ..."
    "@CALL `"%~dp0\python.exe`" %*" | Out-File $pythonWrapper -Encoding default
}

$pipPackageDir = [IO.Path]::Combine($pythonDir, "lib", "site-packages", "pip")
if (!(Test-Path $pipPackageDir -PathType Container)) {
    Write-Host "Setting up PIP ..."
    & $python -m ensurepip
}

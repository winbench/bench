$python = App-Exe Python2
if (!$python) { throw "Python2 not found" }

$pythonDir = App-Dir Python2

$pythonWrapper = [IO.Path]::Combine($pythonDir, "python2.cmd")
if (!(Test-Path $pythonWrapper)) {
    Write-Host "Creating wrapper to call Python 2 via 'python2' ..."
    "@CALL `"%~dp0\python.exe`" %*" | Out-File $pythonWrapper -Encoding default
}

$pipPackageDir = [IO.Path]::Combine($pythonDir, "lib", "site-packages", "pip")
if (!(Test-Path $pipPackageDir -PathType Container)) {
    Write-Host "Setting up PIP ..."
    & $python -m ensurepip
    pushd $pythonDir
    & $python -m pip install --upgrade setuptools
    & $python -m pip install --upgrade pip
    popd
}

$python = App-Exe Python2
if (!$python) { throw "Python2 not found" }

$pythonDir = App-Dir Python2

& $python -m ensurepip

$python = App-Exe Python3
if (!$python) { throw "Python3 not found" }

$pythonDir = App-Dir Python3

& $python -m ensurepip

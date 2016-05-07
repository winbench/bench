$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$binDir = Resolve-Path ([IO.Path]::Combine([IO.Path]::Combine($myDir, ".."), "bin"))

function Load-Assembly($dir, $name)
{
    $file = [IO.Path]::Combine($dir, $name)
    $_ = [Reflection.Assembly]::LoadFrom($file)
}

Load-Assembly $binDir "BenchLib.dll"

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$binDir = Resolve-Path ([IO.Path]::Combine([IO.Path]::Combine($myDir, ".."), "bin"))

function Load-Assembly($dir, $name)
{
    $file = [IO.Path]::Combine($dir, $name)
    $_ = [Reflection.Assembly]::LoadFile($file)
}

Load-Assembly $binDir "Ionic.Zip.dll"
Load-Assembly $binDir "Interop.IWshRuntimeLibrary.dll"
Load-Assembly $binDir "BenchLib.dll"

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)

$assemblyPath = "$rootDir\BenchManager\BenchLib\bin\Debug\BenchLib.dll"

if (!(Test-Path $assemblyPath))
{
    & "$myDir\build-debug.ps1"
}

$assembly = Resolve-path $assemblyPath
$outDir = "$rootDir\docs\content\clr-api"
if (Test-Path $outDir -PathType Container)
{
    del "$outDir\*" -Recurse -Force
}

& "$myDir\xmldoc2md\xmldoc2md.ps1" `
    -TargetPath "$myDir\..\docs\content\clr-api" `
    -Assemblies @($assembly) `
    -UrlBase "/clr-api/" `
    -UrlFileNameExtension "/" `
    -Title "BenchLib" `
    -Author "Tobias Kiertscher" `
    -Copyright "Licensed by CC-BY-4.0" `
    -NoTitleHeadline `
    -MetaDataStyle Hugo

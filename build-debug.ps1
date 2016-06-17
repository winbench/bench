$rootDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

& "$rootDir\build.ps1" -Mode Debug -MsBuildVerbosity normal -NoRelease

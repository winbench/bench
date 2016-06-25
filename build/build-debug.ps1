$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

& "$myDir\build.ps1" -Mode Debug -MsBuildVerbosity normal -NoRelease

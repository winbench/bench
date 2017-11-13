$MyDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$sdkSetupUrl = "https://go.microsoft.com/fwlink/p/?LinkId=226658"

$tmpDir = "$MyDir\..\tmp"
if (!(Test-Path $tmpDir))
{
    $_ = mkdir $tmpDir
}

Invoke-WebRequest $sdkSetupUrl -OutFile "$tmpDir\sdksetup.exe"

$sdkSetup = Resolve-Path "$tmpDir\sdksetup.exe"

& $sdkSetup /ceip off /uninstall

$MyDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$sdkSetupUrl = "https://go.microsoft.com/fwlink/p/?LinkId=838916" # win 10 1607 14393  .NET 4.6.2
$sdkFeature = "OptionId.NetFxSoftwareDevelopmentKit"

$tmpDir = "$MyDir\..\tmp"
if (!(Test-Path $tmpDir))
{
    $_ = mkdir $tmpDir
}

Invoke-WebRequest $sdkSetupUrl -OutFile "$tmpDir\sdksetup.exe"

$sdkSetup = Resolve-Path "$tmpDir\sdksetup.exe"

& $sdkSetup /ceip off /features $sdkFeature

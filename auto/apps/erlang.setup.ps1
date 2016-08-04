$erlangDir = App-Dir Erlang

$majorVersion = Get-AppConfigValue Erlang VersionMajor
$ertsVersion = Get-AppConfigValue Erlang ErtsVersion

$binDir = "$erlangDir\bin"
$ertsDir = "$erlangDir\erts-${ertsVersion}"
$releaseDir = "$erlangDir\releases\${majorVersion}"

if (!(Test-Path $binDir))
{
    $_ = mkdir $binDir
    cp "$ertsDir\bin\ct_run.exe" $binDir
    cp "$ertsDir\bin\dialyzer.exe" $binDir
    cp "$ertsDir\bin\erl.exe" $binDir
    cp "$ertsDir\bin\erlc.exe" $binDir
    cp "$ertsDir\bin\escript.exe" $binDir
    cp "$ertsDir\bin\typer.exe" $binDir
    cp "$ertsDir\bin\werl.exe" $binDir
    cp "$releaseDir\no_dot_erlang.boot" $binDir
    cp "$releaseDir\start.boot" $binDir
    cp "$releaseDir\start_clean.boot" $binDir
    cp "$releaseDir\start_sasl.boot" $binDir
}

Purge-Dir "$erlangDir\`$PLUGINDIR"
del "$erlangDir\Install.*"
del "$erlangDir\Uninstall.*"
del "$erlangDir\*.template"

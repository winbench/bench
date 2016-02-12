$pgResourceDir = "$(Get-ConfigPathValue AppResourceBaseDir)\postgresql"
$pgPath = App-Path PostgreSQL

$dataDir = Get-AppConfigPathValue PostgreSQL PostgreSqlDataDir
$logFile = Get-AppConfigPathValue PostgreSQL PostgreSqlLogFile

if (!(Test-Path $dataDir -PathType Container)) {
    Write-Host "Initializing PostgreSQL database in $dataDir"
    pushd $pgPath
    .\initdb.exe "--pgdata=$dataDir" "--username=postgres" "--pwfile=$pgResourceDir\defaultpw.txt" | Out-File $logFile -Encoding OEM
    popd
    Write-Host "Login to PostgreSQL with user 'postgres' and password 'bench'."
    if ($LASTEXITCODE -ne 0) {
        throw "Error during initialization of the PostgreSQL data directory: Exit Code = $LASTEXITCODE."
    }
}

$regFile = Get-AppRegistryFileName PostgreSQL bench
if (!(Test-Path $regFile)) {
    cp "$pgResourceDir\default.reg" $regFile
    Write-Host "Initialize default registry backup for pgAdmin III."
}

if (!(Test-Path "$pgPath\postgresql_start.cmd")) {
    cp "$pgResourceDir\postgresql_start.cmd" $pgPath
    Write-Host "Run 'postgresql_start' on the Bench shell to start the PostgreSQL server."
}
if (!(Test-Path "$pgPath\postgresql_stop.cmd")) {
    cp "$pgResourceDir\postgresql_stop.cmd" $pgPath
    Write-Host "Run 'postgresql_stop' on the Bench shell to stop a running PostgreSQL server."
}
if (!(Test-Path "$pgPath\postgresql_log.cmd")) {
    cp "$pgResourceDir\postgresql_log.cmd" $pgPath
    Write-Host "Run 'postgresql_log' to open the PostgreSQL log file in the system editor."
}

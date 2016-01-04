$mysqlResourceDir = "$(Get-ConfigPathValue AppResourceBaseDir)\mysql"
$mysqlDir = App-Dir MySQL
$mysqlPath = App-Path MySQL
$dataDir = "$(Get-ConfigPathValue HomeDir)\mysql_data"

if (!(Test-Path $dataDir -PathType Container)) {
    $_ = mkdir $dataDir
    $logFile = "$dataDir\$env:COMPUTERNAME.err"
    if (Test-Path $logFile) {
        del $logFile
    }
    & "$mysqlPath\mysqld.exe" --initialize --init-file "$mysqlResourceDir\init.sql" --log_syslog=0 "--basedir=$mysqlDir" "--datadir=$dataDir"
}

if (!(Test-Path "$mysqlPath\mysql_start.cmd")) {
    cp "$mysqlResourceDir\mysql_start.cmd" $mysqlPath
    Write-Host "Run 'mysql_start' on the Bench shell to start the MySQL server."
}
if (!(Test-Path "$mysqlPath\mysql_stop.cmd")) {
    cp "$mysqlResourceDir\mysql_stop.cmd" $mysqlPath
    Write-Host "Run 'mysql_stop' on the Bench shell to stop a running MySQL server."
}
if (!(Test-Path "$mysqlPath\mysql_log.cmd")) {
    cp "$mysqlResourceDir\mysql_log.cmd" $mysqlPath
    Write-Host "Run 'mysql_log' to open the MySQL log file in the system editor."
}

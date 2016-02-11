@ECHO OFF
SET mysql_root=%~dp0\..
CD "%mysql_root%"
START mysqld --log_syslog=0 "--basedir=%mysql_root%" "--datadir=%MYSQL_DATA%"
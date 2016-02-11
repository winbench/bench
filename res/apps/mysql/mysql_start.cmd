@ECHO OFF
SET mysql_root=%~dp0\..
SET data_dir=%HOME%\mysql_data
CD "%mysql_root%"
START mysqld --log_syslog=0 "--basedir=%mysql_root%" "--datadir=%data_dir%"
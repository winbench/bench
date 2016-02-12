@ECHO OFF
SET pgsql_root=%~dp0\..
CD "%pgsql_root%"
pg_ctl  start -w --log "%PG_LOG%"

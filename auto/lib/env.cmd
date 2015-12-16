@ECHO OFF
REM **** MD Bench Environment Setup ****

SET HTTP_PROXY=http://10.132.192.83:3129/
SET HTTPS_PROXY=http://10.132.192.83:3129/
SET USERPROFILE=C:\kit1bbh_d\Projects\bench\home
SET HOMEDRIVE=C:
SET HOMEPATH=\kit1bbh_d\Projects\bench\home
SET APPDATA=C:\kit1bbh_d\Projects\bench\home\AppData
SET LOCALAPPDATA=C:\kit1bbh_d\Projects\bench\home\LocalAppData
SET BENCH_HOME=C:\kit1bbh_d\Projects\bench
SET L=C:\kit1bbh_d\Projects\bench\lib
SET BENCH_PATH=C:\kit1bbh_d\Projects\bench\auto\lib;%L%\7z;%L%\git\bin;%L%\node;%L%\python;%L%\code;%L%\sublime;%L%\pandoc;%L%\graphviz\release\bin;%L%\inkscape;%L%\miktex\miktex\bin
SET PATH=%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0
SET PATH=%BENCH_PATH%;%PATH%

@ECHO ON
SET ROOT=%~dp0
SET ZIPURL=https://github.com/mastersign/bench/archive/master.zip
SET ZIPFILE=%ROOT%bench.zip

ECHO.Download ZIP archive ...
CD "%ROOT%"
powershell -NoLogo -NoProfile -C "& { (New-Object System.Net.WebClient).DownloadFile('%ZIPURL%', '%ZIPFILE%') }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Download failed.
  PAUSE
  EXIT /B 1
)

ECHO.Extracting ZIP archive ...
CD "%ROOT%"
powershell -NoLogo -NoProfile -C "& { $ws = New-Object -ComObject Shell.Application; $zip = $ws.NameSpace('%ZIPFILE%\bench-master'); $trg = $ws.NameSpace('%ROOT%'); foreach($item in $zip.items()) { $trg.copyhere($item) } }"
IF %ERRORLEVEL% NEQ 0 (
  ECHO.Extracting ZIP archive failed.
  PAUSE
  EXIT /B 1
)

ECHO.Deleting ZIP archive ...
CD "%ROOT%"
DEL "%ZIPFILE%"

ECHO.Running install script ...
CD %ROOT%
.\auto\install.cmd

@ECHO OFF
ECHO.XML DOC to Markdown Converter
powershell -NoLogo -NoProfile -ExecutionPolicy Unrestricted -Command "& '%~dp0\xmldoc2md.ps1' "%*

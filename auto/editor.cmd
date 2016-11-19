@ECHO OFF
FOR %%f in ("%CD%") DO (
    SET PROJECT=%%~nf
)
runps Edit-Project.ps1 "%PROJECT%"

@ECHO OFF
FOR %%f in ("%CD%") DO (
    SET PROJECT=%%~nf
)
runps Archive-Project.ps1 "%PROJECT%"

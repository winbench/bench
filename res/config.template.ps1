##
## Customize the configuration with Set-ConfigValue and Set-AppConfigValue.
##

## Uncomment and adapt the following lines to setup a proxy:

# Set-ConfigValue UseProxy $true
# Set-ConfigValue HttpProxy "http://myproxy:3128/"
# Set-ConfigValue HttpsProxy "http://myproxy:3128/"

## Uncomment and adapt the following lines to change the location of directories:
## (paths can be relative or absolute)

# Set-ConfigValue TempDir "C:\Temp"
# Set-ConfigValue HomeDir "D:\bench_profile"
# Set-ConfigValue ProjectRootDir "D:\bench_projects"
# Set-ConfigValue ProjectArchiveDir "backups"

## Uncomment and adapt the following line to change the project archive format:
## Possible values are all archive formats supported by 7zip (e.g. zip, 7z)

# Set-ConfigValue ProjectArchiveFormat "zip"

## Uncomment and adapt the following lines to change the app selection:
## Custom apps from apps.md need to be activated here.

# Deactivate-App Gulp
# Deactivate-App Bower
# Deactivate-App MdProc
# Deactivate-App JSHint
# Deactivate-App VSCode
# Deactivate-App Pandoc
# Deactivate-App Graphviz
# Deactivate-App Inkscape
# Deactivate-App MikTeX
# Activate-App Grunt
# Activate-App cURL
# Activate-App Python2
# Activate-App Python3
# Activate-App SublimeText3
# Activate-App Iron
# Activate-App MySQL
# Activate-App MySQLWB
# Activate-App JDK7
# Activate-App JDK8
# Activate-App EclipseJava

## Deactivate forced update for NPM:

# Set-AppConfigValue Npm ForceInstall $false

## Override URL for binary download:

# Set-AppConfigValue XYZ Url "http://my-server.com/xyz/release-v2.zip"
# Set-AppConfigValue XYZ Archive "release-v*.zip"

## Override Version for NodeJS packages:

# Set-AppConfigValue XYZ Version "^3.5.0"

## Choose a Default Editor App

# Set-ConfigValue EditorApp VSCode

## Setup developer identity:

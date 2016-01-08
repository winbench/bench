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

## Uncomment the following lines to activate app groups or single apps:
## Custom apps from apps.md need to be added and activated here too.

## Application Groups

# Activate-App Markdown # MdProc, VSCode
# Activate-App WebDevPHP7 # PHP7, MySQL, MySQLWB, Apache
# Activate-App WebDevPHP5 # PHP5, MySQL, MySQLWB, Apache
# Activate-App DevJava # EclipseJava, JDK8
# Activate-App DevClojure # Leiningen, JDK8
# Activate-App DevPython2 # Python2, SublimeText3
# Activate-App DevPython3 # Python3, SublimeText3

## Single applications

# Activate-App OpenSSL
# Activate-App Sift
# Activate-App cURL
# Activate-App Pandoc
# Activate-App MikTeX
# Activate-App GraphicsMagick
# Activate-App FFmpeg
# Activate-App Graphviz
# Activate-App Inkscape
# Activate-App Node
# Activate-App Npm
# Activate-App Gulp
# Activate-App Grunt
# Activate-App Bower
# Activate-App Yeoman
# Activate-App JSHint
# Activate-App MdProc
# Activate-App Python2
# Activate-App Python3
# Activate-App Ruby
# Activate-App PHP5
# Activate-App PHP7
# Activate-App JDK7
# Activate-App JDK8
# Activate-App Leiningen
# Activate-App VSCode
# Activate-App SublimeText3
# Activate-App EclipseJava
# Activate-App Iron
# Activate-App MySQL
# Activate-App MySQLWB
# Activate-App Apache

## Activate GNU tools from Git's MinGW distribution
## http://www.mingw.org/

# Set-AppConfigValue Git Path @("mingw32\bin", "usr\bin", "cmd")

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

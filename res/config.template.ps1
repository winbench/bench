##
## Customize the configuration with Set-ConfigValue and Set-AppConfigValue.
##

## Uncomment and adapt the following lines to setup a proxy:

# Set-ConfigValue UseProxy $true
# Set-ConfigValue HttpProxy "http://myproxy:3128/"
# Set-ConfigValue HttpsProxy "http://myproxy:3128/"

## Uncomment and adapt the following lines to change the location of directories:
## (paths can be relative or absolute)

# Set-ConfigValue TempDir 'C:\Temp'
# Set-ConfigValue HomeDir '$BenchDrive$\bench_profile'
# Set-ConfigValue ProjectRootDir '$BenchDrive$\bench_projects'
# Set-ConfigValue ProjectArchiveDir 'backups'
# Set-ConfigValue LauncherDir 'D:\bench_apps'

## Uncomment and adapt the following lines to change the isolation behavior of Bench:

## Do not use the Bench temporary folder, but the default system temp directories
# Set-ConfigValue OverrideTemp $false

## Do not use the Bench home directory, but the logged in user profile instead
# Set-ConfigValue OverrideHome $false

## Do not cleanup the PATH before adding the Bench apps
# Set-ConfigValue IgnoreSystemPath $false

## Uncomment and adapt the following line to change the project archive format:
## Possible values are all archive formats supported by 7zip (e.g. zip, 7z)

# Set-ConfigValue ProjectArchiveFormat "zip"

## Uncomment the following lines to activate app groups or single apps:
## Custom apps from apps.md need to be added and activated here too.

## Application Groups

# Activate-App Markdown # MdProc, VSCode
# Activate-App WebDevPHP7 # PHP7, MySQL, MySQLWB, Apache, EclipsePHP
# Activate-App WebDevPHP5 # PHP5, MySQL, MySQLWB, Apache, EclipsePHP
# Activate-App DevJava # JDK8, EclipseJava
# Activate-App DevClojure # JDK8, Leiningen, LightTable
# Activate-App DevPython2 # Python2, SublimeText3, IPython
# Activate-App DevPython3 # Python3, SublimeText3, IPython

## Single applications

# Activate-App OpenSSL
# Activate-App Putty
# Activate-App Sift
# Activate-App cURL
# Activate-App Pandoc
# Activate-App MikTeX
# Activate-App GraphicsMagick
# Activate-App FFmpeg
# Activate-App Graphviz
# Activate-App Dia
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
# Activate-App IPython
# Activate-App Ruby
# Activate-App PHP5
# Activate-App PHP7
# Activate-App JDK7
# Activate-App JDK8
# Activate-App Leiningen
# Activate-App Go
# Activate-App VSCode
# Activate-App SublimeText3
# Activate-App LightTable
# Activate-App Emacs
# Activate-App Spacemacs
# Activate-App EclipseJava
# Activate-App EclipsePHP
# Activate-App Iron
# Activate-App MySQL
# Activate-App MySQLWB
# Activate-App PostgreSql
# Activate-App Apache

## Activate GNU tools from Git's MinGW distribution
## http://www.mingw.org/

# Set-AppConfigValue Git Path @("mingw32\bin", "usr\bin", "cmd")

## Override URL for binary download:

# Set-AppConfigValue XYZ Url "http://my-server.com/xyz/release-v2.zip"
# Set-AppConfigValue XYZ Archive "release-v*.zip"

## Override Version for NodeJS packages:

# Set-AppConfigValue XYZ Version ">=3.5.0 <4.0.0"

## Choose a Default Editor App

# Set-ConfigValue EditorApp VSCode

## Setup developer identity:

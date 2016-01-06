# Bench Apps

This document is the registry for the applications, used by _Bench_.

There are three groups of apps:

1. **Required**  
   These apps are required by _Bench_ itself.
2. **Default**  
   These apps are activated by default, but they can be deactivated,
   under consideration of thier dependencies.
3. **Optional**  
   These apps can be activated optionally.

An app is defined by a number of name-value-pairs.
The pairs are written as an unordered list, with a colon separating the name and the value.
A value can be surrounded by angle brackets `<` and `>` if it is a URL.
Any value can be surrounded by backticks.
If a value is a list, its items must be surrounded by backticks and separated by commas `, `.

You can use placeholders in variable values.
Placeholders can be application specific configuration values
like `$Git:Dir$` and `$Npm:Path$` or global configuration values
like `$BenchRoot$` and `$ProjectArchiveDir$`.

All apps are identified by an ID, which must only contain alphanumeric characters
and must not start with a numeric character.
The ID must be the first entry in a list, defining an app.

There are currently two types of apps: Windows executables and NodeJS packages.

## Windows Executables

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (optional, default is `default`)
* **Url**:
  The URL to the file, containing the app binaries
* **DownloadCookies**:
  A list of cookies, to send with the download request (optional, default is empty)
* **AppFile**:
  The name of the downloaded file (only for directly executable downloads like `*.exe` or `*.cmd`).
* **AppArchive**:
  The name of the downloaded archive with wildcards `?` and `*` (for archives which need to be extracted).
* **AppArchiveTyp**:
  The archive typ, which yields to the extractor selection (optional, default is `auto`).
  Possible values are:
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* **AppArchiveSubDir**:
  A sub folder in the archive to extract (optional, default is the archive root).
* **Dir**:
  The name of the target directory for the app (optional, default is the app ID in lowercase).
* **Path**:
  A list of relative paths inside the app directory to register in the environment `PATH`. (optional, default is `.`).
* **Register**:
  A boolean to indicate if the path(s) of the application should be added to the environment `PATH` (optional, default is `true`).
* **Exe**:
  The name of the app executable (optional, default is empty).
  The existance of an app executable is used to determine, if an app is allready installed.
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (option, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`

Some restrictions for the properties:

* The properties _AppFile_ and _AppArchive_ are mutually exclusive.
* The property _AppArchiveSubDir_ is only recognized, if _AppArchive_ is used.
* The property _Path_ is only recognized, if _Register_ is `true`.

## NodeJS Packages

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (required to be `node-package`).
* **NpmPackage**:
  The name of the NPM package to install via NPM (optional, default is the app ID in lowercase).
* **Version**:
  The package version to install (e.g. `^2.5.0`), if empty install latest (optional, default empty).
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is allready installed (optional, default is `false`).
* **Exe**:
  The name of an NPM CLI wrapper from this package (optional, default is empty).
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (option, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`

To determine, if a NodeJS package is allready installed, the existence of its package folder in `node_modules` in the NodeJS directory is checked.

## Required

### Less MSIerables

* ID: `LessMsi`
* Website: <http://lessmsi.activescott.com/>
* Version: v1.3
* Url: <https://github.com/activescott/lessmsi/releases/download/v1.3/lessmsi-v1.3.zip>
* AppArchive: `lessmsi-*.zip`
* Exe: `lessmsi.exe`
* Register: `false`

### 7zip

* ID: `SvZ`
* Website: <http://www.7-zip.de/download.html>
* Version: 15.14
* Url: <http://7-zip.org/a/7z1514.msi>
* AppArchive: `7z*.msi`
* AppArchiveSubDir: `SourceDir\Files\7-Zip`
* Dir: `7z`
* Exe: `7z.exe`

### Inno Setup Unpacker

* ID: `InnoUnp`
* Website: <http://innounp.sourceforge.net/>
* Version: 0.45
* Url: <http://sourceforge.net/projects/innounp/files/innounp/innounp 0.45/innounp045.rar>
* AppArchive: `innounp*.rar`
* Exe: `innounp.exe`
* Register: `false`

### Git

* ID: `Git`
* Website: <https://git-scm.com/download/win>
* Version: v2.6.4
* Url: <https://github.com/git-for-windows/git/releases/download/v2.6.4.windows.1/PortableGit-2.6.4-32-bit.7z.exe>
* AppArchive: `PortableGit-*-32-bit.7z.exe`
* Path: `bin`
* Exe: `git.exe`

## Default

### NodeJS

* ID: `Node`
* Website: <https://nodejs.org>
* Version: v4.2.3
* Url: <https://nodejs.org/dist/v4.2.3/win-x86/node.exe>
* AppFile: `node.exe`
* Dir: `node`
* Exe: `node.exe`

### NPM Bootstrap

Because _NodeJS_ is downloaded as bare executable, _NPM_ must be installed seperately.
But NPM, in its latest versions, is only distributed as part of the _NodeJS_ setup.
_NPM Bootstrap_ is the last version of _NPM_ which was released seperately.
Therefore, the latest version of _NPM_ is installed afterwards via _NPM Bootstrap_.

* ID: `NpmBootstrap`
* Dependencies: `Node`
* Website: <https://nodejs.org>
* Version: v1.4.12
* Url: <https://nodejs.org/dist/npm/npm-1.4.12.zip>
* AppArchive: `npm-*.zip`
* Dir: `$Node:Dir$`
* Exe: `npm.cmd`

### NPM

* ID: `Npm`
* Typ: `node-package`
* Dependencies: `NpmBootstrap`
* Website: <https://npmjs.org>
* Exe: `npm.cmd`
* ForceInstall: `true`

### Gulp

* ID: `Gulp`
* Typ: `node-package`
* Dependencies: `Npm`
* Website: <https://www.npmjs.com/package/npm>
* Version: `^3.9.0`
* Exe: `gulp.cmd`

### Bower

* ID: `Bower`
* Typ: `node-package`
* Dependencies: `Npm`
* Website: <https://www.npmjs.com/package/bower>
* Version: `^1.7.0`
* Exe: `bower.cmd`

### Yeoman

* ID: `Yeoman`
* Typ: `node-package`
* Dependencies: `Npm`
* Website: <https://www.npmjs.com/package/yeoman>
* Version: `^1.5.0`
* NpmPackage: `yo`
* Exe: `yo.cmd`

### Yeoman Generator for Markdown Projects

* ID: `MdProc`
* Typ: `node-package`
* NpmPackage: `generator-mdproc`
* Website: <https://www.npmjs.com/package/generator-mdproc>
* Dependencies: `Npm`
* Version: `^0.1.6`

### JSHint

* ID: `JSHint`
* Typ: `node-package`
* Dependencies: `Npm`
* Website: <https://www.npmjs.com/package/jshint>
* Version: `^2.8.0`
* Exe: `jshint.cmd`

### Visual Studio Code

* ID: `VSCode`
* Website: <https://code.visualstudio.com/Docs/?dv=win>
* Version: latest
* Url: <http://go.microsoft.com/fwlink/?LinkID=623231>
* AppArchive: `VSCode-win32.zip`
* Dir: `code`
* Exe: `code.exe`

### Pandoc

* ID: `Pandoc`
* Website: <https://github.com/jgm/pandoc/releases/latest>
* Version: v1.15.1.1
* Url: <https://github.com/jgm/pandoc/releases/download/1.15.1.1/pandoc-1.15.1.1-windows.msi>
* AppArchive: `pandoc-*-windows.msi`
* AppArchiveSubDir: `SourceDir\Pandoc`
* Exe: `pandoc.exe`

### Graphviz

* ID: `Graphviz`
* Website: <http://www.graphviz.org/Download_windows.php>
* Version: v2.38
* Url: <http://www.graphviz.org/pub/graphviz/stable/windows/graphviz-2.38.zip>
* AppArchive: `graphviz-*.zip`
* Path: `release\bin`
* Exe: `dot.exe`

### Inkscape

* ID: `Inkscape`
* Website: <https://inkscape.org/de/herunterladen/>
* Version: v0.91
* Url: <https://inkscape.org/en/gallery/item/3932/download/>
* AppArchive: `Inkscape-*-win32.7z`
* AppArchiveSubDir: `inkscape`
* Exe: `inkscape.exe`

### MikTeX

* ID: `MikTeX`
* Website: <http://miktex.org/portable>
* Version: v2.9.5719
* Url: <http://mirrors.ctan.org/systems/win32/miktex/setup/miktex-portable-2.9.5719.exe>
* AppArchive: `miktex-portable-2.*.exe`
* Path: `miktex\bin`
* Exe: `latex.exe`

## Optional

### Grunt

* ID: `Grunt`
* Typ: `node-package`
* Dependencies: `Npm`
* Website: <http://gruntjs.com>
* Version: `^0.4.5`
* Exe: `grunt.cmd`

### Python 2

* ID: `Python2`
* Website: <https://www.python.org/ftp/python/>
* Version: v2.7.11
* Url: <https://www.python.org/ftp/python/2.7.11/python-2.7.11.msi>
* AppArchive: `python-2.*.msi`
* AppArchiveSubDir: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Python 3

* ID: `Python3`
* Website: <https://www.python.org/ftp/python/>
* Version: v3.4.3
* Url: <https://www.python.org/ftp/python/3.4.3/python-3.4.3.msi>
* AppArchive: `python-3.*.msi`
* AppArchiveSubDir: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Sublime Text 3

* ID: `SublimeText3`
* Website: <http://www.sublimetext.com/3>
* Version: Build 3083
* Url: <http://c758482.r82.cf2.rackcdn.com/Sublime%20Text%20Build%203083.zip>
* AppArchive: `Sublime*Text*Build*.zip`
* Exe: `sublime_text.exe`

### SRWare Iron

A free portable derivative of Chromium, optimized for privacy.

* ID: `Iron`
* Website: <http://www.chromium.org/Home>
* Version: latest
* Url: <http://www.srware.net/downloads/IronPortable.zip>
* AppArchiveSubDir: `IronPortable\Iron`
* AppArchive: `IronPortable.zip`
* Exe: `chrome.exe`

### MySQL

The MySQL data is stored in `%HOMEDRIVE%%HOMEPATH%\mysql_data`.
You can start the MySQL server by running `mysql_start` in the _Bench_ shell.
You can stop the MySQL server by typing `Ctrl+C` in the console window of MySQL,
or by running `mysql_stop` in the _Bench_ shell.
The initial password for _root_ is `bench`.

* ID: `MySQL`
* Website: <http://www.mysql.com/>
* Version: 5.7.10
* Url: <http://dev.mysql.com/get/Downloads/MySQL-5.7/mysql-5.7.10-win32.zip>
* AppArchive: `mysql-5.7.10-win32.zip`
* AppArchiveSubDir: `mysql-5.7.10-win32`
* Path: `bin`
* Exe: `mysqld.exe`

### MySQL Workbench

* ID: `MySQLWB`
* Version: 6.3.6
* Website: <http://dev.mysql.com/downloads/workbench/>
* Url: <http://dev.mysql.com/get/Downloads/MySQLGUITools/mysql-workbench-community-6.3.6-win32-noinstall.zip>
* AppArchive: `mysql-workbench-community-*-win32-noinstall.zip`
* AppArchiveSubDir: `MySQL Workbench 6.3.6 CE (win32)`
* Exe: `MySQLWorkbench.exe`

### Java Development Kit 7

* ID: `JDK7`
* Version: 7u80
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/jdk7-downloads-1880260.html>
* Url: <http://download.oracle.com/otn-pub/java/jdk/7u79-b15/jdk-7u79-windows-i586.exe>
* DownloadCookies: `oracle.com: oraclelicense=accept-securebackup-cookie`
* AppArchive: `jdk-7u*-windows-i586.exe`
* Path: `bin`
* Exe: `javac.exe`
* Environment: `JAVA_HOME=$JDK7:Dir$`

### Java Development Kit 8

* ID: `JDK8`
* Version: 8u66
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/index.html>
* Url: <http://download.oracle.com/otn-pub/java/jdk/8u66-b18/jdk-8u66-windows-i586.exe>
* DownloadCookies: `oracle.com: oraclelicense=accept-securebackup-cookie`
* AppArchive: `jdk-8u*-windows-i586.exe`
* Path: `bin`
* Exe: `javac.exe`
* Environment: `JAVA_HOME=$JDK8:Dir$`

### Eclipse

* ID: `EclipseJava`
* Version: Luna SR2 4.4.2
* Website: <http://www.eclipse.org/>
* Url: <http://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/luna/SR2/eclipse-java-luna-SR2-win32.zip>
* AppArchive: `eclipse-java-*-win32.zip`
* AppArchiveSubDir: `eclipse`
* Dir: `eclipse_java`
* Exe: `eclipse.exe`

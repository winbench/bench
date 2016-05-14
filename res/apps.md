# Bench Apps

This document is the registry for the applications, used by _Bench_.

There are three groups of apps:

1. **Required**  
   These apps are required by _Bench_ itself.
2. **Groups**  
   These apps are defined only by dependencies to optional apps.
3. **Optional**  
   These apps can be activated optionally.

An app is defined by a number of properties (name-value-pairs).
The pairs are written as an unordered list, with a colon separating the name and the value.

An app is identified by an ID, which must only contain alphanumeric characters
and must not start with a numeric character.
The ID must be the first entry in the list, defining an app.

```Markdown
* ID: `AppId`
* Property1: Value 1
```

Property names must consist of alpha-numerical characters and must *not* contain whitespaces.
A value can be surrounded by angle brackets `<` and `>` if it is an URL.
A value can be surrounded by backticks.

```Markdown
* Property: Value 1
* UrlProperty: <http://github.com>
* QuotedProperty: `some value`
```

If a value is a list, its items must be surrounded by backticks and separated by commas &ndash;
or formatted as nested list.

```Markdown
* ListProperty1: `Value 1`, `http://value2-com`, `Value 3`
* ListProperty2:
    + Value 1
    + <http://value-2.com>
    + `Value 3`
```

If a value is a dictionary, its items follow the rules of a list value,
but each item is a key-value-pair separated by a colon.

```Markdown
* DictionaryProperty1: `Key 1: Value`, `KEY_2: Value 2`
* DictionaryProperty2:
    + Key 1: Value
    + `KEY_2`: `Value 2`
```

You can use placeholders in variable values.
Placeholders can be application specific configuration values
like `$Git:Dir$` and `$Npm:Path$` or global configuration values
like `$BenchRoot$` and `$ProjectArchiveDir$`.
Application specific placeholders can skip the app ID
to refer to the current app like `$:Dir`.

## Common Properties

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ
  (optional, default is `default`).
* **Dependencies**:
  A list with the IDs of all apps in this app group.
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is already installed
  (optional, default is `false`).
* **Dir**:
  The name of the target directory for the app
  (optional, default is the app ID in lowercase).
* **Path**:
  A list of relative paths inside the app directory to register in the environment `PATH`
  (optional, default is `.`).
  This property is only recognized, if `Register` is `true`.
* **Register**:
  A boolean to indicate if the path(s) of the application should be added to the environment `PATH`
  (optional, default is `true`).
* **Environment**:
  A list of key-value-pairs, describing additional environment variables
  (optional, default is empty).
  E.g. `MY_APP_HOME: $MyApp:Dir$`, `MY_APP_LOG: D:\logs\myapp.log`
* **Exe**:
  The path of the main executable for this app, relative to the target directory of the app
  (optional, default is `<app ID>.exe`).
* **AdornedExecutables**:
  A list of executable paths, relative to the target directory of the app.
  Every listed executable will be adorned with pre- and post-execution scripts.
  (optional, default is empty)
* **RegistryKeys**:
  A list of relative key paths in the Windows registry hive `HKEY_CURRENT_USER`,
  which are used by this app and must be backed up and restored,
  during execution of this app.
  (optional, default is empty)
* **Launcher**:
  A label for the app launcher
  (optional, default is empty).
  A launcher for the app is created only if this property is set to a non empty string.
* **LauncherExecutable**:
  The path to the executable targeted by the app launcher,
  absolute or relative to the target directory of the app
  (optional, default is the `Exe` property).
* **LauncherArguments**:
  A list with arguments to the app executable
  (optional, default is `%*`).
* **LauncherIcon**:
  The path to the icon of the launcher,
  absolute or relative to the target directory of the app
  (optional, default is the executable).

## App Types

There are currently the following types of apps:

* Typ `meta`: app groups or apps with a fully customized setup process
* Typ `default`: Windows executables from a downloades file, archive, or setup
* Typ `node-package`: NodeJS packages, installable with NPM
* Typ `ruby-package`: Ruby packages, installable with Gem
* Typ `python2-package`: Python packages for Python 2 from PyPI, installable with PIP
* Typ `python3-package`: Python packages for Python 3 from PyPI, installable with PIP

### App Group and Custom Setup

* **Typ**:
  The application typ
  (required to be `meta`)

### Windows Apps

A Windows app is some kind of executable for the Windows OS.

* **Url**:
  The URL to the file, containing the app binaries
* **DownloadCookies**:
  A list of cookies, to send with the download request
  (optional, default is empty)
* **ResourceName**:
  The name of the downloaded file
  (only for directly executable downloads like `*.exe` or `*.cmd`).
* **ArchiveName**:
  The name of the downloaded archive
  (for archives which need to be extracted like `*.zip`, `*.msi`, or setup programs).
* **ArchiveTyp**:
  The archive typ, which yields to the extractor selection
  (optional, default is `auto`).
  Possible values are:
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `inno` Use Inno Setup Unpacker to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* **ArchivePath**:
  A sub folder in the archive to extract
  (optional, default is the archive root).
* **SetupTestFile**:
  The existence of this file is used, to determine if the app is already installed
  (optional, default is the value of the property `App-Exe`).

Some restrictions for the properties:

* The properties _ResourceName_ and _ArchiveName_ are mutually exclusive.
* The property _ArchivePath_ is only recognized, if _ArchiveName_ is used.

### NodeJS Packages

* **Typ**:
  The application typ
  (required to be `node-package`).
* **PackageName**:
  The name of the NPM package to install via NPM
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0 <3.0.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an NPM CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for NodeJS packages.

To determine, if a NodeJS package is already installed, the existence of its package folder in
`node_modules` in the NodeJS directory is checked.

### Ruby Packages

* **Typ**:
  The application typ
  (required to be `ruby-package`).
* **PackageName**:
  The name of the Ruby package to install via Gem
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an Gem CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for Ruby packages.

To determine, if a Ruby gem is already installed, the existence of its package folder in
`lib\ruby\gems\<ruby-version>\gems` in the Ruby directory is checked.

### Python Package

* **Typ**:
  The application typ
  (required to be `python2-package` or `python3-package`)
* **PackageName**:
  The name of the PyPI package to install via PIP
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0,<3.0.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an PIP CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for Python packages.

To determine, if a Python package is already installed, the existence of its package folder in
`lib\site-packages` in the Python directory is checked.

## Required

### Less MSIerables

* ID: `LessMsi`
* Version: 1.3
* Website: <http://lessmsi.activescott.com/>
* Url: `https://github.com/activescott/lessmsi/releases/download/v$:Version$/$:ArchiveName$`
* ArchiveName: `lessmsi-v$:Version$.zip`
* Exe: `lessmsi.exe`
* Register: `false`

### 7zip

* ID: `SvZ`
* Website: <http://www.7-zip.de/download.html>
* Version: 15.14
* Release: 1514
* Url: `http://7-zip.org/a/$:ArchiveName$`
* ArchiveName: `7z$:Release$.msi`
* ArchivePath: `SourceDir\Files\7-Zip`
* Dir: `7z`
* Exe: `7z.exe`

### Inno Setup Unpacker

* ID: `InnoUnp`
* Website: <http://innounp.sourceforge.net/>
* Version: 0.45
* Release: 045
* Url: `http://sourceforge.net/projects/innounp/files/innounp/innounp%20$:Version$/$:ArchiveName$`
* ArchiveName: `innounp$:Release$.rar`
* Exe: `innounp.exe`
* Register: `false`

### ConEmu

* ID: `ConEmu`
* Website: <https://conemu.github.io/>
* Version: 16.03.13
* Release: 160313
* Url: `https://github.com/Maximus5/ConEmu/releases/download/v$:Version$/$:ArchiveName$`
* ArchiveName: `ConEmuPack.$:Release$.7z`
* Launcher: `ConEmu`
* LauncherArguments: `-LoadCfgFile`, `$CustomConfigDir$\ConEmu.xml`,  `%*`

### Git

* ID: `Git`
* Website: <https://git-scm.com/download/win>
* Version: 2.8.2
* Release: 2.8.2.windows.1
* Url: `https://github.com/git-for-windows/git/releases/download/v$:Release$/$:ArchiveName$`
* ArchiveName: `PortableGit-$:Version$-32-bit.7z.exe`
* Path: `cmd`
* Exe: `cmd\git.exe`
* Environment:
    + `GIT_SSH`: `$:Dir$\usr\bin\ssh.exe`

## Groups

### Group: Markdown

* ID: `Markdown`
* Typ: `meta`
* Dependencies: `MdProc`, `VSCode`

### Group: Multimedia

* ID: `Multimedia`
* Typ: `meta`
* Dependencies: `Inkscape`, `Dia`, `Gimp`, `Pandoc`, `MikTeX`, `GraphicsMagick`, `Graphviz`, `FFmpeg`

### Group: Web Development with PHP7 and MySQL

* ID: `WebDevPHP7`
* Typ: `meta`
* Dependencies: `PHP7`, `MySQL`, `MySQLWB`, `Apache`, `EclipsePHP`

### Group: Web Development with PHP5 and MySQL

* ID: `WebDevPHP5`
* Typ: `meta`
* Dependencies: `PHP5`, `MySQL`, `MySQLWB`, `Apache`, `EclipsePHP`

### Group: Java Development

* ID: `DevJava`
* Typ: `meta`
* Dependencies: `JDK8`, `Maven`, `EclipseJava`

### Group: Clojure Development

* ID: `DevClojure`
* Typ: `meta`
* Dependencies: `Maven`, `Leiningen`, `LightTable`

### Group: Python 2

* ID: `DevPython2`
* Typ: `meta`
* Dependencies: `Python2`, `SublimeText3`, `IPython2`

### Group: Python 3

* ID: `DevPython3`
* Typ: `meta`
* Dependencies: `Python3`, `SublimeText3`, `IPython3`

### Group: C++

* ID: `DevCpp`
* Typ: `meta`
* Dependencies: `MinGW`, `EclipseCpp`

## Optional

### GitKraken

* ID: `GitKraken`
* Version: latest
* Website: <https://www.gitkraken.com/>
* Url: <https://release.gitkraken.com/win32/GitKrakenSetup.exe>
* ArchiveName: `GitKrakenSetup.exe`
* ArchiveTyp: `custom`
* ArchivePath: `lib\net45`
* Launcher: GitKraken

### OpenSSL

* ID: `OpenSSL`
* Website: <https://www.openssl.org/>
* Version: 1.0.2d-fips-2.0.10
* Url: `http://sourceforge.net/projects/openssl/files/openssl-$:Version$/$:ArchiveName$`
* ArchiveName: `openssl-$:Version$.zip`
* ArchivePath: `openssl-$:Version$`
* Path: `bin`
* Exe: `bin\openssl.exe`

### Putty

* ID: `Putty`
* Website: <http://www.putty.org>
* Version: latest
* Url: <http://the.earth.li/~sgtatham/putty/latest/x86/putty.zip>
* ArchiveName: `putty.zip`
* RegistryKeys: `Software\SimonTatham`
* Launcher: `Putty`

### GNU TLS

* ID: `GnuTLS`
* Version: 3.3.11
* Url: `http://sourceforge.net/projects/ezwinports/files/$:ArchiveName$`
* ArchiveName: `gnutls-$:Version$-w32-bin.zip`
* Dir: `gnu`
* Path: `bin`
* Exe: `bin\gnutls-cli.exe`

### GnuPG

* ID: `GnuPG`
* Version: 2.0.30
* Url: `https://sourceforge.net/projects/portableapps/files/GPG Plugin Portable/$:ArchiveName$`
* ArchiveName: `GPG_Plugin_Portable_$:Version$.paf.exe`
* Dir: `gpg`
* Path: `pub`
* Exe: `pub\gpg.exe`

### Wget

* ID: `Wget`
* Version: 1.11.4-1
* Dependencies: `WgetDeps`
* Url: `https://sourceforge.net/projects/gnuwin32/files/wget/$:Version$/$:ArchiveName$`
* ArchiveName: `wget-$:Version$-bin.zip`
* Dir: `gnu`
* Path: `bin`
* Exe: `bin\wget.exe`
* Environment:
    + `HTTP_CLIENT`: `wget --no-check-certificate -O`

* ID: `WgetDeps`
* Version: `$Wget:Version$`
* Url: `https://sourceforge.net/projects/gnuwin32/files/wget/$:Version$/$:ArchiveName$`
* ArchiveName: `wget-$:Version$-dep.zip`
* Dir: `gnu`
* SetupTestFile: `bin\libssl32.dll`

### cURL

* ID: `cURL`
* Website: <http://curl.haxx.se/>
* Version: 7.48.0
* Url: `https://bintray.com/artifact/download/vszakats/generic/$:ArchiveName$`
* ArchiveName: `curl-$:Version$-win32-mingw.7z`
* ArchivePath: `curl-$:Version$-win32-mingw`
* Path: `bin`
* Exe: `bin\curl.exe`

### FileZilla

* ID: `FileZilla`
* Version: 3.17.0.1
* Website: <https://filezilla-project.org/>
* Url: `https://sourceforge.net/projects/portableapps/files/FileZilla%20Portable/$:ArchiveName$`
* ArchiveName: `FileZillaPortable_$:Version$.paf.exe`
* ArchivePath: `App/filezilla`
* Exe: `filezilla.exe`
* Register: `false`
* Launcher: `FileZilla`

### Sift

* ID: `Sift`
* Website: <https://sift-tool.org/>
* Version: 0.8.0
* Url: `https://sift-tool.org/downloads/sift/$:ArchiveName$`
* ArchiveName: `sift_$:Version$_windows_386.zip`

### WinMerge

* ID: `WinMerge`
* Version: 2.14.0
* Website: <http://winmerge.org/>
* Url: `https://sourceforge.net/projects/portableapps/files/WinMerge%20Portable/$:ArchiveName$`
* ArchiveName: `WinMergePortable_$:Version$.paf.exe`
* ArchivePath: `App/winmerge`
* Exe: `WinMergeU.exe`
* RegistryKeys: `Software\Thingamahoochie`
* Register: `false`
* Launcher: `WinMerge`

### Pandoc

* ID: `Pandoc`
* Website: <https://github.com/jgm/pandoc/releases/latest>
* Version: 1.17.0.2
* Url: `https://github.com/jgm/pandoc/releases/download/$:Version$/$:ArchiveName$`
* ArchiveName: `pandoc-$:Version$-windows.msi`
* ArchivePath: `SourceDir\Pandoc`
* Exe: `pandoc.exe`

### MikTeX

* ID: `MikTeX`
* Website: <http://miktex.org/portable>
* Version: 2.9.5857
* Url: `http://mirrors.ctan.org/systems/win32/miktex/setup/$:ArchiveName$`
* ArchiveName: `miktex-portable-$:Version$.exe`
* Path: `miktex\bin`
* Exe: `miktex\bin\latex.exe`
* Launcher: `TeXworks`
* LauncherExecutable: `miktex\bin\texworks.exe`

### Graphics Magick

* ID: `GraphicsMagick`
* Website: <http://www.graphicsmagick.org/>
* Version: 1.3.23
* Url: `http://sourceforge.net/projects/graphicsmagick/files/graphicsmagick-binaries/$:Version$/$:ArchiveName$`
* ArchiveName: `GraphicsMagick-$:Version$-Q16-win32-dll.exe`
* ArchiveTyp: `inno`
* ArchivePath: `{app}`
* Dir: `gm`
* Exe: `gm.exe`

### FFmpeg

* ID: `FFmpeg`
* Website: <https://www.ffmpeg.org/>
* Version: 20160512-git-cd244fa
* Url: `http://ffmpeg.zeranoe.com/builds/win32/shared/$:ArchiveName$`
* ArchiveName: `ffmpeg-$:Version$-win32-shared.7z`
* ArchivePath: `ffmpeg-$:Version$-win32-shared`
* Path: `bin`
* Exe: `bin\ffmpeg.exe`

### Graphviz

* ID: `Graphviz`
* Website: <http://www.graphviz.org/Download_windows.php>
* Version: 2.38
* Url: `http://www.graphviz.org/pub/graphviz/stable/windows/$:ArchiveName$`
* ArchiveName: `graphviz-$:Version$.zip`
* Path: `release\bin`
* Exe: `release\bin\dot.exe`

### Dia

* ID: `Dia`
* Version: 0.97.2
* Release: 0.97.2-2
* Website: <https://wiki.gnome.org/action/show/Apps/Dia>
* Url: `http://sourceforge.net/projects/dia-installer/files/dia-win32-installer/$:Version$/$:ArchiveName$`
* ArchiveName: `dia-setup-$:Release$-unsigned.exe`
* Path: `bin`
* Exe: `bin\dia.exe`
* Launcher: `Dia`
* LauncherExecutable: `bin\diaw.exe`
* LauncherArguments: `--integrated`, `%*`

### Inkscape

* ID: `Inkscape`
* Website: <https://inkscape.org/en/download/>
* Version: 0.91-1
* Url: <https://inkscape.org/en/gallery/item/3932/download/>
* ArchiveName: `Inkscape-$:Version$-win32.7z`
* ArchivePath: `inkscape`
* Exe: `inkscape.exe`
* Launcher: `Inkscape`

### GIMP

* ID: `Gimp`
* Version: 2.8.16
* Website: <http://www.gimp.org/>
* Url: `https://sourceforge.net/projects/portableapps/files/GIMP Portable/$:ArchiveName$`
* ArchiveName: `GIMPPortable_$:Version$.paf.exe`
* ArchivePath: `App/gimp`
* Exe: `bin\gimp-2.8.exe`
* Register: `false`
* Launcher: `GIMP`

### NodeJS

* ID: `Node`
* Website: <https://nodejs.org>
* Version: 4.4.4
* Url: `https://nodejs.org/dist/v$:Version$/win-x86/node.exe`
* ResourceName: `node.exe`
* Dir: `node`
* Exe: `node.exe`
* Launcher: NodeJS

### NPM

Because _NodeJS_ is downloaded as bare executable, _NPM_ must be installed seperately.
But NPM, in its latest versions, is only distributed as part of the _NodeJS_ setup.
_NPM_ 1.4.12 is the last version of _NPM_ which was released seperately.
Therefore, the latest version of _NPM_ is installed afterwards via the setup script `auto\apps\npm.setup.ps1`.

* ID: `Npm`
* Dependencies: `Node`
* Website: <https://www.npmjs.com/package/npm>
* Version: `>=3.7.0 <4.0.0`
* Url: <https://nodejs.org/dist/npm/npm-1.4.12.zip>
* ArchiveName: `npm-1.4.12.zip`
* Dir: `$Node:Dir$`
* Exe: `npm.cmd`

### Gulp

* ID: `Gulp`
* Typ: `node-package`
* Version: `>=3.9.0 <4.0.0`
* Website: <https://www.npmjs.com/package/gulp>
* Exe: `gulp.cmd`

### Grunt

* ID: `Grunt`
* Typ: `node-package`
* Version: `>=0.4.5 <0.5.0`
* Website: <http://gruntjs.com>
* Exe: `grunt.cmd`

### Bower

* ID: `Bower`
* Typ: `node-package`
* Version: `>=1.7.0 <2.0.0`
* Website: <https://www.npmjs.com/package/bower>
* Exe: `bower.cmd`

### Yeoman

* ID: `Yeoman`
* Typ: `node-package`
* PackageName: `yo`
* Version: `>=1.5.0 <2.0.0`
* Website: <https://www.npmjs.com/package/yo>
* Exe: `yo.cmd`

### Yeoman Generator for Markdown Projects

* ID: `MdProc`
* Typ: `node-package`
* PackageName: `generator-mdproc`
* Version: `>=0.1.6 <0.2.0`
* Website: <https://www.npmjs.com/package/generator-mdproc>
* Dependencies: `Yeoman`, `Gulp`, `Pandoc`, `Graphviz`, `Inkscape`, `MikTeX`

### JSHint

* ID: `JSHint`
* Typ: `node-package`
* Version: `>=2.8.0 <3.0.0`
* Website: <https://www.npmjs.com/package/jshint>
* Exe: `jshint.cmd`

### Python 2

* ID: `Python2`
* Website: <https://www.python.org/ftp/python/>
* Version: 2.7.11
* Url: `https://www.python.org/ftp/python/$:Version$/$:ArchiveName$`
* ArchiveName: `python-$:Version$.msi`
* ArchivePath: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Python 3

* ID: `Python3`
* Website: <https://www.python.org/ftp/python/>
* Version: 3.4.4
* Url: `https://www.python.org/ftp/python/$:Version$/$:ArchiveName$`
* ArchiveName: `python-$:Version$.msi`
* ArchivePath: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### PyReadline

Required for colors in IPython.

for Python 2:

* ID: `PyReadline2`
* PackageName: `pyreadline`
* Typ: `python2-package`
* Website: <https://pypi.python.org/pypi/pyreadline>

for Python 3:

* ID: `PyReadline3`
* PackageName: `pyreadline`
* Typ: `python3-package`
* Website: <https://pypi.python.org/pypi/pyreadline>

### IPython

for Python 2:

* ID: `IPython2`
* Typ: `python2-package`
* PackageName: `ipython`
* Dependencies: `PyReadline2`
* Website: <https://pypi.python.org/pypi/ipython>
* Exe: `Scripts\ipython2.exe`
* Launcher: `IPython 2`

for Python 3:

* ID: `IPython3`
* Typ: `python3-package`
* PackageName: `ipython`
* Dependencies: `PyReadline3`
* Website: <http://pypi.python.org/pypi/ipython>
* Exe: `Scripts\ipython3.exe`
* Launcher: `IPython 3`

### Ruby

* ID: `Ruby`
* Website: <https://www.ruby-lang.org/>
* Version: 2.2.4
* Url: `http://dl.bintray.com/oneclick/rubyinstaller/$:ArchiveName$`
* ArchiveName: `rubyinstaller-$:Version$.exe`
* ArchiveTyp: `inno`
* ArchivePath: `{app}`
* Path: `bin`
* Exe: `bin\ruby.exe`
* Launcher: `Ruby`
* LauncherArguments: `$:Dir$\bin\irb`

### SASS

* ID: `Sass`
* Typ: `ruby-package`
* Website: <http://sass-lang.com/>

### PHP 5

This application needs the x86 version of the [Visual C++ 11 Redistributable][MS VC11] installed.

* ID: `PHP5`
* Website: <http://www.php.net>
* Version: 5.6.20
* Url: `http://windows.php.net/downloads/releases/archives/$:ArchiveName$`
* ArchiveName: `php-$:Version$-Win32-VC11-x86.zip`
* Exe: `php.exe`

### PHP 7

This application needs the x86 version of the [Visual C++ 14 Redistributable][MS VC14] installed.

* ID: `PHP7`
* Website: <http://www.php.net>
* Version: 7.0.5
* Url: `http://windows.php.net/downloads/releases/archives/$:ArchiveName$`
* ArchiveName: `php-$:Version$-Win32-VC14-x86.zip`
* Exe: `php.exe`

### Java Runtime Environment 7

* ID: `JRE7`
* Version: 7u80
* Release: b15
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/java-archive-downloads-javase7-521261.html>
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jre-$:Version$-windows-i586.tar.gz`
* ArchivePath: `jre1.7.0_80`
* Path: `bin`
* Exe: `bin\java.exe`
* Environment:
    + `JAVA_CMD`: `$:Exe$`

### Java Runtime Environment 8

* ID: `JRE8`
* Version: 8u92
* Release: b14
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html>
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jre-$:Version$-windows-i586.tar.gz`
* ArchivePath: `jre1.8.0_92`
* Path: `bin`
* Exe: `bin\java.exe`
* Environment:
    + `JAVA_CMD`: `$:Exe$`

### Java Development Kit 7

* ID: `JDK7`
* Version: $JRE7:Version$
* Release: $JRE7:Release$
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/java-archive-downloads-javase7-521261.html>
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jdk-$:Version$-windows-i586.exe`
* Path: `bin`
* Exe: `bin\javac.exe`
* Environment:
    + `JAVA_HOME`: `$:Dir$`
    + `JAVA_CMD`: `$:Dir$\jre\bin\java.exe`

### Java Development Kit 8

* ID: `JDK8`
* Version: $JRE8:Version$
* Release: $JRE8:Release$
* Website: <http://www.oracle.com/technetwork/java/javase/downloads/index.html>
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jdk-$:Version$-windows-i586.exe`
* Path: `bin`
* Exe: `bin\javac.exe`
* Environment:
    + `JAVA_HOME`: `$:Dir$`
    + `JAVA_CMD`: `$:Dir$\jre\bin\java.exe`

### Maven

* ID: `Maven`
* Dependencies: `JRE8`, `GnuPG`
* Version: `3.3.9`
* Website: <https://maven.apache.org>
* Url: `http://www-eu.apache.org/dist/maven/maven-3/$:Version$/binaries/$:ArchiveName$`
* ArchiveName: `apache-maven-$:Version$-bin.zip`
* ArchivePath: `apache-maven-$:Version$`
* Dir: `mvn`
* Path: `bin`
* Exe: `bin\mvn.cmd`

### Leiningen

* ID: `Leiningen`
* Dependencies: `JDK8`, `GnuPG`, `Wget`
* Version: latest
* Website: <http://leiningen.org>
* Url: <https://raw.githubusercontent.com/technomancy/leiningen/stable/bin/lein.bat>
* ResourceName: `lein.bat`
* Dir: `lein`
* Exe: `lein.bat`
* Environment:
    + `LEIN_JAR`: `$:Dir$\leiningen.jar`

### MinGW

[MinGW] provides a GNU development environment for Windows, including compilers for C/C++, Objective-C, Fortran, Ada, ...

The MinGW package manager MinGW Get:

* ID: `MinGwGet`
* Version: 0.6.2
* Release: beta-20131004-1
* Dependencies: `Wget`
* Url: `https://sourceforge.net/projects/mingw/files/Installer/mingw-get/mingw-get-$:Version$-$:Release$/$:ArchiveName$`
* ArchiveName: `mingw-get-$:Version$-mingw32-$:Release$-bin.tar.xz`
* Dir: `mingw`
* Path: `bin`
* Exe: `bin\mingw-get.exe`

Graphical user interface for MinGW Get:

* ID: `MinGwGetGui`
* Dependencies: `MinGwGet`
* Url: `https://sourceforge.net/projects/mingw/files/Installer/mingw-get/mingw-get-$MinGwGet:Version$-$MinGwGet:Release$/$:ArchiveName$`
* ArchiveName: `mingw-get-$MinGwGet:Version$-mingw32-$MinGwGet:Release$-gui.tar.xz`
* Dir: `mingw`
* Exe: `libexec\mingw-get\guimain.exe`
* Register: `false`
* Launcher: `MinGW Package Manager`

Meta app MinGW with package manager and graphical user interface:

* ID: `MinGW`
* Typ: `meta`
* Dependencies: `MinGwGet`, `MinGwGetGui`
* Website: <http://www.mingw.org/>
* Dir: `mingw`
* Path: `bin`, `msys\1.0\bin`
* Packages:
    + `mingw32-base`
    + `mingw32-gcc-g++`

You can adapt the preselected MinGW packages by putting something like this in your `config\config.md`:

```Markdown
### My MinGW Packages

* ID: `MinGW`
* Packages:
    + `mingw32-base`
    + `mingw32-gcc-g++`
    + `mingw32-autotools`
    + `msys-bash`
    + `msys-grep`
```

After the automatic setup by _Bench_, you can use the launcher shortcut `MinGW Package Manager`
to start the GUI for _MinGW Get_ and install more MinGW packages.

### CMake

Usually you want to use this app with _MinGW_.

To setup a C/C++ project with CMake and MinGW (`mingw32-make`), you have to activate the _MinGW_ app with the `mingw32-make` package.
Setup your project with a `CMakeLists.txt` file and run `cmake -G "MinGW Makefiles" <project folder>` to generate the `Makefile`. Run `cmake --build <project folder>` to compile the project.

* ID: `CMake`
* MajorVersion: 3.5
* Version: $:MajorVersion$.2
* Website: <https://cmake.org/>
* Url: `https://cmake.org/files/v$:MajorVersion$/$:ArchiveName$`
* ArchiveName: `cmake-$:Version$-win32-x86.zip`
* ArchivePath: `cmake-$:Version$-win32-x86`
* Path: `bin`
* Exe: `bin\cmake.exe`

### LLVM Clang

The Clang compiler can act as drop-in replacement for the GCC compilers.

This app sets the environment variables `CC` and `CXX` to inform _CMake_
about the C/C++ compiler path. Therefore, if you build your C/C++ projects
with _CMake_, it is sufficient to just activate the _Clang_ app and _CMake_
will use _Clang_ instead of the GCC compiler from _MinGW_.

If you want to use the Clang compiler with Eclipse, you must manually
install the LLVM-Plugin for Eclipse CDT.

* ID: `Clang`
* Version: 3.7.1
* Website: <http://clang.llvm.org/>
* Url: `http://llvm.org/releases/$:Version$/$:ArchiveName$`
* ArchiveName: `LLVM-$:Version$-win32.exe`
* Dir: `llvm`
* Path: `bin`
* Exe: `bin\clang.exe`
* Environment:
    + `CC`: `$:Dir$\bin\clang.exe`
    + `CXX`: `$:Dir$\bin\clang++.exe`

### Go

* ID: `Go`
* Version: 1.6
* Website: <https://golang.org>
* Url: `https://storage.googleapis.com/golang/$:ArchiveName$`
* ArchiveName: `go$:Version$.windows-386.zip`
* ArchivePath: `go`
* Path: `bin`
* Exe: `bin\go.exe`
* Environment:
    + `GOROOT`: `$:Dir$`

### Visual Studio Code

* ID: `VSCode`
* Website: <https://code.visualstudio.com/Docs/?dv=win>
* Version: latest
* Url: <http://go.microsoft.com/fwlink/?LinkID=623231>
* ArchiveName: `VSCode-win32.zip`
* Dir: `code`
* Exe: `code.exe`
* Launcher: `Visual Studio Code`

### LightTable

* ID: `LightTable`
* Website: <http://lighttable.com>
* Version: 0.8.1
* Url: `https://github.com/LightTable/LightTable/releases/download/$:Version$/$:ArchiveName$`
* ArchiveName: `lighttable-$:Version$-windows.zip`
* ArchivePath: `lighttable-$:Version$-windows`
* Dir: `lt`
* Exe: `LightTable.exe`
* Launcher: `LightTable`

### Sublime Text 3

* ID: `SublimeText3`
* Website: <http://www.sublimetext.com/3>
* Version: Build 3114
* Url: `https://download.sublimetext.com/$:ArchiveName$`
* ArchiveName: `Sublime Text $:Version$.zip`
* Exe: `sublime_text.exe`
* Launcher: `Sublime Text 3`

### Emacs

* ID: `Emacs`
* Dependencies: `GnuTLS`
* Website: <https://www.gnu.org/software/emacs/>
* Version: 24.5
* Url: `http://ftp.gnu.org/gnu/emacs/windows/$:ArchiveName$`
* ArchiveName: `emacs-$:Version$-bin-i686-mingw32.zip`
* Dir: `gnu`
* Path: `bin`
* Exe: `bin\emacs.exe`
* Launcher: `Emacs`
* LauncherExecutable: `$:Dir$\bin\runemacs.exe`

### Spacemacs

* ID: `Spacemacs`
* Typ: `meta`
* Dependencies: `Git`, `Emacs`
* Website: <http://spacemacs.org/>
* SetupTestFile: `$HomeDir$\.emacs.d\spacemacs.mk`

### Eclipse

Eclipse for Java development:

* ID: `EclipseJava`
* Version: 4.5
* CodeName: mars
* Release: 1
* Dependencies: `JRE8`
* Website: <http://www.eclipse.org/>
* Url: `http://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/$:CodeName$/$:Release$/$:ArchiveName$`
* ArchiveName: `eclipse-java-$:CodeName$-$:Release$-win32.zip`
* ArchivePath: `eclipse`
* Dir: `eclipse_java`
* Exe: `eclipse.exe`
* Register: `false`
* Launcher: `Eclipse Java`

Eclipse for PHP development:

* ID: `EclipsePHP`
* Version: 4.5
* CodeName: mars
* Release: 1
* Dependencies: `JRE8`
* Website: <http://www.eclipse.org/>
* Url: `http://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/$:CodeName$/$:Release$/$:ArchiveName$`
* ArchiveName: `eclipse-php-$:CodeName$-$:Release$-win32.zip`
* ArchivePath: `eclipse`
* Dir: `eclipse_php`
* Exe: `eclipse.exe`
* Register: `false`
* Launcher: `Eclipse PHP`

Eclipse for C/C++ development:

* ID: `EclipseCpp`
* Version: 4.5
* CodeName: mars
* Release: 1
* Dependencies: `JRE8`
* Website: <http://www.eclipse.org/>
* Url: `http://www.eclipse.org/downloads/download.php?file=/technology/epp/downloads/release/$:CodeName$/$:Release$/$:ArchiveName$`
* ArchiveName: `eclipse-cpp-$:CodeName$-$:Release$-win32.zip`
* ArchivePath: `eclipse`
* Dir: `eclipse_cpp`
* Exe: `eclipse.exe`
* Register: `false`
* Launcher: `Eclipse C++`

### SRWare Iron

A free portable derivative of Chromium, optimized for privacy.

* ID: `Iron`
* Website: <http://www.chromium.org/Home>
* Version: latest
* Url: <http://www.srware.net/downloads/IronPortable.zip>
* ArchivePath: `IronPortable\Iron`
* ArchiveName: `IronPortable.zip`
* Exe: `chrome.exe`
* Launcher: `SRWare Iron`

### MySQL

The MySQL data is stored in `%USERPROFILE%\mysql_data`.
You can start the MySQL server by running `mysql_start` in the _Bench_ shell.
You can stop the MySQL server by running `mysql_stop` in the _Bench_ shell.
The initial password for _root_ is `bench`.

* ID: `MySQL`
* Website: <http://www.mysql.com/>
* Version: 5.7.11
* Url: `http://dev.mysql.com/get/Downloads/MySQL-5.7/$:ArchiveName$`
* ArchiveName: `mysql-$:Version$-win32.zip`
* ArchivePath: `mysql-$:Version$-win32`
* Path: `bin`
* Exe: `bin\mysqld.exe`
* MySqlDataDir: `$HomeDir$\mysql_data`
* Environment:
    + `MYSQL_DATA`: `$:MySqlDataDir$`

### MySQL Workbench

* ID: `MySQLWB`
* Version: 6.3.6
* Website: <http://dev.mysql.com/downloads/workbench/>
* Url: `http://dev.mysql.com/get/Downloads/MySQLGUITools/$:ArchiveName$`
* ArchiveName: `mysql-workbench-community-$:Version$-win32-noinstall.zip`
* ArchivePath: `MySQL Workbench 6.3.6 CE (win32)`
* Exe: `MySQLWorkbench.exe`
* Register: `false`
* Launcher: `MySQL Workbench`

### PostgreSQL

Contains the _PostgreSQL Server_ and the management tool _pgAdminIII_.
The initial password for _postgres_ is `bench`.

* ID: `PostgreSQL`
* Website: <http://www.postgresql.org>
* Version: 9.5.0-1
* Url: `http://get.enterprisedb.com/postgresql/$:ArchiveName$`
* ArchiveName: `postgresql-$:Version$-windows-binaries.zip`
* ArchivePath: `pgsql`
* Dir: `postgres`
* Path: `bin`
* Exe: `bin\postgres.exe`
* RegistryKeys: `Software\pgAdmin III`
* Launcher: `PostgreSQL Admin 3`
* LauncherExecutable: `bin\pgAdmin3.exe`
* AdornedExecutables: `bin\pgAdmin3.exe`
* PostgreSqlDataDir: `$HomeDir$\pg_data`
* PostgreSqlLogFile: `$HomeDir$\pg.log`
* Environment:
	+ `PGDATA`: `$:PostgreSqlDataDir$`
	+ `PG_LOG`: `$:PostgreSqlLogFile$`

### Apache

This application needs the x86 version of the [Visual C++ 11 Redistributable][MS VC11] installed.

* ID: `Apache`
* Version: 2.4.20
* Website: <https://httpd.apache.org/>
* Url: `http://www.apachelounge.com/download/VC11/binaries/$:ArchiveName$`
* ArchiveName: `httpd-$:Version$-win32-VC11.zip`
* ArchivePath: `Apache24`
* Path: `bin`
* Exe: `bin\httpd.exe`
* HttpdDocumentRoot: `$HomeDir$\www`
* HttpdListen: `127.0.0.1:80`

### Windows Sysinternals Suite

* ID: `SysInternals`
* Version: latest
* Website: <https://technet.microsoft.com/de-de/sysinternals/bb842062>
* Url: <https://download.sysinternals.com/files/SysinternalsSuite.zip>
* ArchiveName: `SysinternalsSuite.zip`
* Exe: `procexp.exe`
* Launcher: `Process Explorer`


[MS VC11]: https://www.microsoft.com/download/details.aspx?id=30679 "Microsoft Visual C++ Redistributable for Visual Studio 2012"
[MS VC14]: https://www.microsoft.com/download/details.aspx?id=48145 "Microsoft Visual C++ Redistributable for Visual Studio 2015"
[MinGW]: http://www.mingw.org/

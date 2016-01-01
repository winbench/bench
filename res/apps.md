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
* **File**:
  The name of the downloaded file (only for executable downloads like `*.exe` or `*.cmd`).
* **Archive**:
  The name of the downloaded archive with wildcards `?` and `*` (for archives which need to be extracted).
* **ArchiveSubDir**:
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

Some restrictions for the properties:

* The properties _File_ and _Archive_ are mutually exclusive.
* The property _ArchiveSubDir_ is only recognized, if _Archive_ is used.
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

To determine, if a NodeJS package is allready installed, `npm list -g` is called.

## Required

### 7zip

* ID: `SvZ`
* Website: <http://www.7-zip.de/download.html>
* Version: v9.20
* Url: <http://7-zip.org/a/7za920.zip>
* Archive: `7za*.zip`
* Dir: `7z`
* Exe: `7za.exe`

### Less MSIerables

* ID: `LessMsi`
* Website: <http://lessmsi.activescott.com/>
* Version: v1.3
* Url: <https://github.com/activescott/lessmsi/releases/download/v1.3/lessmsi-v1.3.zip>
* Archive: `lessmsi-*.zip`
* Exe: `lessmsi.exe`
* Register: `false`

### Git

* ID: `Git`
* Website: <https://git-scm.com/download/win>
* Version: v2.6.4
* Url: <https://github.com/git-for-windows/git/releases/download/v2.6.4.windows.1/PortableGit-2.6.4-32-bit.7z.exe>
* Archive: `PortableGit-*-32-bit.7z.exe`
* Path: `bin`
* Exe: `git.exe`

## Default

### NodeJS

* ID: `Node`
* Website: <https://nodejs.org>
* Version: v4.2.3
* Url: <https://nodejs.org/dist/v4.2.3/win-x86/node.exe>
* Download: `node.exe`
* Dir: `node`
* Exe: `node.exe`

### NPM Bootstrap

Because _NodeJS_ is downloaded as bare executable, _NPM_ must be installed seperately.
But NPM, in its latest versions, is only distributed as part of the _NodeJS_ setup.
_NPM Bootstrap_ is the last version of _NPM_ which was released seperately.
Therefore, the latest version of _NPM_ is installed afterwards via _NPM Bootstrap_.

* ID: `NpmBootstrap`
* Requires: `Node`
* Website: <https://nodejs.org>
* Version: v1.4.12
* Url: <https://nodejs.org/dist/npm/npm-1.4.12.zip>
* Archive: `npm-*.zip`
* Dir: `$Node:Dir$`
* Exe: `npm.cmd`

### NPM

* ID: `Npm`
* Typ: `node-package`
* Requires: `NpmBootstrap`
* Website: <https://npmjs.org>
* Exe: `npm.cmd`
* ForceInstall: `true`

### Gulp

* ID: `Gulp`
* Typ: `node-package`
* Requires: `Npm`
* Website: <https://www.npmjs.com/package/npm>
* Version: `^3.9.0`
* Exe: `gulp.cmd`

### Bower

* ID: `Bower`
* Typ: `node-package`
* Requires: `Npm`
* Website: <https://www.npmjs.com/package/bower>
* Version: `^1.7.0`
* Exe: `bower.cmd`

### Yeoman

* ID: `Yeoman`
* Typ: `node-package`
* Requires: `Npm`
* Website: <https://www.npmjs.com/package/yeoman>
* Version: `^1.5.0`
* NpmPackage: `yo`
* Exe: `yo.cmd`

### JSHint

* ID: `JSHint`
* Typ: `node-package`
* Requires: `Npm`
* Website: <https://www.npmjs.com/package/jshint>
* Version: `^2.8.0`

### Visual Studio Code

* ID: `VSCode`
* Typ: `node-package`
* Website: <https://code.visualstudio.com/Docs/?dv=win>
* Version: latest
* Url: <http://go.microsoft.com/fwlink/?LinkID=623231>
* Archive: `VSCode-win32.zip`
* Dir: `code`
* Exe: `code.exe`

### Chromium

* ID: `Chromium`
* Website: <http://www.chromium.org/Home>
* Version: 49.0.2593.0
* Url: <https://downloads.sourceforge.net/crportable/ChromiumPortable_49.0.2593.0.paf.exe>
* Archive: `ChromiumPortable_*.paf.exe`

### Pandoc

* ID: `Pandoc`
* Website: <https://github.com/jgm/pandoc/releases/latest>
* Version: v1.15.1.1
* Url: <https://github.com/jgm/pandoc/releases/download/1.15.1.1/pandoc-1.15.1.1-windows.msi>
* Archive: `pandoc-*-windows.msi`
* ArchiveSubDir: `SourceDir\Pandoc`
* Exe: `pandoc.exe`

### Graphviz

* ID: `Graphviz`
* Website: <http://www.graphviz.org/Download_windows.php>
* Version: v2.38
* Url: <http://www.graphviz.org/pub/graphviz/stable/windows/graphviz-2.38.zip>
* Archive: `graphviz-*.zip`
* Path: `release\bin`
* Exe: `dot.exe`

### Inkscape

* ID: `Inkscape`
* Website: <https://inkscape.org/de/herunterladen/>
* Version: v0.91
* Url: <https://inkscape.org/en/gallery/item/3932/download/>
* Archive: `Inkscape-*-win32.7z`
* ArchiveSubDir: `inkscape`
* Exe: `inkscape.exe`

### MikTeX

* ID: `MikTeX`
* Website: <http://miktex.org/portable>
* Version: v2.9.5719
* Url: <http://mirrors.ctan.org/systems/win32/miktex/setup/miktex-portable-2.9.5719.exe>
* Archive: `miktex-portable-2.*.exe`
* Path: `miktex\bin`
* Exe: `latex.exe`

## Optional

### Grunt

* ID: `Grunt`
* Typ: `node-package`
* Requires: `Npm`
* Website: <http://gruntjs.com>
* Version: `^0.4.5`
* Exe: `grunt.cmd`

### Python 2

* ID: `Python2`
* Website: <https://www.python.org/ftp/python/>
* Version: v2.7.11
* Url: <https://www.python.org/ftp/python/2.7.11/python-2.7.11.msi>
* Archive: `python-3.*.msi`
* ArchiveSubDir: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Python 3

* ID: `Python3`
* Website: <https://www.python.org/ftp/python/>
* Version: v3.4.3
* Url: <https://www.python.org/ftp/python/3.4.3/python-3.4.3.msi>
* Archive: `python-2.*.msi`
* ArchiveSubDir: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Sublime Text 3

* ID: `SublimeText3`
* Website: <http://www.sublimetext.com/3>
* Version: Build 3083
* Url: <http://c758482.r82.cf2.rackcdn.com/Sublime%20Text%20Build%203083.zip>
* Archive: `Sublime*Text*Build*.zip`
* Exe: `sublime_text.exe`

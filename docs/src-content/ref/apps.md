+++
date = "2016-06-22T13:44:20+02:00"
description = "A list with all included apps and app groups"
draft = true
title = "App Library"
weight = 1
+++

## Overview

**Groups**

<!--
#data-table /*/Groups/*
#column ID: value(ID)
#column Name: name(.)
-->

**Required Apps**

<!--
#data-table /*/Required Apps/*
#column ID: value(ID)
#column Name: name(.)
#column Version: value(Version)
#column Website: value(Website)
-->

**Optional Apps**

<!--
#data-table /*/Optional Apps/*
#column ID: value(ID)
#column Name: name(.)
#column Version: value(Version)
#column Website: value(Website)
-->

## Groups

### 3D Modeling {#Dev3D}

* ID: `Dev3D`
* Typ: `meta`
* Version: latest
* Dependencies: [Blender](#Blender), [MeshLab](#MeshLab), [GIMP](#Gimp)

### C++ Development {#DevCpp}

* ID: `DevCpp`
* Typ: `meta`
* Version: latest
* Dependencies: [MinGW](#MinGW), [Eclipse for C++](#EclipseCpp)

### Clojure Development {#DevClojure}

* ID: `DevClojure`
* Typ: `meta`
* Version: latest
* Dependencies: [Maven](#Maven), [Leiningen](#Leiningen), [LightTable](#LightTable)

### Java Development {#DevJava}

* ID: `DevJava`
* Typ: `meta`
* Version: latest
* Dependencies: [Java Development Kit 8](#JDK8), [Maven](#Maven), [Eclipse for Java](#EclipseJava)

### LaTeX Writing {#LaTeX}

* ID: `LaTeX`
* Typ: `meta`
* Version: latest
* Dependencies: [MiKTeX](#MiKTeX), [JabRef](#JabRef), [TeXnicCenter](#TeXnicCenter)

### Markdown {#Markdown}

* ID: `Markdown`
* Typ: `meta`
* Version: latest
* Dependencies: [Yeoman Generator for Markdown Projects](#MdProc), [Visual Studio Code](#VSCode)

### Multimedia {#Multimedia}

* ID: `Multimedia`
* Typ: `meta`
* Version: latest
* Dependencies: [Inkscape](#Inkscape), [Dia](#Dia), [GIMP](#Gimp), [Pandoc](#Pandoc), [MiKTeX](#MiKTeX), [Graphics Magick](#GraphicsMagick), [Graphviz](#Graphviz), [FFmpeg](#FFmpeg), [VLC Player](#VLC), [Blender](#Blender)

### Python 2 Development {#DevPython2}

* ID: `DevPython2`
* Typ: `meta`
* Version: latest
* Dependencies: [Python 2](#Python2), [Sublime Text 3](#SublimeText3), [IPython 2](#IPython2)

### Python 3 Development {#DevPython3}

* ID: `DevPython3`
* Typ: `meta`
* Version: latest
* Dependencies: [Python 3](#Python3), [Sublime Text 3](#SublimeText3), [IPython 3](#IPython3)

### Web Development with PHP 5 {#WebDevPHP5}

* ID: `WebDevPHP5`
* Typ: `meta`
* Version: latest
* Dependencies: [PHP 5](#PHP5), [MySQL](#MySQL), [MySQL Workbench](#MySQLWB), [Apache](#Apache), [Eclipse for PHP](#EclipsePHP)

### Web Development with PHP 7 {#WebDevPHP7}

* ID: `WebDevPHP7`
* Typ: `meta`
* Version: latest
* Dependencies: [PHP 7](#PHP7), [MySQL](#MySQL), [MySQL Workbench](#MySQLWB), [Apache](#Apache), [Eclipse for PHP](#EclipsePHP)

## Required Apps

### 7-Zip {#SvZ}

* ID: `SvZ`
* Typ: `default`
* Website: <http://www.7-zip.de/download.html>
* Version: 16.02

### ConEmu {#ConEmu}

* ID: `ConEmu`
* Typ: `default`
* Website: <https://conemu.github.io/>
* Version: 16.06.19

### Inno Setup Unpacker {#InnoUnp}

* ID: `InnoUnp`
* Typ: `default`
* Website: <http://innounp.sourceforge.net/>
* Version: 0.45

### Less MSIerables {#LessMsi}

* ID: `LessMsi`
* Typ: `default`
* Website: <http://lessmsi.activescott.com/>
* Version: 1.3

## Optional Apps

### Ant Renamer {#AntRenamer}

* ID: `AntRenamer`
* Typ: `default`
* Website: <http://antp.be/software/renamer>
* Version: latest

### Apache {#Apache}

* ID: `Apache`
* Typ: `default`
* Website: <https://httpd.apache.org/>
* Version: 2.4.20

### Atom {#Atom}

* ID: `Atom`
* Typ: `default`
* Website: <https://atom.io>
* Version: 1.8.0

### Blender {#Blender}

* ID: `Blender`
* Typ: `default`
* Website: <https://www.blender.org>
* Version: 2.77

### Bower {#Bower}

* ID: `Bower`
* Typ: `node-package`
* Website: <https://bower.io/>
* Version: >=1.7.0 <2.0.0
* Dependencies: [Git](#Git), [NPM](#Npm)

### CMake {#CMake}

* ID: `CMake`
* Typ: `default`
* Website: <https://cmake.org/>
* Version: 3.5.2

### cURL {#cURL}

* ID: `cURL`
* Typ: `default`
* Website: <http://curl.haxx.se/>
* Version: 7.48.0

### Dia {#Dia}

* ID: `Dia`
* Typ: `default`
* Website: <https://wiki.gnome.org/Apps/Dia>
* Version: 0.97.2

### Eclipse for C++ {#EclipseCpp}

* ID: `EclipseCpp`
* Typ: `default`
* Website: <http://www.eclipse.org/>
* Version: 4.5
* Dependencies: [Java Runtime Environment 8](#JRE8)

### Eclipse for Java {#EclipseJava}

* ID: `EclipseJava`
* Typ: `default`
* Website: <http://www.eclipse.org/>
* Version: 4.5
* Dependencies: [Java Runtime Environment 8](#JRE8)

### Eclipse for PHP {#EclipsePHP}

* ID: `EclipsePHP`
* Typ: `default`
* Website: <http://www.eclipse.org/>
* Version: 4.5
* Dependencies: [Java Runtime Environment 8](#JRE8)

### Emacs {#Emacs}

* ID: `Emacs`
* Typ: `default`
* Website: <https://www.gnu.org/software/emacs/>
* Version: 24.5
* Dependencies: [GNU TLS](#GnuTLS)

### FFmpeg {#FFmpeg}

* ID: `FFmpeg`
* Typ: `default`
* Website: <https://www.ffmpeg.org/>
* Version: 20160512-git-cd244fa

### FileZilla {#FileZilla}

* ID: `FileZilla`
* Typ: `default`
* Website: <https://filezilla-project.org/>
* Version: 3.18.0

### GIMP {#Gimp}

* ID: `Gimp`
* Typ: `default`
* Website: <http://www.gimp.org/>
* Version: 2.8.16

### Git {#Git}

* ID: `Git`
* Typ: `default`
* Website: <https://git-scm.com/download/win>
* Version: 2.9.0

### GitKraken {#GitKraken}

* ID: `GitKraken`
* Typ: `default`
* Website: <https://www.gitkraken.com/>
* Version: latest

### GNU TLS {#GnuTLS}

* ID: `GnuTLS`
* Typ: `default`
* Website: <http://www.gnutls.org/>
* Version: 3.3.11

### GnuPG {#GnuPG}

* ID: `GnuPG`
* Typ: `default`
* Website: <https://gnupg.org>
* Version: 2.0.30

### Go {#Go}

* ID: `Go`
* Typ: `default`
* Website: <https://golang.org>
* Version: 1.6

### Graphics Magick {#GraphicsMagick}

* ID: `GraphicsMagick`
* Typ: `default`
* Website: <http://www.graphicsmagick.org/>
* Version: 1.3.23

### Graphviz {#Graphviz}

* ID: `Graphviz`
* Typ: `default`
* Website: <http://www.graphviz.org/>
* Version: 2.38

### Grunt {#Grunt}

* ID: `Grunt`
* Typ: `node-package`
* Website: <http://gruntjs.com>
* Version: >=0.4.5 <0.5.0
* Dependencies: [NPM](#Npm)

### Gulp {#Gulp}

* ID: `Gulp`
* Typ: `node-package`
* Website: <http://gulpjs.com>
* Version: >=3.9.0 <4.0.0
* Dependencies: [NPM](#Npm)

### Hugo {#Hugo}

* ID: `Hugo`
* Typ: `default`
* Website: <https://gohugo.io/>
* Version: 0.16

### Inkscape {#Inkscape}

* ID: `Inkscape`
* Typ: `default`
* Website: <https://inkscape.org/en/download/>
* Version: 0.91-1

### IPython 2 {#IPython2}

* ID: `IPython2`
* Typ: `python2-package`
* Website: <http://pypi.python.org/pypi/ipython>
* Version: latest
* Dependencies: [PyReadline (Python 2)](#PyReadline2), [Python 2](#Python2)

### IPython 3 {#IPython3}

* ID: `IPython3`
* Typ: `python3-package`
* Website: <http://pypi.python.org/pypi/ipython>
* Version: latest
* Dependencies: [PyReadline (Python 3)](#PyReadline3), [Python 3](#Python3)

### JabRef {#JabRef}

* ID: `JabRef`
* Typ: `default`
* Website: <http://www.jabref.org>
* Version: 3.3
* Dependencies: [Java Runtime Environment 8](#JRE8)

### Java Development Kit 7 {#JDK7}

* ID: `JDK7`
* Typ: `default`
* Website: <https://www.oracle.com/java/>
* Version: 7u80

### Java Development Kit 8 {#JDK8}

* ID: `JDK8`
* Typ: `default`
* Website: <https://www.oracle.com/java/>
* Version: 8u92

### Java Runtime Environment 7 {#JRE7}

* ID: `JRE7`
* Typ: `default`
* Website: <https://www.oracle.com/java/>
* Version: 7u80

### Java Runtime Environment 8 {#JRE8}

* ID: `JRE8`
* Typ: `default`
* Website: <https://www.oracle.com/java/>
* Version: 8u92

### JSHint {#JSHint}

* ID: `JSHint`
* Typ: `node-package`
* Website: <http://jshint.com/>
* Version: >=2.8.0 <3.0.0
* Dependencies: [NPM](#Npm)

### Leiningen {#Leiningen}

* ID: `Leiningen`
* Typ: `default`
* Website: <http://leiningen.org>
* Version: latest
* Dependencies: [Java Development Kit 8](#JDK8), [GnuPG](#GnuPG), [Wget](#Wget)

### LightTable {#LightTable}

* ID: `LightTable`
* Typ: `default`
* Website: <http://lighttable.com>
* Version: 0.8.1

### LLVM Clang {#Clang}

* ID: `Clang`
* Typ: `default`
* Website: <http://clang.llvm.org/>
* Version: 3.8.0

### Maven {#Maven}

* ID: `Maven`
* Typ: `default`
* Website: <https://maven.apache.org>
* Version: 3.3.9
* Dependencies: [Java Runtime Environment 8](#JRE8), [GnuPG](#GnuPG)

### MeshLab {#MeshLab}

* ID: `MeshLab`
* Typ: `default`
* Website: <http://meshlab.sourceforge.net/>
* Version: 1.3.3

### MiKTeX {#MiKTeX}

* ID: `MiKTeX`
* Typ: `default`
* Website: <http://miktex.org/portable>
* Version: 2.9.5987

### MinGW {#MinGW}

* ID: `MinGW`
* Typ: `meta`
* Website: <http://www.mingw.org/>
* Version: latest
* Dependencies: [MinGwGet](#MinGwGet), [MinGwGetGui](#MinGwGetGui)

### MinGwGet {#MinGwGet}

* ID: `MinGwGet`
* Typ: `default`
* Version: 0.6.2
* Dependencies: [Wget](#Wget)

### MinGwGetGui {#MinGwGetGui}

* ID: `MinGwGetGui`
* Typ: `default`
* Version: latest
* Dependencies: [MinGwGet](#MinGwGet)

### MySQL {#MySQL}

* ID: `MySQL`
* Typ: `default`
* Website: <http://www.mysql.com/>
* Version: 5.7.12

### MySQL Workbench {#MySQLWB}

* ID: `MySQLWB`
* Typ: `default`
* Website: <http://dev.mysql.com/downloads/workbench/>
* Version: 6.3.6

### Node.js {#Node}

* ID: `Node`
* Typ: `default`
* Website: <https://nodejs.org>
* Version: 6.2.2

### NPM {#Npm}

* ID: `Npm`
* Typ: `default`
* Website: <https://www.npmjs.com/package/npm>
* Version: >=3.7.0 <4.0.0
* Dependencies: [Node.js](#Node)

### NuGet {#NuGet}

* ID: `NuGet`
* Typ: `default`
* Website: <https://www.nuget.org>
* Version: latest

### OpenSSL {#OpenSSL}

* ID: `OpenSSL`
* Typ: `default`
* Website: <https://www.openssl.org/>
* Version: 1.0.2h

### Pandoc {#Pandoc}

* ID: `Pandoc`
* Typ: `default`
* Website: <https://github.com/jgm/pandoc/releases/latest>
* Version: 1.17.1

### PHP 5 {#PHP5}

* ID: `PHP5`
* Typ: `default`
* Website: <http://www.php.net>
* Version: 5.6.21

### PHP 7 {#PHP7}

* ID: `PHP7`
* Typ: `default`
* Website: <http://www.php.net>
* Version: 7.0.6

### PostgreSQL {#PostgreSQL}

* ID: `PostgreSQL`
* Typ: `default`
* Website: <http://www.postgresql.org>
* Version: 9.5.3-1

### Putty {#Putty}

* ID: `Putty`
* Typ: `default`
* Website: <http://www.putty.org>
* Version: latest

### PyReadline (Python 2) {#PyReadline2}

* ID: `PyReadline2`
* Typ: `python2-package`
* Website: <https://pypi.python.org/pypi/pyreadline>
* Version: latest
* Dependencies: [Python 2](#Python2)

### PyReadline (Python 3) {#PyReadline3}

* ID: `PyReadline3`
* Typ: `python3-package`
* Website: <https://pypi.python.org/pypi/pyreadline>
* Version: latest
* Dependencies: [Python 3](#Python3)

### Python 2 {#Python2}

* ID: `Python2`
* Typ: `default`
* Website: <https://www.python.org/>
* Version: 2.7.11

### Python 3 {#Python3}

* ID: `Python3`
* Typ: `default`
* Website: <https://www.python.org/>
* Version: 3.4.4

### Ruby {#Ruby}

* ID: `Ruby`
* Typ: `default`
* Website: <https://www.ruby-lang.org/>
* Version: 2.2.4

### SASS {#Sass}

* ID: `Sass`
* Typ: `ruby-package`
* Website: <http://sass-lang.com/>
* Version: latest
* Dependencies: [Ruby](#Ruby)

### Sift {#Sift}

* ID: `Sift`
* Typ: `default`
* Website: <https://sift-tool.org/>
* Version: 0.8.0

### Spacemacs {#Spacemacs}

* ID: `Spacemacs`
* Typ: `meta`
* Website: <http://spacemacs.org/>
* Version: latest
* Dependencies: [Git](#Git), [Emacs](#Emacs)

### Sublime Text 3 {#SublimeText3}

* ID: `SublimeText3`
* Typ: `default`
* Website: <http://www.sublimetext.com/3>
* Version: Build 3114

### SWare Iron {#Iron}

* ID: `Iron`
* Typ: `default`
* Website: <http://www.chromium.org/Home>
* Version: latest

### SysInternals {#SysInternals}

* ID: `SysInternals`
* Typ: `default`
* Website: <https://technet.microsoft.com/de-de/sysinternals>
* Version: latest

### TeXnicCenter {#TeXnicCenter}

* ID: `TeXnicCenter`
* Typ: `default`
* Website: <http://www.texniccenter.org>
* Version: 2.02
* Dependencies: [MiKTeX](#MiKTeX)

### Vim {#Vim}

* ID: `Vim`
* Typ: `default`
* Website: <http://www.vim.org>
* Version: 7.4
* Dependencies: [VimRT](#VimRT), [VimConsole](#VimConsole)

### VimConsole {#VimConsole}

* ID: `VimConsole`
* Typ: `default`
* Version: 7.4
* Dependencies: [VimRT](#VimRT)

### VimRT {#VimRT}

* ID: `VimRT`
* Typ: `default`
* Version: 7.4

### Visual Studio Code {#VSCode}

* ID: `VSCode`
* Typ: `default`
* Website: <https://code.visualstudio.com/>
* Version: latest

### VLC Player {#VLC}

* ID: `VLC`
* Typ: `default`
* Website: <http://www.videolan.org/vlc/>
* Version: 2.2.4

### Wget {#Wget}

* ID: `Wget`
* Typ: `default`
* Website: <https://www.gnu.org>
* Version: 1.11.4-1
* Dependencies: [WgetDeps](#WgetDeps)

### WgetDeps {#WgetDeps}

* ID: `WgetDeps`
* Typ: `default`
* Version: 1.11.4-1

### WinMerge {#WinMerge}

* ID: `WinMerge`
* Typ: `default`
* Website: <http://winmerge.org/>
* Version: 2.14.0

### Yeoman {#Yeoman}

* ID: `Yeoman`
* Typ: `node-package`
* Website: <http://yeoman.io/>
* Version: >=1.5.0 <2.0.0
* Dependencies: [NPM](#Npm)

### Yeoman Generator for Markdown Projects {#MdProc}

* ID: `MdProc`
* Typ: `node-package`
* Website: <https://www.npmjs.com/package/generator-mdproc>
* Version: >=0.1.6 <0.2.0
* Dependencies: [Yeoman](#Yeoman), [Gulp](#Gulp), [Pandoc](#Pandoc), [Graphviz](#Graphviz), [Inkscape](#Inkscape), [MiKTeX](#MiKTeX), [NPM](#Npm)

### Zeal Docs {#Zeal}

* ID: `Zeal`
* Typ: `default`
* Website: <https://zealdocs.org>
* Version: 0.2.1


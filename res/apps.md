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
* **Label**:
  A user friendly name for the app. 
  (optional, default is the apps ID)
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
* Typ `node-package`: Node.js packages, installable with NPM
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

### Node.js Packages

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
  This property is ignored for Node.js packages.

To determine, if a Node.js package is already installed, the existence of its package folder in
`node_modules` in the Node.js directory is checked.

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

A tool to view and extract the contents of a Windows Installer (.msi) file.

* ID: `LessMsi`
* Label: Less MSIerables
* Version: 1.3
* Website: <http://lessmsi.activescott.com/>
* Url: `https://github.com/activescott/lessmsi/releases/download/v$:Version$/$:ArchiveName$`
* ArchiveName: `lessmsi-v$:Version$.zip`
* Exe: `lessmsi.exe`
* Register: `false`

### 7zip

7-Zip is a file archiver with a high compression ratio.
It comes with a graphical file manager and supports a large range of compression formats for extraction.

* ID: `SvZ`
* Label: 7-Zip
* Website: <http://www.7-zip.de/download.html>
* Docs:
    + Help: `7-zip.chm`
* VersionYear: 16
* VersionNo: 02
* Version: $:VersionYear$.$:VersionNo$
* Release: $:VersionYear$$:VersionNo$
* Url: `http://7-zip.org/a/$:ArchiveName$`
* ArchiveName: `7z$:Release$.msi`
* ArchivePath: `SourceDir\Files\7-Zip`
* Dir: `7z`
* Exe: `7z.exe`
* Launcher: $:Label$
* LauncherExecutable: `7zFM.exe`

### Inno Setup Unpacker

A tool to extract the files from an Inno Setup executable.

* ID: `InnoUnp`
* Label: Inno Setup Unpacker
* Website: <http://innounp.sourceforge.net/>
* Version: 0.45
* Release: 045
* Url: `http://sourceforge.net/projects/innounp/files/innounp/innounp%20$:Version$/$:ArchiveName$`
* ArchiveName: `innounp$:Release$.rar`
* Exe: `innounp.exe`
* Register: `false`

### ConEmu

ConEmu-Maximus5 is a Windows console emulator with tabs, which presents multiple consoles and simple GUI applications as one customizable GUI window with various features. 

* ID: `ConEmu`
* Website: <https://conemu.github.io/>
* Docs:
    + Documentation: <https://conemu.github.io/en/TableOfContents.html>
* Version: 16.03.13
* Release: 160313
* Url: `https://github.com/Maximus5/ConEmu/releases/download/v$:Version$/$:ArchiveName$`
* ArchiveName: `ConEmuPack.$:Release$.7z`
* Launcher: `ConEmu`
* LauncherArguments: `-LoadCfgFile`, `$CustomConfigDir$\ConEmu.xml`,  `%*`

## Groups

### Group: Markdown

* ID: `Markdown`
* Typ: `meta`
* Dependencies: `MdProc`, `VSCode`

### Group: Multimedia

* ID: `Multimedia`
* Typ: `meta`
* Dependencies: `Inkscape`, `Dia`, `Gimp`, `Pandoc`, `MikTeX`, `GraphicsMagick`, `Graphviz`, `FFmpeg`

### Group: 3D Modeling

* ID: `Dev3D`
* Label: 3D Modeling
* Typ: `meta`
* Dependencies: `Blender`, `MeshLab`, `Gimp`

### Group: Web Development with PHP7 and MySQL

* ID: `WebDevPHP7`
* Label: Web Development with PHP 7
* Typ: `meta`
* Dependencies: `PHP7`, `MySQL`, `MySQLWB`, `Apache`, `EclipsePHP`

### Group: Web Development with PHP5 and MySQL

* ID: `WebDevPHP5`
* Label: Web Development with PHP 5
* Typ: `meta`
* Dependencies: `PHP5`, `MySQL`, `MySQLWB`, `Apache`, `EclipsePHP`

### Group: Java Development

* ID: `DevJava`
* Label: Java Development
* Typ: `meta`
* Dependencies: `JDK8`, `Maven`, `EclipseJava`

### Group: Clojure Development

* ID: `DevClojure`
* Label: Clojure Development
* Typ: `meta`
* Dependencies: `Maven`, `Leiningen`, `LightTable`

### Group: Python 2

* ID: `DevPython2`
* Label: Python 2 Development
* Typ: `meta`
* Dependencies: `Python2`, `SublimeText3`, `IPython2`

### Group: Python 3

* ID: `DevPython3`
* Label: Python 3 Development
* Typ: `meta`
* Dependencies: `Python3`, `SublimeText3`, `IPython3`

### Group: C++

* ID: `DevCpp`
* Label: C++ Development
* Typ: `meta`
* Dependencies: `MinGW`, `EclipseCpp`

### Group: LaTeX

* ID: `LaTeX`
* Label: `LaTeX Writing`
* Typ: `meta`
* Dependencies: `MiKTeX`, `JabRef`, `TeXnicCenter`

## Optional

### Git

Git is a free and open source distributed version control system designed to handle everything from small to very large projects with speed and efficiency.

* ID: `Git`
* Website: <https://git-scm.com/download/win>
* Docs:
    + Reference: <https://git-scm.com/docs>
    + Pro Git Book: <https://git-scm.com/book/en/v2>
* Version: 2.9.0
* Release: $:Version$.windows.1
* Url: `https://github.com/git-for-windows/git/releases/download/v$:Release$/$:ArchiveName$`
* ArchiveName: `PortableGit-$:Version$-32-bit.7z.exe`
* Path: `cmd`
* Exe: `cmd\git.exe`
* Environment:
    + `GIT_SSH`: `$:Dir$\usr\bin\ssh.exe`

### GitKraken

The downright luxurious Git client, for Windows, Mac & Linux.

No proxy support yet (Version 1.3.0).

* ID: `GitKraken`
* Version: latest
* Website: <https://www.gitkraken.com/>
* Docs:
    + FAQ: <https://www.gitkraken.com/faq>
* Url: <https://release.gitkraken.com/win32/GitKrakenSetup.exe>
* ArchiveName: `GitKrakenSetup.exe`
* ArchiveTyp: `custom`
* ArchivePath: `lib\net45`
* Launcher: $:Label$

### OpenSSL

OpenSSL is an open source project that provides a robust, commercial-grade, and full-featured toolkit for the Transport Layer Security (TLS) and Secure Sockets Layer (SSL) protocols.
It is also a general-purpose cryptography library.

* ID: `OpenSSL`
* Website: <https://www.openssl.org/>
* Docs:
    + Overview: <https://www.openssl.org/docs/man1.0.2/apps/openssl.html>
    + Commands: <https://www.openssl.org/docs/man1.0.2/apps/>
    + Windows Build: <http://slproweb.com/products/Win32OpenSSL.html>
* Version: 1.0.2h
* Version2: `1_0_2h`
* Url: `http://slproweb.com/download/$:ArchiveName$`
* ArchiveName: `Win32OpenSSL-$:Version2$.exe`
* ArchiveTyp: `inno`
* ArchivePath: `{app}`
* Path: `bin`
* Exe: `bin\openssl.exe`

### Putty

PuTTY is a free (MIT-licensed) Win32 Telnet and SSH client.

* ID: `Putty`
* Website: <http://www.putty.org>
* Docs:
    + User Manual: <http://the.earth.li/~sgtatham/putty/0.67/htmldoc/>
* Version: latest
* Url: <http://the.earth.li/~sgtatham/putty/latest/x86/putty.zip>
* ArchiveName: `putty.zip`
* RegistryKeys: `Software\SimonTatham`
* Launcher: $:Label$

### GNU TLS

The GnuTLS Transport Layer Security Library.

* ID: `GnuTLS`
* Label: GNU TLS
* Website: <http://www.gnutls.org/>
* Docs:
    + Manual: <http://www.gnutls.org/manual/gnutls.html>
* Version: 3.3.11
* Url: `http://sourceforge.net/projects/ezwinports/files/$:ArchiveName$`
* ArchiveName: `gnutls-$:Version$-w32-bin.zip`
* Dir: `gnu`
* Path: `bin`
* Exe: `bin\gnutls-cli.exe`

### GnuPG

GnuPG is a complete and free implementation of the OpenPGP standard as defined by RFC4880 (also known as PGP).
GnuPG allows to encrypt and sign your data and communication, features a versatile key management system
as well as access modules for all kinds of public key directories.
GnuPG, also known as GPG, is a command line tool with features for easy integration with other applications. 

* ID: `GnuPG`
* Website: <https://gnupg.org>
* Docs:
    + Manual: <https://gnupg.org/documentation/manuals/gnupg-2.0/>
    + Commands: <https://gnupg.org/documentation/manuals/gnupg-2.0/Operational-GPG-Commands.html>
* Version: 2.0.30
* Url: `https://sourceforge.net/projects/portableapps/files/GPG Plugin Portable/$:ArchiveName$`
* ArchiveName: `GPG_Plugin_Portable_$:Version$.paf.exe`
* Dir: `gpg`
* Path: `pub`
* Exe: `pub\gpg.exe`

### Wget

GNU Wget is a free utility for non-interactive download of files from the Web.
It supports HTTP, HTTPS, and FTP protocols, as well as retrieval through HTTP proxies.

* ID: `Wget`
* Website: <https://www.gnu.org>
* Docs:
    + Manual: <https://www.gnu.org/software/wget/>
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

curl is an open source command line tool and library for transferring data with URL syntax,
supporting DICT, FILE, FTP, FTPS, Gopher, HTTP, HTTPS, IMAP, IMAPS, LDAP, LDAPS, POP3, POP3S,
RTMP, RTSP, SCP, SFTP, SMB, SMTP, SMTPS, Telnet and TFTP. curl supports SSL certificates,
HTTP POST, HTTP PUT, FTP uploading, HTTP form based upload, proxies, HTTP/2, cookies,
user+password authentication (Basic, Plain, Digest, CRAM-MD5, NTLM, Negotiate and Kerberos),
file transfer resume, proxy tunneling and more.

* ID: `cURL`
* Website: <http://curl.haxx.se/>
* Docs:
    + Manual: <https://curl.haxx.se/docs/manual.html>
    + Man Page: <https://curl.haxx.se/docs/manpage.html>
* Version: 7.48.0
* Url: `https://bintray.com/artifact/download/vszakats/generic/$:ArchiveName$`
* ArchiveName: `curl-$:Version$-win32-mingw.7z`
* ArchivePath: `curl-$:Version$-win32-mingw`
* Path: `bin`
* Exe: `bin\curl.exe`

### FileZilla

FileZilla Client is a free, open source FTP client. It supports FTP, SFTP, and FTPS (FTP over SSL/TLS). 

* ID: `FileZilla`
* Website: <https://filezilla-project.org/>
* Docs:
    + Documentation: <https://wiki.filezilla-project.org/Documentation>
    + Usage: <https://wiki.filezilla-project.org/Using>
    + Tutorial: <https://wiki.filezilla-project.org/FileZilla_Client_Tutorial_(en)>
* Version: 3.18.0
* Url: `https://sourceforge.net/projects/portableapps/files/FileZilla%20Portable/$:ArchiveName$`
* ArchiveName: `FileZillaPortable_$:Version$.paf.exe`
* ArchivePath: `App/filezilla`
* Exe: `filezilla.exe`
* Register: `false`
* Launcher: $:Label$

### Sift

Sift - grep on steroids. A fast and powerful alternative to grep.

* ID: `Sift`
* Website: <https://sift-tool.org/>
* Docs:
    + Documentation: <https://sift-tool.org/docs>
* Version: 0.8.0
* Url: `https://sift-tool.org/downloads/sift/$:ArchiveName$`
* ArchiveName: `sift_$:Version$_windows_386.zip`

### WinMerge

WinMerge is an Open Source differencing and merging tool for Windows.
WinMerge can compare both folders and files, presenting differences in a visual text format
that is easy to understand and handle.

* ID: `WinMerge`
* Website: <http://winmerge.org/>
* Docs:
    + Quick Tour: <http://tour.winmerge.org/>
    + Manual: <http://manual.winmerge.org/>
* Version: 2.14.0
* Url: `https://sourceforge.net/projects/portableapps/files/WinMerge%20Portable/$:ArchiveName$`
* ArchiveName: `WinMergePortable_$:Version$.paf.exe`
* ArchivePath: `App/winmerge`
* Exe: `WinMergeU.exe`
* RegistryKeys: `Software\Thingamahoochie`
* Register: `false`
* Launcher: $:Label$

### Ant Renamer

Ant Renamer is a free program that makes easier the renaming of lots of files and folders
by using specified settings.

* ID: `AntRenamer`
* Label: Ant Renamer
* Website: <http://antp.be/software/renamer>
* Docs:
    + Forum: <http://forum.antp.be/phpbb2/index.php?c=5>
* Version: latest
* Url: <http://update.antp.be/renamer/antrenamer2.zip>
* ArchiveName: `antrenamer2.zip`
* Dir: `renamer`
* Exe: `Renamer.exe`
* Launcher: $:Label$

### Pandoc

Pandoc is a library and command-line tool for converting from one markup format to another.

* ID: `Pandoc`
* Website: <https://github.com/jgm/pandoc/releases/latest>
* Docs:
    + User's Guide: <http://pandoc.org/README.html>
    + FAQ: <http://pandoc.org/faqs.html>
* Version: 1.17.1
* Release: $:Version$-1
* Url: `https://github.com/jgm/pandoc/releases/download/$:Version$/$:ArchiveName$`
* ArchiveName: `pandoc-$:Release$-windows.msi`
* ArchivePath: `SourceDir\Pandoc`
* Exe: `pandoc.exe`

### MikTeX

MiKTeX (pronounced mick-tech) is an up-to-date implementation of TeX/LaTeX
and related programs for Windows (all current variants).

* ID: `MikTeX`
* Website: <http://miktex.org/portable>
* Docs:
    + Manual: <http://docs.miktex.org/2.9/manual/>
    + LaTeX Guides: <https://latex-project.org/guides/>
* Version: 2.9.5987
* Url: `http://mirrors.ctan.org/systems/win32/miktex/setup/$:ArchiveName$`
* ArchiveName: `miktex-portable-$:Version$.exe`
* ArchivePath: `texmfs`
* Path: `install\miktex\bin`
* Exe: `install\miktex\bin\latex.exe`
* Launcher: `MikTeX Tray Icon`
* LauncherExecutable: `install\miktex\bin\miktex-taskbar-icon.exe`
* LauncherIcon: `install\miktex\bin\mo.exe`

### JabRef

JabRef is an open source bibliography reference manager.
The native file format used by JabRef is BibTeX, the standard LaTeX bibliography format.

* ID: `JabRef`
* Dependencies: `JRE8`
* Website: <http://www.jabref.org>
* Docs:
    + Help: <http://help.jabref.org/en/>
    + FAQ: <http://www.jabref.org/faq/>
* Version: 3.3
* FileNo: 1460933278
* Url: `http://app.oldfoss.com/download/JabRef/$:FileNo$$:ResourceName$`
* ResourceName: `JabRef-$:Version$.jar`
* Exe: `$:ResourceName$`
* Launcher: $:Label$
* LauncherExecutable: `$JRE8:Path$\javaw.exe`
* LauncherArguments: `-jar`, `$:Exe$`

### Graphics Magick

GraphicsMagick is the swiss army knife of image processing. It provides a robust
and efficient collection of tools and libraries which support reading, writing,
and manipulating an image in over 88 major formats including important formats
like DPX, GIF, JPEG, JPEG-2000, PNG, PDF, PNM, and TIFF.

* ID: `GraphicsMagick`
* Label: Graphics Magick
* Website: <http://www.graphicsmagick.org/>
* Docs:
    + Utilities: <http://www.graphicsmagick.org/utilities.html>
    + FAQ: <http://www.graphicsmagick.org/FAQ.html>
    + APIs: <http://www.graphicsmagick.org/programming.html>
    + Supported Formats: <http://www.graphicsmagick.org/programming.html>
    + Color Reference: <http://www.graphicsmagick.org/color.html>
* Version: 1.3.23
* Url: `http://sourceforge.net/projects/graphicsmagick/files/graphicsmagick-binaries/$:Version$/$:ArchiveName$`
* ArchiveName: `GraphicsMagick-$:Version$-Q16-win32-dll.exe`
* ArchiveTyp: `inno`
* ArchivePath: `{app}`
* Dir: `gm`
* Exe: `gm.exe`

### FFmpeg

FFmpeg is the leading multimedia framework, able to decode, encode, transcode,
mux, demux, stream, filter and play pretty much anything that humans and machines have created.
It supports the most obscure ancient formats up to the cutting edge.
No matter if they were designed by some standards committee, the community or a corporation.

* ID: `FFmpeg`
* Website: <https://www.ffmpeg.org/>
* Docs:
    + Overview: <http://ffmpeg.org/documentation.html>
    + ffmpeg Tool: <http://ffmpeg.org/ffmpeg.html>
    + ffplay Tool: <http://ffmpeg.org/ffplay.html>
    + ffprobe Tool: <http://ffmpeg.org/ffprobe.html>
    + ffserver Tool: <http://ffmpeg.org/ffserver.html>
    + Wiki: <https://trac.ffmpeg.org/wiki>
* Version: 20160512-git-cd244fa
* Url: `http://ffmpeg.zeranoe.com/builds/win32/shared/$:ArchiveName$`
* ArchiveName: `ffmpeg-$:Version$-win32-shared.7z`
* ArchivePath: `ffmpeg-$:Version$-win32-shared`
* Path: `bin`
* Exe: `bin\ffmpeg.exe`

### VLC

VLC is a free and open source cross-platform multimedia player and framework
that plays most multimedia files, and various streaming protocols.

* ID: `VLC`
* Label: VLC Player
* Website: <http://www.videolan.org/vlc/>
* Docs:
    + Features: <http://www.videolan.org/vlc/features.html>
    + Skins: <http://www.videolan.org/vlc/skins.html>
* Version: 2.2.4
* Url: `http://get.videolan.org/vlc/$:Version$/win32/$:ArchiveName$`
* ArchiveName: `vlc-$:Version$-win32.7z`
* ArchivePath: `vlc-$:Version$`
* Launcher: $:Label$

### Graphviz

Graphviz is open source graph visualization software.
Graph visualization is a way of representing structural information as diagrams
of abstract graphs and networks. It has important applications in networking,
bioinformatics,  software engineering, database and web design, machine learning,
and in visual interfaces for other technical domains. 

* ID: `Graphviz`
* Website: <http://www.graphviz.org/>
* Docs:
    + Overview: <http://www.graphviz.org/Documentation.php>
    + The DOT Language: <http://www.graphviz.org/content/dot-language>
    + Attributes: <http://www.graphviz.org/content/attrs>
    + Color Names: <http://www.graphviz.org/content/color-names>
    + Node Shapes: <http://www.graphviz.org/content/node-shapes>
    + Arrow Shapes: <http://www.graphviz.org/content/arrow-shapes>
    + Command-Line Invocation: <http://www.graphviz.org/content/command-line-invocation>
    + Output Formats: <http://www.graphviz.org/content/output-formats>
* Version: 2.38
* Url:`https://github.com/ErwinJanssen/graphviz/releases/download/v$:Version$/$:ArchiveName$`
* ArchiveName: `graphviz-$:Version$.msi`
* ArchivePath: `SourceDir`
* Path: `bin`
* Exe: `bin\dot.exe`

### Dia

Dia is a program to draw structured diagrams.

* ID: `Dia`
* Website: <https://wiki.gnome.org/Apps/Dia>
* Docs:
    + Overview: <https://wiki.gnome.org/Apps/Dia/Documentation>
    + Manual: <http://dia-installer.de/doc/en/index.html>
    + FAQ: <https://wiki.gnome.org/Apps/Dia/Faq>
* Version: 0.97.2
* Release: 0.97.2-2
* Url: `http://sourceforge.net/projects/dia-installer/files/dia-win32-installer/$:Version$/$:ArchiveName$`
* ArchiveName: `dia-setup-$:Release$-unsigned.exe`
* Path: `bin`
* Exe: `bin\dia.exe`
* Launcher: $:Label$
* LauncherExecutable: `bin\diaw.exe`
* LauncherArguments: `--integrated`, `%*`

### Inkscape

Inkscape is a professional vector graphics editor for Windows, Mac OS X and Linux.
It's free and open source.

* ID: `Inkscape`
* Website: <https://inkscape.org/en/download/>
* Docs:
    + Manual: <http://tavmjong.free.fr/INKSCAPE/MANUAL/html/>
    + Tutorials: <https://inkscape.org/en/learn/tutorials/>
    + FAQ: <https://inkscape.org/en/learn/faq/>
    + Command Line Reference: <https://inkscape.org/en/doc/inkscape-man.html>
    + Keyboard Shortcuts: <https://inkscape.org/en/doc/keys091.html>
* Version: 0.91-1
* Url: <https://inkscape.org/en/gallery/item/3932/download/>
* ArchiveName: `Inkscape-$:Version$-win32.7z`
* ArchivePath: `inkscape`
* Exe: `inkscape.exe`
* Launcher: $:Label$

### GIMP

The GNU Image Manipulation Program.

GIMP is a cross-platform image editor.
Whether you are a graphic designer, photographer, illustrator, or scientist,
GIMP provides you with sophisticated tools to get your job done.

* ID: `Gimp`
* Label: GIMP
* Website: <http://www.gimp.org/>
* Docs:
    + Manual: <http://docs.gimp.org/2.8/en/>
    + Tutorials: <http://www.gimp.org/tutorials/>
* Version: 2.8.16
* Url: `https://sourceforge.net/projects/portableapps/files/GIMP Portable/$:ArchiveName$`
* ArchiveName: `GIMPPortable_$:Version$.paf.exe`
* ArchivePath: `App/gimp`
* Exe: `bin\gimp-2.8.exe`
* Register: `false`
* Launcher: $:Label$

### MeshLab

MeshLab is an open source, portable, and extensible system for the processing
and editing of unstructured 3D triangular meshes.
The system is aimed to help the processing of the typical not-so-small
unstructured models arising in 3D scanning, providing a set of tools for editing,
cleaning, healing, inspecting, rendering and converting this kind of meshes. 

* ID: `MeshLab`
* VersionMajor: 1
* VersionMinor: 3
* VersionRevision: 3
* Version: $:VersionMajor$.$:VersionMinor$.$:VersionRevision$
* Website: <http://meshlab.sourceforge.net/>
* Url: `https://sourceforge.net/projects/meshlab/files/meshlab/MeshLab%20v$:Version$/$:ArchiveName$`
* ArchiveName: `MeshLab_v$:VersionMajor$$:VersionMinor$$:VersionRevision$.exe`
* Exe: `meshlab_32.exe`
* Launcher: $:Label$

### Blender

Blender is the open source, cross platform suite of tools for 3D creation.

* ID: `Blender`
* Website: <https://www.blender.org>
* Docs:
    + Features: <https://www.blender.org/features/>
    + Tutorials: <https://www.blender.org/support/tutorials/>
    + Manual: <https://www.blender.org/manual/>
    + Python API: <https://www.blender.org/api/blender_python_api_2_77_1/>
* Version: 2.77
* VersionSuffix: a
* Url: `http://download.blender.org/release/Blender$:Version$/$:ArchiveName$`
* ArchiveName: `blender-$:Version$$:VersionSuffix$-windows32.zip`
* ArchivePath: `blender-$:Version$$:VersionSuffix$-windows32`
* Launcher: $:Label$

### Node.js

Node.js is a JavaScript runtime built on Chrome's V8 JavaScript engine.
Node.js uses an event-driven, non-blocking I/O model that makes it lightweight and efficient.
Node.js' package ecosystem, npm, is the largest ecosystem of open source libraries in the world.

* ID: `Node`
* Label: Node.js
* Website: <https://nodejs.org>
* Docs:
    + API Documentation: <https://nodejs.org/dist/latest-v4.x/docs/api/>
    + Guides: <https://nodejs.org/en/docs/guides/>
* Version: 4.4.5
* Url: `https://nodejs.org/dist/v$:Version$/win-x86/node.exe`
* ResourceName: `node.exe`
* Dir: `node`
* Exe: `node.exe`
* Launcher: $:Label$

### NPM

npm is the package manager for JavaScript.
Find, share, and reuse packages of code from hundreds of thousands of 
developers — and assemble them in powerful new ways.

Because _Node.js_ is downloaded as bare executable, _NPM_ must be installed seperately.
But NPM, in its latest versions, is only distributed as part of the _Node.js_ setup.
_NPM_ 1.4.12 is the last version of _NPM_ which was released seperately.
Therefore, the latest version of _NPM_ is installed afterwards via the setup script `auto\apps\npm.setup.ps1`.

* ID: `Npm`
* Label: NPM
* Dependencies: `Node`
* Website: <https://www.npmjs.com/package/npm>
* Version: `>=3.7.0 <4.0.0`
* Url: <https://nodejs.org/dist/npm/npm-1.4.12.zip>
* ArchiveName: `npm-1.4.12.zip`
* Dir: `$Node:Dir$`
* Exe: `npm.cmd`

### Gulp

The streaming build system.

* ID: `Gulp`
* Typ: `node-package`
* Website: <http://gulpjs.com>
* Docs:
    + npm Package: <https://www.npmjs.com/package/gulp>
    + Getting Started: <https://github.com/gulpjs/gulp/blob/master/docs/getting-started.md
    + API: <https://github.com/gulpjs/gulp/blob/master/docs/API.md>
    + Command-Line: <https://github.com/gulpjs/gulp/blob/master/docs/CLI.md>
    + Unofficial Documentation: <http://gulpjs.org/>
* Version: `>=3.9.0 <4.0.0`
* Exe: `gulp.cmd`

### Grunt

The JavaScript Task Runner

* ID: `Grunt`
* Typ: `node-package`
* Website: <http://gruntjs.com>
* Docs:
    + npm Package: <https://www.npmjs.org/package/grunt>
    + Getting Started: <http://gruntjs.com/getting-started>
    + Sample Gruntfile: <http://gruntjs.com/sample-gruntfile>
    + Creating Tasks: <http://gruntjs.com/creating-tasks>
    + Command-Line: <http://gruntjs.com/using-the-cli>
    + API: <http://gruntjs.com/api/grunt>
* Version: `>=0.4.5 <0.5.0`
* Exe: `grunt.cmd`

### Bower

Web sites are made of lots of things — frameworks, libraries, assets, and utilities.
Bower manages all these things for you.

Bower can manage components that contain HTML, CSS, JavaScript, fonts or even image files.
Bower doesn’t concatenate or minify code or do anything else - it just installs
the right versions of the packages you need and their dependencies.

* ID: `Bower`
* Typ: `node-package`
* Website: <https://bower.io/>
* Docs:
    + npm Package: <https://www.npmjs.com/package/bower>
    + Getting Started: <https://bower.io/#getting-started>
    + Commands: <https://bower.io/docs/api/#commands>
* Version: `>=1.7.0 <2.0.0`
* Exe: `bower.cmd`

### Yeoman

The web's scaffolding tool for modern webapps.

Yeoman helps you to kickstart new projects, prescribing best practices and tools
to help you stay productive.

* ID: `Yeoman`
* Typ: `node-package`
* PackageName: `yo`
* Website: <http://yeoman.io/>
* Docs:
    + npm Package: <https://www.npmjs.com/package/yo>
    + Getting Started: <http://yeoman.io/learning/index.html>
    + Tutorial: <http://yeoman.io/codelab/index.html>
    + Generators: <http://yeoman.io/generators/>
    + Creating a Generator: <http://yeoman.io/authoring/>
* Version: `>=1.5.0 <2.0.0`
* Exe: `yo.cmd`

### Yeoman Generator for Markdown Projects

A project generator for Markdown projects with support for building
HTML, PDF, and DOCX output. Leveraging Pandoc, Graphviz and a number of
Node.js packages to pre-process the Markdown and post-process the output.

* ID: `MdProc`
* Label: Yeoman Generator for Markdown Projects
* Typ: `node-package`
* PackageName: `generator-mdproc`
* Website: <https://www.npmjs.com/package/generator-mdproc>
* Version: `>=0.1.6 <0.2.0`
* Dependencies: `Yeoman`, `Gulp`, `Pandoc`, `Graphviz`, `Inkscape`, `MikTeX`

### JSHint

JSHint is a tool that helps to detect errors and potential problems in your JavaScript code.

* ID: `JSHint`
* Typ: `node-package`
* Website: <http://jshint.com/>
* Docs:
    + npm Package: <https://www.npmjs.com/package/jshint>
    + Documentation: <http://jshint.com/docs/>
    + Command-Line: <http://jshint.com/docs/cli/>
    + API: <http://jshint.com/docs/api/>
* Version: `>=2.8.0 <3.0.0`
* Exe: `jshint.cmd`

### Python 2

Python is a programming language that lets you work quickly and integrate systems more effectively.

* ID: `Python2`
* Label: Python 2
* Website: <https://www.python.org/>
* Docs:
    + Documentation: <https://docs.python.org/2/>
    + Language Reference: <https://docs.python.org/2/reference/index.html>
    + Library Reference: <https://docs.python.org/2/library/index.html>
* Version: 2.7.11
* Url: `https://www.python.org/ftp/python/$:Version$/$:ArchiveName$`
* ArchiveName: `python-$:Version$.msi`
* ArchivePath: `SourceDir`
* Path: `.`, `Scripts`
* Exe: `python.exe`

### Python 3

Python is a programming language that lets you work quickly and integrate systems more effectively.

* ID: `Python3`
* Label: Python 3
* Website: <https://www.python.org/>
* Docs:
    + Documentation: <https://docs.python.org/3/>
    + Language Reference: <https://docs.python.org/3/reference/index.html>
    + Library Reference: <https://docs.python.org/3/library/index.html>
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
* Label: PyReadline (Python 2)
* PackageName: `pyreadline`
* Typ: `python2-package`
* Website: <https://pypi.python.org/pypi/pyreadline>

for Python 3:

* ID: `PyReadline3`
* Label: PyReadline (Python 3)
* PackageName: `pyreadline`
* Typ: `python3-package`
* Website: <https://pypi.python.org/pypi/pyreadline>

### IPython

IPython provides a rich architecture for computing with a powerful interactive shell.

for Python 2:

* ID: `IPython2`
* Label: IPython 2
* Typ: `python2-package`
* PackageName: `ipython`
* Dependencies: `PyReadline2`
* Website: <http://pypi.python.org/pypi/ipython>
* Docs:
    + Documentation: <http://ipython.readthedocs.io/en/stable/>
* Exe: `Scripts\ipython2.exe`
* Launcher: $:Label$

for Python 3:

* ID: `IPython3`
* Label: IPython 3
* Typ: `python3-package`
* PackageName: `ipython`
* Dependencies: `PyReadline3`
* Website: <http://pypi.python.org/pypi/ipython>
* Docs:
    + Documentation: <http://ipython.readthedocs.io/en/stable/>
* Exe: `Scripts\ipython3.exe`
* Launcher: $:Label$

### Ruby

A dynamic, open source programming language with a focus on simplicity and productivity.
It has an elegant syntax that is natural to read and easy to write.

* ID: `Ruby`
* Website: <https://www.ruby-lang.org/>
* Docs:
    + Documentation: <https://www.ruby-lang.org/en/documentation/>
    + Programming Ruby: <http://ruby-doc.com/docs/ProgrammingRuby/>
    + Libraries: <https://www.ruby-lang.org/en/libraries/>
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

Sass is the most mature, stable, and powerful professional grade CSS extension language in the world.

* ID: `Sass`
* Label: SASS
* Typ: `ruby-package`
* Website: <http://sass-lang.com/>
* Docs:
    + Guide: <http://sass-lang.com/guide>
    + Reference: <http://sass-lang.com/documentation/file.SASS_REFERENCE.html>

### PHP 5

PHP is a popular general-purpose scripting language that is especially suited to web development.
Fast, flexible and pragmatic, PHP powers everything from your blog to the most popular websites in the world.

This application needs the x86 version of the [Visual C++ 11 Redistributable][MS VC11] installed.

* ID: `PHP5`
* Label: PHP 5
* Website: <http://www.php.net>
* Docs:
    + PHP Manual: <http://php.net/manual/en/>
* Version: 5.6.21
* Url: `http://windows.php.net/downloads/releases/archives/$:ArchiveName$`
* ArchiveName: `php-$:Version$-Win32-VC11-x86.zip`
* Exe: `php.exe`

### PHP 7

PHP is a popular general-purpose scripting language that is especially suited to web development.
Fast, flexible and pragmatic, PHP powers everything from your blog to the most popular websites in the world.

This application needs the x86 version of the [Visual C++ 14 Redistributable][MS VC14] installed.

* ID: `PHP7`
* Label: PHP 7
* Website: <http://www.php.net>
* Docs:
    + PHP Manual: <http://php.net/manual/en/>
* Version: 7.0.6
* Url: `http://windows.php.net/downloads/releases/archives/$:ArchiveName$`
* ArchiveName: `php-$:Version$-Win32-VC14-x86.zip`
* Exe: `php.exe`

### Java Runtime Environment 7

According to Oracle, Java is the world's #1 programming language.

The runtime environment is required for a compiled Java program to get executed.

* ID: `JRE7`
* Label: Java Runtime Environment 7
* Website: <https://www.oracle.com/java/>
* Docs:
    + Downloads: <http://www.oracle.com/technetwork/java/javase/downloads/java-archive-downloads-javase7-521261.html>
* Version: 7u80
* Release: b15
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jre-$:Version$-windows-i586.tar.gz`
* ArchivePath: `jre1.7.0_80`
* Path: `bin`
* Exe: `bin\java.exe`
* Environment:
    + `JAVA_CMD`: `$:Exe$`

### Java Runtime Environment 8

According to Oracle, Java is the world's #1 programming language.

The runtime environment is required for a compiled Java program to get executed.

* ID: `JRE8`
* Label: Java Runtime Environment 8
* Website: <https://www.oracle.com/java/>
* Docs:
    + Downloads: <http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html>
* Version: 8u92
* Release: b14
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jre-$:Version$-windows-i586.tar.gz`
* ArchivePath: `jre1.8.0_92`
* Path: `bin`
* Exe: `bin\java.exe`
* Environment:
    + `JAVA_CMD`: `$:Exe$`

### Java Development Kit 7

According to Oracle, Java is the world's #1 programming language.

The development kit is required for Java source code to get compiled.

* ID: `JDK7`
* Label: Java Development Kit 7
* Website: <https://www.oracle.com/java/>
* Docs:
    + Downloads: <http://www.oracle.com/technetwork/java/javase/downloads/java-archive-downloads-javase7-521261.html>
    + Documentation: <http://docs.oracle.com/javase/7/docs/>
    + Java SE 7 API: <http://docs.oracle.com/javase/7/docs/api/index.html>
* Version: $JRE7:Version$
* Release: $JRE7:Release$
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jdk-$:Version$-windows-i586.exe`
* Path: `bin`
* Exe: `bin\javac.exe`
* Environment:
    + `JAVA_HOME`: `$:Dir$`
    + `JAVA_CMD`: `$:Dir$\jre\bin\java.exe`

### Java Development Kit 8

According to Oracle, Java is the world's #1 programming language.

The development kit is required for Java source code to get compiled.

* ID: `JDK8`
* Label: Java Development Kit 8
* Website: <https://www.oracle.com/java/>
* Docs:
    + Downloads: <http://www.oracle.com/technetwork/java/javase/downloads/index.html>
    + Documentation: <http://docs.oracle.com/javase/8/docs/>
    + Java SE 8 API: <http://docs.oracle.com/javase/8/docs/api/index.html>
* Version: $JRE8:Version$
* Release: $JRE8:Release$
* Url: `http://download.oracle.com/otn-pub/java/jdk/$:Version$-$:Release$/$:ArchiveName$`
* DownloadCookies: `oraclelicense: accept-securebackup-cookie`
* ArchiveName: `jdk-$:Version$-windows-i586.exe`
* Path: `bin`
* Exe: `bin\javac.exe`
* Environment:
    + `JAVA_HOME`: `$:Dir$`
    + `JAVA_CMD`: `$:Dir$\jre\bin\java.exe`

### Maven

Apache Maven is a software project management and comprehension tool.
Based on the concept of a project object model (POM), Maven can manage a project's build,
reporting and documentation from a central piece of information.

* ID: `Maven`
* Dependencies: `JRE8`, `GnuPG`
* Website: <https://maven.apache.org>
* Docs:
    + Reference: `https://maven.apache.org/ref/$:Version$/`
    + API Docs: `https://maven.apache.org/ref/$:Version$/apidocs/index.html`
* Version: `3.3.9`
* Url: `http://www-eu.apache.org/dist/maven/maven-3/$:Version$/binaries/$:ArchiveName$`
* ArchiveName: `apache-maven-$:Version$-bin.zip`
* ArchivePath: `apache-maven-$:Version$`
* Dir: `mvn`
* Path: `bin`
* Exe: `bin\mvn.cmd`

### Leiningen

Leiningen is the easiest way to use Clojure.
With a focus on project automation and declarative configuration,
it gets out of your way and lets you focus on your code.

* ID: `Leiningen`
* Dependencies: `JDK8`, `GnuPG`, `Wget`
* Website: <http://leiningen.org>
* Docs:
    + Tutorial: <https://github.com/technomancy/leiningen/blob/stable/doc/TUTORIAL.md>
    + Sample project.clj: <https://github.com/technomancy/leiningen/blob/stable/sample.project.clj>
    + FAQ: <https://github.com/technomancy/leiningen/blob/stable/doc/FAQ.md>
    + Clojure Documentation: <http://clojure-doc.org/>
    + Clojure Reference: <http://clojure.org/reference/documentation>
    + Clojure API: <http://clojure.github.io/clojure/>
* Version: latest
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
* Docs:
    + FAQ: <http://mingw.org/wiki/FAQ>
    + HOWTO: <http://mingw.org/wiki/HOWTO>
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

CMake is an open-source, cross-platform family of tools designed to build,
test and package software. CMake is used to control the software compilation process
using simple platform and compiler independent configuration files, and generate native
makefiles and workspaces that can be used in the compiler environment of your choice.
The suite of CMake tools were created by Kitware in response to the need for a powerful,
cross-platform build environment for open-source projects such as ITK and VTK.

Usually you want to use this app with _MinGW_.

To setup a C/C++ project with CMake and MinGW (`mingw32-make`), you have to activate the _MinGW_ app with the `mingw32-make` package.
Setup your project with a `CMakeLists.txt` file and run `cmake -G "MinGW Makefiles" <project folder>` to generate the `Makefile`. Run `cmake --build <project folder>` to compile the project.

* ID: `CMake`
* Website: <https://cmake.org/>
* MajorVersion: 3.5
* Version: $:MajorVersion$.2
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
* Label: LLVM Clang
* Version: 3.8.0
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

Go is an open source programming language that makes it easy
to build simple, reliable, and efficient software.

* ID: `Go`
* Version: 1.6
* Website: <https://golang.org>
* Docs:
    + Overview: <https://golang.org/doc/>
    + FAQ: <https://golang.org/doc/faq>
    + Language Specification: <https://golang.org/ref/spec>
    + Command-Line: <https://golang.org/doc/cmd>
    + Packages: <https://golang.org/pkg/>
* Url: `https://storage.googleapis.com/golang/$:ArchiveName$`
* ArchiveName: `go$:Version$.windows-386.zip`
* ArchivePath: `go`
* Path: `bin`
* Exe: `bin\go.exe`
* Environment:
    + `GOROOT`: `$:Dir$`

### NuGet

NuGet is the package manager for the Microsoft development platform including .NET.
The NuGet client tools provide the ability to produce and consume packages.
The NuGet Gallery is the central package repository used by all package authors and consumers.

* ID: `NuGet`
* Website: <https://www.nuget.org>
* Docs:
    + Consume: <https://docs.nuget.org/consume>
    + Create: <https://docs.nuget.org/create>
    + Command-Line: <https://docs.nuget.org/Consume/Command-Line-Reference>
    + Configuration File: <https://docs.nuget.org/Consume/NuGet-Config-File>
* Version: latest
* Url: <https://dist.nuget.org/win-x86-commandline/latest/nuget.exe>
* ResourceName: `nuget.exe`

### Zeal

An offline documentation browser inspired by [Dash](https://kapeli.com/dash/).

* ID: `Zeal`
* Label: Zeal Docs
* Website: <https://zealdocs.org>
* Version: 0.2.1
* Url: `https://bintray.com/artifact/download/zealdocs/windows/$:ArchiveName$`
* ArchiveName: `zeal-$:Version$-windows-x86.msi`
* ArchivePath: `SourceDir\PFiles\Zeal`
* RegistryKeys: `Software\Zeal`
* Launcher: $:Label$

### Visual Studio Code

A cross platform code editor from Microsoft.

* ID: `VSCode`
* Label: Visual Studio Code
* Website: <https://code.visualstudio.com/>
* Docs:
    + Documentation: <https://code.visualstudio.com/Docs>
* Version: latest
* Url: <http://go.microsoft.com/fwlink/?LinkID=623231>
* ArchiveName: `VSCode-win32.zip`
* Dir: `code`
* Exe: `code.exe`
* Launcher: $:Label$

### LightTable

The next generation code editor.

* ID: `LightTable`
* Website: <http://lighttable.com>
* Docs:
    + Documentation: <http://docs.lighttable.com/>
* Version: 0.8.1
* Url: `https://github.com/LightTable/LightTable/releases/download/$:Version$/$:ArchiveName$`
* ArchiveName: `lighttable-$:Version$-windows.zip`
* ArchivePath: `lighttable-$:Version$-windows`
* Dir: `lt`
* Exe: `LightTable.exe`
* Launcher: $:Label$

### Sublime Text 3

Sublime Text is a sophisticated text editor for code, markup and prose.
You'll love the slick user interface, extraordinary features and amazing performance.

* ID: `SublimeText3`
* Label: Sublime Text 3
* Website: <http://www.sublimetext.com/3>
* Docs:
    + Documentation: <https://www.sublimetext.com/docs/3/>
    + Unofficial Documentation: <http://docs.sublimetext.info/en/latest/index.html>
    + Package Control: <https://packagecontrol.io/>
* Version: Build 3114
* Url: `https://download.sublimetext.com/$:ArchiveName$`
* ArchiveName: `Sublime Text $:Version$.zip`
* Exe: `sublime_text.exe`
* Launcher: $:Label$

### TeXnicCenter

Premium LaTeX Editing for Windows.

* ID: `TeXnicCenter`
* Dependencies: `MikTeX`
* Website: <http://www.texniccenter.org>
* Docs:
    + Features: <http://www.texniccenter.org/about/feature/>
    + Documentation: <http://texniccenter.sourceforge.net/>
* Version: 2.02
* Url: `http://sourceforge.net/projects/texniccenter/files/TeXnicCenter/$:Version$%20Stable/$:ArchiveName$`
* ArchiveName: `TXCSetup_$:Version$Stable_Win32.exe`
* ArchiveTyp: `inno`
* ArchivePath: `{app}`
* RegistryKeys: `SOFTWARE\ToolsCenter`
* Launcher: $:Label$

### Emacs

An extensible, customizable, free text editor - and more.

GNU Emacs at its core is an interpreter for Emacs Lisp, a dialect of the Lisp programming language
with extensions to support text editing. 

* ID: `Emacs`
* Dependencies: `GnuTLS`
* Website: <https://www.gnu.org/software/emacs/>
* Docs:
    + Manual: <https://www.gnu.org/software/emacs/manual/html_node/emacs/index.html>
    + Emacs Lisp: <https://www.gnu.org/software/emacs/manual/html_node/elisp/index.html>
    + Other Manuals: <https://www.gnu.org/software/emacs/manual/index.html>
* Version: 24.5
* Url: `http://ftp.gnu.org/gnu/emacs/windows/$:ArchiveName$`
* ArchiveName: `emacs-$:Version$-bin-i686-mingw32.zip`
* Dir: `gnu`
* Path: `bin`
* Exe: `bin\emacs.exe`
* Launcher: $:Label$
* LauncherExecutable: `$:Dir$\bin\runemacs.exe`

### Spacemacs

The best editor is neither Emacs nor Vim, it's Emacs and Vim!

* ID: `Spacemacs`
* Typ: `meta`
* Dependencies: `Git`, `Emacs`
* Website: <http://spacemacs.org/>
* Docs:
    + Documentation: <http://spacemacs.org/doc/DOCUMENTATION.html>
    + Configuration Layers: <http://spacemacs.org/doc/LAYERS.html>
    + Layers: <http://spacemacs.org/layers/LAYERS.html>
* SetupTestFile: `$HomeDir$\.emacs.d\spacemacs.mk`

### Eclipse

Eclipse for Java development:

* ID: `EclipseJava`
* Label: Eclipse for Java
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
* Launcher: $:Label$

Eclipse for PHP development:

* ID: `EclipsePHP`
* Label: Eclipse for PHP
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
* Launcher: $:Label$

Eclipse for C/C++ development:

* ID: `EclipseCpp`
* Label: Eclipse for C++
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
* Launcher: $:Label$

### SRWare Iron

A free portable derivative of Chromium, optimized for privacy.

* ID: `Iron`
* Label: SWare Iron
* Website: <http://www.chromium.org/Home>
* Version: latest
* Url: <http://www.srware.net/downloads/IronPortable.zip>
* ArchivePath: `IronPortable\Iron`
* ArchiveName: `IronPortable.zip`
* Exe: `chrome.exe`
* Launcher: $:Label$

### MySQL

According to Oracle:
MySQL Community Edition is the freely downloadable version
of the world's most popular open source database.

The MySQL data is stored in `%USERPROFILE%\mysql_data`.
You can start the MySQL server by running `mysql_start` in the _Bench_ shell.
You can stop the MySQL server by running `mysql_stop` in the _Bench_ shell.
The initial password for _root_ is `bench`.

* ID: `MySQL`
* Website: <http://www.mysql.com/>
* Docs:
    + Documentation: <http://dev.mysql.com/doc/refman/5.7/en/>
    + SQL Syntax: <http://dev.mysql.com/doc/refman/5.7/en/sql-syntax.html>
    + Data Types: <http://dev.mysql.com/doc/refman/5.7/en/data-types.html>
    + Functions: <http://dev.mysql.com/doc/refman/5.7/en/functions.html>
* Version: 5.7.12
* Url: `http://dev.mysql.com/get/Downloads/MySQL-5.7/$:ArchiveName$`
* ArchiveName: `mysql-$:Version$-win32.zip`
* ArchivePath: `mysql-$:Version$-win32`
* Path: `bin`
* Exe: `bin\mysqld.exe`
* MySqlDataDir: `$HomeDir$\mysql_data`
* Environment:
    + `MYSQL_DATA`: `$:MySqlDataDir$`

### MySQL Workbench

MySQL Workbench is a unified visual tool for database architects, developers, and DBAs.
MySQL Workbench provides data modeling, SQL development, and comprehensive administration
tools for server configuration, user administration, backup, and much more.

* ID: `MySQLWB`
* Label: MySQL Workbench
* Website: <http://dev.mysql.com/downloads/workbench/>
* Docs:
    + Documentation: <http://dev.mysql.com/doc/workbench/en/>
* Version: 6.3.6
* Url: `http://dev.mysql.com/get/Downloads/MySQLGUITools/$:ArchiveName$`
* ArchiveName: `mysql-workbench-community-$:Version$-win32-noinstall.zip`
* ArchivePath: `MySQL Workbench 6.3.6 CE (win32)`
* Exe: `MySQLWorkbench.exe`
* Register: `false`
* Launcher: $:Label$

### PostgreSQL

PostgreSQL is a powerful, open source object-relational database system.
It has more than 15 years of active development and a proven architecture
that has earned it a strong reputation for reliability, data integrity, and correctness.
It is fully ACID compliant, has full support for foreign keys, joins, views,
triggers, and stored procedures (in multiple languages).
It also supports storage of binary large objects, including pictures, sounds, or video.
It has native programming interfaces for C/C++, Java, .Net, Perl, Python,
Ruby, Tcl, ODBC, among others

Contains the _PostgreSQL Server_ and the management tool _pgAdminIII_.
The initial password for _postgres_ is `bench`.

* ID: `PostgreSQL`
* Website: <http://www.postgresql.org>
* Docs:
    + Documentation: <https://www.postgresql.org/docs/9.5/static/index.html>
* Version: 9.5.3-1
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

The Apache HTTP Server is a secure, efficient and extensible server
that provides HTTP services in sync with the current HTTP standards.
The Apache HTTP Server is the most popular web server since over 20 years.

This application needs the x86 version of the [Visual C++ 11 Redistributable][MS VC11] installed.

* ID: `Apache`
* Website: <https://httpd.apache.org/>
* Docs:
    + Documentation: <http://httpd.apache.org/docs/2.4/en/>
* Version: 2.4.20
* Url: `http://www.apachelounge.com/download/VC11/binaries/$:ArchiveName$`
* ArchiveName: `httpd-$:Version$-win32-VC11.zip`
* ArchivePath: `Apache24`
* Path: `bin`
* Exe: `bin\httpd.exe`
* HttpdDocumentRoot: `$HomeDir$\www`
* HttpdListen: `127.0.0.1:80`

### Windows Sysinternals Suite

A collection of tools by Mark Russinovich, to inspect and investigate
the Microsoft Windows operating systems and its processes.

* ID: `SysInternals`
* Website: <https://technet.microsoft.com/de-de/sysinternals>
* Docs:
    + Downloads: <https://technet.microsoft.com/de-de/sysinternals/bb842062>
    + Forum: <http://forum.sysinternals.com/>
    + Blog: <https://blogs.technet.microsoft.com/sysinternals/>
    + Learning Resources: <https://technet.microsoft.com/de-de/sysinternals/bb469930>
* Version: latest
* Url: <https://download.sysinternals.com/files/SysinternalsSuite.zip>
* ArchiveName: `SysinternalsSuite.zip`
* Exe: `procexp.exe`
* Launcher: `Process Explorer`


[MS VC11]: https://www.microsoft.com/download/details.aspx?id=30679 "Microsoft Visual C++ Redistributable for Visual Studio 2012"
[MS VC14]: https://www.microsoft.com/download/details.aspx?id=48145 "Microsoft Visual C++ Redistributable for Visual Studio 2015"
[MinGW]: http://www.mingw.org/

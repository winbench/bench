# Changelog

All notable changes to this project will be documented in this file.
Until this project reaches the maturity of version 1.0.0, the versioning
follows the following rules:

* The MAJOR number is 0.
* The MINOR number increases, if backward compatibility was broken
  or an important set of features has been added.
* The PATCH number increases, if backward compatible features
  have been added or bugs got fixed.

After this project reaches the version number 1.0.0 it will follow
[Semantic Versioning](http://semver.org/).

<!--
This document follows the guidelines in http://keepachangelog.md.

Use the following change groups: Added, Changed, Deprecated, Removed, Fixed, Security
Add a link to the GitHub diff like
[Full Changelog](https://github.com/mastersign/bench/compare/v<last-version>...v<this-version>)
-->

## [Unreleased]
[Dev Changes](https://github.com/mastersign/bench/compare/master...dev),

### Added
- Bench CLI
  ([#87](https://github.com/mastersign/bench/issues/87))
- Update check in the About dialog of _BenchDashboard_
- _Upgrade Bench_ entry in the _Setup_ menu of the setup dialog of _BenchDashboard_
- Configurable columns to the Setup dialog of _BenchDashboard_
- Config properties
    + `VersionUrl`
    + `UpdateUrlTemplate`
    + `BootstrapUrlTemplate`
    + `AutoUpdateCheck`
    + `KnownLicenses`
- Support for multiple app libraries
  ([#90](https://github.com/mastersign/bench/issues/90))
- Namespaces for app IDs
- Config properties:
    + `AppLibs`  
      (this property must be overriden in the user or site config, to load more than the core app library)
    + `AppLibsDownloadDir`
    + `AppLibsDir`
    + `AppLibIndexFileName`
    + `AppLibCustomScriptDirName`
    + `AppLibResourceDirName`
    + `License`
    + `LicenseUrl`
- _Update App Libraries and Apps_ entry in the _Setup_ menu of the setup dialog of _BenchDashboard_
- Markdown info text in the app info dialog
- License link to the app info dialog
- Additional columns to the setup dialog
    + Category
    + Library
    + License

### Changed
- Upgrade process is using the _Bench CLI_ now
  ([#84](https://github.com/mastersign/bench/issues/84))
- Directory for custom scripts in the user app library
  was moved from `config\apps` to `config\scripts`
- Moved app definitions into their own Git repositories
    + <https://github.com/mastersign/bench-apps-core>
    + <https://github.com/mastersign/bench-apps-default>
- In the future, all app related issues are attended at the app library repositories on GitHub

### Fixed
- Proxy setup for Maven
  ([#89](https://github.com/mastersign/bench/issues/89))

### Removed
- Script based actions
- Embedded app library  
  **(the `AppLibs` property must be overriden in the user or site config now, to load more than the core app library)**
- Config properties
    + `ActionDir`
    + `AppIndexFile`
    + `CustomAppIndexFile`
    + `AppResourceBaseDir`

## [0.13.3] - 2016-11-19
[Full Changelog](https://github.com/mastersign/bench/compare/v0.13.2...v0.13.3)

### Added
- Support for second argument `verbose` in `bench-ctl`

### Changed
- Update: OpenSSL from 1.0.2h to 1.1.0c
- Update: FreeCAD 0.16 from build 6704 to 6706

### Fixed
- `bench-ctl update-env` was not working
- Broken RubyGems installer, because of changed certificate
  ([#86](https://github.com/mastersign/bench/issues/86))

## [0.13.2] - 2016-10-22
[Full Changelog](https://github.com/mastersign/bench/compare/v0.13.1...v0.13.2)

### Fixed
- Switched from expanded strings to simple strings when setting
  environment variables in registry.
  ([#82](https://github.com/mastersign/bench/issues/82))

## [0.13.1] - 2016-10-19
[Full Changelog](https://github.com/mastersign/bench/compare/v0.13.0...v0.13.1)

### Added
- Added Scribus
  ([#80](https://github.com/mastersign/bench/issues/80))
- Added .NET Core SDK
  ([#79](https://github.com/mastersign/bench/issues/79))
- Added CoffeeScript
  ([#78](https://github.com/mastersign/bench/issues/78))
- Added FreeCAD

### Changed
- Update: 7-Zip from 16.02 to 16.04
- Update: ConEmu from 16.07.24 to 16.10.09a
- Update: Git from 2.9.2 to 2.10.1
- Update: Sublime Text 3 from 3114 to 3126
- Update: JRE8 / JDK8 from u102 to u112
- Update: Node.js from 6.3.1 to 6.9.0
- Update: Atom from 1.9.8 to 1.11.2
- Update: Blender from 2.77a to 2.78

## [0.13.0] - 2016-08-16
[Full Changelog](https://github.com/mastersign/bench/compare/v0.12.1...v0.13.0)

### Added
- Configuration property `UseRegistryIsolation` to provide a way
  of suppressing the registry isolation when using Bench integrated
  in the user profile with `RegisterInUserProfile`
- Support for NuGet packages
  ([#75](https://github.com/mastersign/bench/issues/75))
- Erlang 19.0
  ([#41](https://github.com/mastersign/bench/issues/41))
- RabbitMQ 3.6.5
  ([#41](https://github.com/mastersign/bench/issues/41))
- NUnit 3 Test Runner (latest)
- Auto Setup button in BenchDashboard

### Changed
- Update: ConEmu from 16.06.19 to 16.07.24
- Update: Git from 2.9.0 to 2.9.2
- Update: cURL from 7.48.0 to 7.50.1
- Update: FileZilla from 3.18.0 to 3.20.1
- Update: Pandoc from 1.17.1 to 1.17.2
- Update: GraphicsMagick from 1.3.23 to 1.3.24
- Update: Python 2 from 2.7.11 to 2.7.12
- Update: Atom from 1.8.0 to 1.9.8
- Update: FFmpeg from 20160512 to _latest_
- Update: Gimp from 2.8.19 to 2.8.18
- Update: Ruby from 2.2.4 to 2.3.1
- Update: PHP 5 from 5.6.21 to 5.6.23
- Update: PHP 7 from 7.0.6 to 7.0.8
- Update: CMake from 3.5.2 to 3.6.1
- Update: Clang from 3.8.0 to 3.8.1
- Update: Eclipse from 4.5 mars to 4.6 neon
- Update: MySQL from 5.7.12 to 5.7.14
- Update: MySQL Workbench from 6.3.6 to 6.3.7
- Update: Apache HTTP Server from 2.4.20 to 2.4.23
- Introduced file system watcher to automatically update
  the configuration of Bench Dashboard in case a configuration
  file was changed.
  ([#66](https://github.com/mastersign/bench/issues/66))
- Improved _Auto Setup_ by adding upgrade capability
- Improved Bench upgrading by _not_ deleting all apps,
  but instead only deleting ConEmu and using the _Auto Setup_ to upgrade apps

### Fixed
- Tooltip for documentation button in BenchDashboard
- Incorrect syntax in `config.md` template
- Exception when clicking on header of icon column in the setup dialog
- Fixed disabled entries in app context menu in setup dialog

### Removed
- Removed option for editing the config file during the initialization
  ([#67](https://github.com/mastersign/bench/issues/67))

## [0.12.1] - 2016-07-29
[Full Changelog](https://github.com/mastersign/bench/compare/v0.12.0...v0.12.1)

### Fixed
- Environment setup fails if the user has no `PATH` environment variable
  ([#76](https://github.com/mastersign/bench/issues/76))

## [0.12.0] - 2016-07-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.11.4...v0.12.0)

### Added
- Support for custom scripts in `config\apps`
  ([#72](https://github.com/mastersign/bench/issues/72))
- Support for setting environment variables in Windows user profile
  ([#74](https://github.com/mastersign/bench/issues/74))

### Changed
- Changed app ID of 7-Zip from `SvZ` to `7z`
- Update: JDK/JRE 8 from rev. 92 to rev. 102
- Update: JabRef from 3.3 to 3.5
- Update: Node.js from 6.2.2 to 6.3.1

### Fixed
- Typo in `HTTP_PROXY` and `HTTPS_PROXY` in `env.cmd`
  ([#71](https://github.com/mastersign/bench/issues/71))
- Atom using the Windows user profile folder
  ([#73](https://github.com/mastersign/bench/issues/73))

## [0.11.4] - 2016-07-04
[Full Changelog](https://github.com/mastersign/bench/compare/v0.11.3...v0.11.4)

### Changed
- Update: Node.js from 4.4.6 to 6.2.2

### Fixed
- UI performance when activating/deactivating apps

## [0.11.3] - 2016-07-02
[Full Changelog](https://github.com/mastersign/bench/compare/v0.11.2...v0.11.3)

### Fixed
- Fixed execution policy for running `PsExecHost.ps1`

## [0.11.2] - 2016-06-30
[Full Changelog](https://github.com/mastersign/bench/compare/v0.11.1...v0.11.2)

### Changed
- Update: ConEmu from 16.03.13 to 16.06.19
- Update: Node.js from 4.4.5 to 4.4.6
- Improved execution speed of custom scripts via GUI

### Fixed
- Focus flicker of GUI app

## [0.11.1] - 2016-06-25
[Full Changelog](https://github.com/mastersign/bench/compare/v0.11.0...v0.11.1)

### Added
- Bench configuration property `Website`
- Link to the Bench Website
- Added Vim (graphical and console)
  ([#69](https://github.com/mastersign/bench/issues/69))
- Added Atom
  ([#65](https://github.com/mastersign/bench/issues/65))

### Changed
- The environment variables are not configured with the Bench environment
  during execution of custom scripts and hooks anymore

### Fixed
- Installing multiple apps into the same dir
  ([#70](https://github.com/mastersign/bench/issues/70))
- Custom setup script of Leiningen

## [0.11.0] - 2016-06-18
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.8...v0.11.0)

### Added
- Custom config dictionary property `Environment` to add environment variables
  in `config.md`
  ([#56](https://github.com/mastersign/bench/issues/56))
- Custom config list property `EnvironmentPath` to add paths
  to the `PATH` environment variable in `config.md`
  ([#56](https://github.com/mastersign/bench/issues/56))
- Entry for opening the apps website in the context menu of the setup dialogs app list
  ([#62](https://github.com/mastersign/bench/issues/62))
- App property label
  ([#63](https://github.com/mastersign/bench/issues/63))
- Support for documentation URLs
  ([#57](https://github.com/mastersign/bench/issues/57))
- Added VLC Player
- Added MeshLab
- Added Blender
- Added Hugo
- Added JabRef
- Added TeXnicCenter
- Group: LaTeX
- Group: 3D Modeling

### Changed
- Moved `auto\env.cmd` to the Bench root folder
  ([#59](https://github.com/mastersign/bench/issues/59))
- Simplified install process by mergin Bench Dashboard into main repo
- Update: Git from 2.8.3 to 2.9.0
- Update: FileZilla from 3.17.0.1 to 3.18.0
- Update: OpenSSL from 1.0.2d to 1.0.2h
- Update: 7-Zip from 16.00 to 16.02
- Update: Pandoc from 1.17.0.2 to 1.17.1
- Update: PHP 5 from 5.6.20 to 5.6.21
- Update: PHP 7 from 7.0.5 to 7.0.6
- Added VLC Player and Blender to group Multimedia

### Fixed
- Visible console window when starting adorned executables from main window
  ([#60](https://github.com/mastersign/bench/issues/60))
- Prevent setup dialog from getting minimized while it is busy
  ([#61](https://github.com/mastersign/bench/issues/61))

## [0.10.8] - 2016-05-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.7...v0.10.8)

### Fixed
- Mark Git as required, if an existing custom config needs to be cloned
  ([#55](https://github.com/mastersign/bench/issues/55))

## [0.10.7] - 2016-05-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.6...v0.10.7)

### Added
- Setup action for downloading all app resources
  ([#50](https://github.com/mastersign/bench/issues/50))
- Setup action for deleting obsolete app resources
  ([#51](https://github.com/mastersign/bench/issues/51))
- Added AntRenamer
  ([#47](https://github.com/mastersign/bench/issues/47))

### Changed
- Made Git an optional app
  ([#54](https://github.com/mastersign/bench/issues/54))
- Update: Git from 2.8.2 to 2.8.3
- Update: MikTeX from 2.9.5857 to 2.9.5987
  ([#52](https://github.com/mastersign/bench/issues/52))
- Update: NodeJS from 4.4.4 to 4.4.5

### Fixed
- Description of upgrade action in `bench-ctl.cmd`
  ([#49](https://github.com/mastersign/bench/issues/49))
- App IDs without quotes
  ([#53](https://github.com/mastersign/bench/issues/53))

## [0.10.6] - 2016-05-24
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.5...v0.10.6)

### Added
- Launcher for 7-Zip file manager
- Added Zeal Docs
  ([#46](https://github.com/mastersign/bench/issues/46))

### Changed
- Update: 7.Zip from 15.18 to 16.00
- Execution order of a custom post-execution script and registry restoration
  for adorned executables

### Fixed
- Initializing/Upgrading the Bench environment

## [0.10.5] - 2016-05-17
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.4...v0.10.5)

### Added
- Configuration properties to control the appearance of shell launchers
  ([#39](https://github.com/mastersign/bench/issues/39))
  + `QuickAccessCmd` defaults to `true`
  + `QuickAccessPowerShell` defaults to `false`
  + `QuickAccessBash` defaults to `false`
- Added NuGet

### Changed
- Do not convert the Bench root into a Git working copy during setup
  ([#44](https://github.com/mastersign/bench/issues/44))

## [0.10.4] - 2016-05-14
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.3...v0.10.4)

### Added
- Content tree in Markdown viewer

### Changed
- Dashboard window behavior (maximizable, no parents)
- Update: Sublime Text 3 from build 3103 to 3114
- Update: GnuPG from 2.0.29 to 2.0.30
- Update: cURL from 7.47.1 to 7.48.0
- Update: FileZilla from 3.15.0.2 to 3.17.0.1
- Update: Pandoc from 1.16.0.2 to 1.17.0.2
- Update: FFmpeg from 20160213 to 20160512
- Update: PHP 5 from 5.6.19 to 5.6.20
- Update: PHP 7 from 7.0.4 to 7.0.5
- Update: CMake from 3.4.3 to 3.5.2
- Update: Clang from 3.7.1 to 3.8.0
- Update: MySQL from 5.7.11 to 5.7.12
- Update: PostgreSQL from 9.5.0 to 9.5.3

### Fixed
- GitKraken Resource Url

## [0.10.3] - 2016-05-13
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.2...v0.10.3)

### Added
- Added GitKraken (latest)

### Fixed
- Path resolution of list properties
  ([#43](https://github.com/mastersign/bench/issues/43))
- MinGW MSYS path was not included in the environment
  ([#42](https://github.com/mastersign/bench/issues/42))

## [0.10.2] - 2016-05-09
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.1...v0.10.2)

### Added
- Support for Ruby Gems (app typ `ruby-package`)
  ([#36](https://github.com/mastersign/bench/issues/36))
- Added SASS (latest)

## [0.10.1] - 2016-05-08
[Full Changelog](https://github.com/mastersign/bench/compare/v0.10.0...v0.10.1)

### Fixed
- Installing NodeJS and Python packages via _Bench Dashboard_
- Leiningen profile setup
- Multiple typos in the app index
    + `DevClojure` dependency `LightTable`
    + `GnuPG` properties
    + `JRE8` properties
    + `Maven` properties

## [0.10.0] - 2016-05-07
[Full Changelog](https://github.com/mastersign/bench/compare/v0.9.3...v0.10.0)

For this release a clean install is required and the configuration
must be migrated to the new format.

### Added
- Graphical User Interface _Bench Dashboard_
- Automatic proxy configuration
- Global config property `ParallelDownloads`
- Support for global custom scripts `config\setup.ps1` and `config\env.ps1`

### Changed
- Moved logic from PowerShell scripts to .NET DLL `BenchLib.dll`
- Improved parsing performance for Markdown configuration files
- Improved format for list properties in Markdown configuration files
- More stable parsing of Markdown configuration files
    + ignore content of HTML comments
    + ignore content of code blocks
    + ignore content of YAML header
- Renamed app properties
    + `AppFile` &rarr; `ResourceName`
    + `AppArchive` &rarr; `ArchiveName`
    + `AppArchiveTyp` &rarr; `ArchiveTyp`
    + `AppArchiveSubDir` &rarr; `ArchivePath`
    + `NpmPackage` &rarr; `PackageName`
    + `PyPiPackage` &rarr; `PackageName`
- Moved default configuration from `auto\lib\config.lib.ps1` to `res\config.md`
- Moved custom configuration from `config\config.ps1` to `config\config.md`
- Moved app activation from `config\config.ps1` to `config\apps-activated.txt`
- Moved proxy and user infos from `config\config.ps1` to `bench-site.md`
- Update: JDK/JRE 8 from rev. 77 to rev. 92
- Update: Apache Web Server from 2.4.18 to 2.4.20
- Update: Git from 2.8.1 to 2.8.2
- Update: NodeJS from 4.4.3 to 4.4.4

### Fixed
- Spacemacs under path with whitespaces

## [0.9.3] - 2016-04-13
[Full Changelog](https://github.com/mastersign/bench/compare/v0.9.2...v0.9.3)

### Added
- App: Maven
- App: GnuPG

### Changed
- Update: Git from 2.7.1 to 2.8.1
- Update: JDK 8 from rev. 74 to rev. 77
- Update: JRE 8 from rev. 74 to rev. 77
- Update: PHP 5 from 5.6.17 to 5.6.19
- Update: PHP 7 from 7.0.2 to 7.0.4
- Update: NodeJS from 4.3.1 to 4.4.3
- Added Maven to group DevJava and DevClojure
- Added GnuPG as dependency to Maven and Leiningen
- Set environment variable `JAVA_CMD` by JRE7/8 and JDK7/8 (last wins)

### Fixed
- Leiningen stores local repo in Windows user profile
  ([#34](https://github.com/mastersign/bench/issues/34))

## [0.9.2] 2016-03-07
[Full Changelog](https://github.com/mastersign/bench/compare/v0.9.1...v0.9.2)

### Added
- App: JRE 7

### Changed
- give JDKs a higher priority in the PATH than the JRE

### Fixed
- MSYS tools from MinGW are not in PATH

## [0.9.1] - 2016-03-03
[Full Changelog](https://github.com/mastersign/bench/compare/v0.9.0...v0.9.1)

### Added
- support for Windows 7 with PowerShell 2
  ([#29](https://github.com/mastersign/bench/issues/29))
- App: JRE 8
  ([#30](https://github.com/mastersign/bench/issues/30))

### Changed
- deal with cyclic dependencies between apps
- renamed bootstrap file `bench.bat` into `bench-install.bat`
- allways run Eclipse with JRE 8
  ([#30](https://github.com/mastersign/bench/issues/30))

### Removed
- progress during downloads, replaced download  with `Invoke-WebRequest`
  by silent download via `System.Net.WebClient` because of compatibility with PowerShell 2
  ([#29](https://github.com/mastersign/bench/issues/29))
- config property `DownloadProgress`

### Fixed
- broken bootstrap file `bench.bat`
  ([#31](https://github.com/mastersign/bench/issues/31))

## [0.9.0] - 2016-03-02
[Full Changelog](https://github.com/mastersign/bench/compare/v0.8.0...v0.9.0)

### Added
- support for unpacking `*.tar.*` archives with two steps
- app property `SetupTestFile` to explicitly specify the file
- config property `DownloadProgress` to control progress info during app resource downloads
  which is used to determine if an app is installed or not
- App: Dia
- App: Gimp
  ([#26](https://github.com/mastersign/bench/issues/26))
- App: FileZilla
  ([#27](https://github.com/mastersign/bench/issues/28))
- App: WinMerge
- App: Wget
- App: MinGW
  ([#28](https://github.com/mastersign/bench/issues/28))
- App: CMake
- App: LLVM Clang
- App: Go
- App: Eclipse PHP
- App: Eclipse C/C++
  ([#28](https://github.com/mastersign/bench/issues/28))
- App: SysInternals Suite
  ([#25](https://github.com/mastersign/bench/issues/25))
- App Group: `Multimedia`
- App Group: `DevCpp`

### Changed
- do not add default executable to explicitly specified list of adorned executables
- decode filename from URLs during download
- Update: NodeJS from 4.3.0 to 4.3.1

### Fixed
- adornment proxies are generated with incorrect relative path
- execution of Git post-install script
- cancel downloads if one download failes

## [0.8.0] - 2016-02-15
[Full Changelog](https://github.com/mastersign/bench/compare/v0.7.2...v0.8.0)

### Added
- `auto\lib\profile.ps1` for customization of the PowerShell environment

### Changed
- moved action scripts from root directory to folder `actions`
- renamed action `project-shell.cmd` into `project-ps.cmd`
- moved custom app index `apps.md` and custom configuration `config.ps1` to `config` folder
- set color of debug messages to `DarkCyan`
- Install Leiningen only if the `leiningen.jar` was not found
- Switched from PHP release URLs to archive URLs for longer lifespan
    + Downgrade: PHP 5 from 5.6.18 to 5.6.17 (archive link)
    + Downgrade: PHP 7 from 7.0.3 to 7.0.2 (archive link)
- Update: Git from 2.7.0 to 2.7.1
- Update: cURL from 7.46.0 to 7.47.1
- Update: Sift from 0.7.1 to 0.8.0
- Update: FFmpeg from 20160124 to 20160213
- Update: NodeJS from 4.2.6 to 4.3.0
- Update: Ruby from 2.2.3 to 2.2.4
- Update: JDK 8 from rev. 72 to rev. 74
- Update: MySQL Server from 5.7.10 to 5.7.11

### Fixed
- action `Archive-Project` did not work after switching from `7za.exe` to `7z.exe`
- expansion of config values `AppAdornmentBaseDir` and `AppRegistryBaseDir`

## [0.7.2] - 2016-02-12
[Full Changelog](https://github.com/mastersign/bench/compare/v0.7.1...v0.7.2)

### Added
- Passing arguments to _Command Line_ launcher with `/C`, to allow
  dropping executables on the launcher, to run them in the _Bench_ environment

### Changed
- Update: PHP 5 from 5.6.17 to 5.6.18
- Update: PHP 7 from 7.0.2 to 7.0.3
- Update: Visual Studio Code Archive Pattern

## [0.7.1] - 2016-02-12
[Full Changelog](https://github.com/mastersign/bench/compare/v0.7.0...v0.7.1)

### Fixed
- FFmpeg exe path

## [0.7.0] - 2016-02-12
[Full Changelog](https://github.com/mastersign/bench/compare/v0.6.1...v0.7.0)

### Added
- `CHANGELOG.md`
- Action script `bench-ctl.cmd` for management of the _Bench_ installation
- Launcher shortcut for `bench-ctl.cmd`
- Support for execution adornment
  ([#17](https://github.com/mastersign/bench/issues/17))
- Support for registry isolation
  ([#18](https://github.com/mastersign/bench/issues/18))
- Support for automatic dependencies
  ([#20](https://github.com/mastersign/bench/issues/20))
    + `node-package` apps automatically depend on `Npm`
    + `python2-package` apps automatically depend on `Python2`
    + `python3-package` apps automatically depend on `Python3`
- App: Putty
  ([#21](https://github.com/mastersign/bench/issues/21))
- App: PostgreSQL and pgAdminIII

### Changes
- changed behavior for `App-Dir`, `App-Path`, and `App-Paths` for `meta` apps
  ([#22](https://github.com/mastersign/bench/issues/22))
- migrated app type `python-package` to `python2-package` and `python3-package`
  ([#19](https://github.com/mastersign/bench/issues/19))
- Update: Sublime Text 3 from rev. 3083 to rev. 3103

### Removed
- Management scripts (replaced by `bench-ctl.cmd`)
    + `auto\update-env.cmd`
    + `auto\bench-download.cmd`
    + `auto\bench-setup.cmd`
    + `auto\bench-refresh.cmd`
    + `auto\bench-upgrade.cmd`

## [0.6.1] - 2016-02-07
[Full Changelog](https://github.com/mastersign/bench/compare/v0.6.0...v0.6.1)

### Added
- Automatic update of PIP to most recent version after installing Python 2 and Python 3
  ([#16](https://github.com/mastersign/bench/issues/16))

### Fixed
- Launcher icon for `bench-cmd.cmd`
- Silently failing NPM update after installing `NpmBootstrap`
  ([#15](https://github.com/mastersign/bench/issues/15))

## [0.6.0] - 2016-01-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.5.4...v0.6.0)

### Added
- actions scripts for _CMD_, _PowerShell_, and _Bash_
- launcher shortcuts for command line action scripts

### Changed
- action script `bench-ps.cmd` passes arguments to `powershell.exe`
- updated documentation of overridden environment variables

## [0.5.4] - 2016-01-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.5.3...v0.5.4)

### Change
- Removed home directory from Git version control to prevent data loss
  during updates of _Bench_
- Moved _Visual Studio Code_ user configuration from home directory
  to `res\apps\vscode`

## [0.5.3] - 2016-01-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.5.2...v0.5.3)

### Fixed
- Building relative paths in `env.cmd`

## [0.5.2] - 2016-01-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.5.2...v0.5.2)

### Fixed
- Incorrect quotes for path strings in `config.template.ps1`

## [0.5.1] - 2016-01-27
[Full Changelog](https://github.com/mastersign/bench/compare/v0.5.0...v0.5.1)

### Changed
- Moved Spacemacs config file from `%HOME%\.spacemacs` to `%HOME%\.spacemacs.d\init.el`
- Changed target path of Emacs launcher from `emacs.exe` to `runemacs.exe`
  to avoid console window
- Update: Pandoc from 1.15.1.1 to 1.16.0.2
- Update: Python 3 from 3.4.3 to 3.4.4
- Update: MikTeX from 2.9.5719 to 2.9.5857
- Update: NodeJS from 4.2.3 to 4.2.6
- Update: FFmpeg from 20160105 to 20160124
- Update: Git from 2.6.4 to 2.7.0
- Update: JDK 8 from rev. 66 to  rev. 72
- Update: LightTable from 0.8.0 to 0.8.1

## [0.5.0] - 2016-01-22
[Full Changelog](https://github.com/mastersign/bench/compare/v0.4.0...v0.5.0)

### Added
- Support for launcher shortcuts
  ([#8](https://github.com/mastersign/bench/issues/8))
- Config Value: `BenchDrive`
- Config Value: `BenchAuto`
- Config Value: `BenchScripts`

### Changed
- Split IPython into to seperate packages for Python 2 and Python 3
- Template for Spacemacs config file
    + added more proposed layers
    + maximize on startup
    + disable HTTPS to Elpa repositories by default to deal with proxies

### Fixed
- Do not empty the temp directory while loading `env.lib.ps1`

## [0.4.0] - 2016-01-20
[Full Changelog](https://github.com/mastersign/bench/compare/v0.3.4...v0.4.0)

### Added
- App: IPython (in app group `DevPython2` and `DevPython3`)
  ([#3](https://github.com/mastersign/bench/issues/3))
- Support for app type `python-package`
  ([#10](https://github.com/mastersign/bench/issues/10))
- Wrapper `python2.cmd` for `python.exe` version 2

### Changed
- Restricted download of app resources to apps of type `default`

## [0.3.4] - 2016-01-20
[Full Changelog](https://github.com/mastersign/bench/compare/v0.3.3...v0.3.4)

### Added
- App: GnuTLS
- App: Emacs
- App: Spacemacs
  ([#4](https://github.com/mastersign/bench/issues/4))

### Fixed
- Custom environment variables and setup scripts for meta apps

## [0.3.3] - 2016-01-18
[Full Changelog](https://github.com/mastersign/bench/compare/v0.3.2...v0.3.3)

### Added
- Support for NPM version ranges

### Changed
- Resolve relative paths with `..` in `env.cmd`
  before setting environment variables

### Fixed
- Incorrect version patterns for NPM packages

## [0.3.2] - 2016-01-11
[Full Changelog](https://github.com/mastersign/bench/compare/v0.3.1...v0.3.2)

### Added
- Override `HOME` in addition to `USERPROFILE`
- Config value `OverrideHome` to make overriding environment variables optional
  ([#5](https://github.com/mastersign/bench/issues/5))
    + `HOME`
    + `HOMEDRIVE`
    + `HOMEPATH`
    + `USERPROFILE`
    + `APPDATA`
    + `LOCALAPPDATA`
- Config value `OverrideTemp` to make overriding environment variables optional
  ([#1](https://github.com/mastersign/bench/issues/1))
    + `TEMP`
    + `TMP`
- Config value `IgnoreSystemPath` to make clean-up of `PATH` optional
- More debug messages
- App: LightTable
  ([#2](https://github.com/mastersign/bench/issues/2))

### Fixed
- Interpretation of boolean app properties
- Incorrect warnings for meta apps during environment update

## [0.3.1] - 2016-01-08
[Full Changelog](https://github.com/mastersign/bench/compare/v0.3.3...v0.3.1)

### Added
- Documentation for groups in `apps.md`

## [0.3.0] - 2016-01-08
[Full Changelog](https://github.com/mastersign/bench/compare/v0.2.2...v0.3.0)

### Added
- Support for app typ `meta` for custom apps and app groups
- Support for dependency based app selection
- App Group: `WebDevPHP5`
- App Group: `WebDevPHP7`
- App Group: `DevPython2`
- App Group: `DevPython3`
- App Group: `DevJava`
- App Group: `DevClojure`
- App Group: `Markdown`
- Automatic setting of `core.autocrlf` in the Git config

### Changed
- Moved all default apps to optional apps
- Update: Eclipse from 4.4.2 to 4.5

## [0.2.2] - 2016-01-07
[Full Changelog](https://github.com/mastersign/bench/compare/v0.2.1...v0.2.2)

### Added
- App: Apache Web Server
- App: PHP 5
- App: PHP 7
- App: Ruby
- App: Leiningen
- App: FFmpeg
- Support for app specific environment variables

### Changed
- Deliver absolute paths for placeholder expansion in app properties

### Fixed
- Switched `lib\git\bin` to `lib\git\cmd` in `PATH`

## [0.2.1] - 2016-01-06
[Full Changelog](https://github.com/mastersign/bench/compare/v0.2.0...v0.2.1)

### Added
- App: Sift
- App: OpenSSL
- App: GrahpicsMagick
- App: cURL
- App: Inno Setup unpacker
- Support for archive type `inno`

### Changed
- Management scripts passing additional arguments

### Fixed
- SourceForge URL recognition
- File name extraction from download URL

## [0.2.0] - 2016-01-06
[Full Changelog](https://github.com/mastersign/bench/compare/v0.1.0...v0.2.0)

### Added
- App: Oracel JDK 7 and 8
- App: Eclipse
- App: MySQL Server
- App: MySQL Workbench
- Environment Var: `BENCH_APPS` with the root path of the installed apps
- Config Value: `BenchRoot`
- Management Script: `auto\bench-download.cmd`
- Support for custom cookies and headers for app resource downloads
- Support for downloading from SourceForge and Eclipse mirrors

### Changed
- Switched from limited 7za to full 7z
- Inverted order of apps in `PATH`
- Renamed app property `File`/`Download` into `AppFile`
- Renamed app property `Archive` into `AppArchive`

### Fixed
- location for `AppDataDir`
- incorrect proxy template in `res\config.template.ps1`

## [0.1.0] - 2016-01-04

First public release on GitHub.

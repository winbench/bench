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
[App Changes](https://github.com/mastersign/bench/compare/master...apps)

### Added
- `CHANGELOG.md`
- Action script `bench-ctl.cmd` for management of the _Bench_ installation
- Launcher shortcut for `bench-ctl.cmd`
- Support for execution adornment
  ([#17](https://github.com/mastersign/bench/issues/17))
- App: PostgreSQL and pgAdminIII

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

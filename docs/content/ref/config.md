+++
date = "2016-06-22T13:42:49+02:00"
description = "The properties of the Bench configuration"
draft = true
title = "Configuration Properties"
weight = 6
+++

TODO

## Fix Distribution Properties

Properties in this group can not be customized.

* VersionFile: `res\version.txt`
* CustomConfigDir: `config`
* CustomConfigFile: `$CustomConfigDir$\config.md`
* CustomConfigTemplateFile: `res\config.template.md`
* SiteConfigFileName: `bench-site.md`
* SiteConfigTemplateFile: `res\bench-site.template.md`
* AppIndexFile: `res\apps.md`
* AppActivationFile: `$CustomConfigDir$\apps-activated.txt`
* AppActivationTemplateFile: `res\apps-activated.template.txt`
* AppDeactivationFile: `$CustomConfigDir$\apps-deactivated.txt`
* AppDeactivationTemplateFile: `res\apps-deactivated.template.txt`
* CustomAppIndexFile: `$CustomConfigDir$\apps.md`
* CustomAppIndexTemplateFile: `res\apps.template.md`
* ConEmuConfigFile: `$CustomConfigDir$\ConEmu.xml`
* ConEmuConfigTemplateFile: `res\ConEmu.template.xml`
* AppResourceBaseDir: `res\apps`
* ActionDir: `actions`
* LibDir: `lib`

## Customizable Properties

Properties in this group are customizable and can be set in
`config/config.md` or in a `bench-site.md` file.

* UseProxy: `false`
* ProxyBypass: `localhost`
* HttpProxy: `http://127.0.0.1:80`
* HttpsProxy: `http://127.0.0.1:443`
* DownloadAttempts: 3
* ParallelDownloads: 4
* LogLevel: `Info`
* UserName: user
* UserEmail: user@localhost
* DownloadDir: `cache`
* AppAdornmentBaseDir: `$LibDir$\_proxies`
* AppRegistryBaseDir: `$HomeDir$\registry_isolation`
* TempDir: `tmp`
* LogDir: `log`
* HomeDir: `home`
* AppDataDir: `$HomeDir$\AppData\Roaming`
* LocalAppDataDir: `$HomeDir$\AppData\Local`
* OverrideHome: `true`
* OverrideTemp: `true`
* IgnoreSystemPath: `true`
* ProjectRootDir: `projects`
* ProjectArchiveDir: `archive`
* ProjectArchiveFormat: `zip`
* LauncherDir: `launcher`
* LauncherScriptDir: `$LibDir$\_launcher`
* WizzardEditCustomConfigBeforeSetup: `false`
* WizzardStartAutoSetup: `true`
* QuickAccessCmd: `true`
* QuickAccessPowerShell: `false`
* QuickAccessBash: `false`
* EditorApp: `VSCode`
* Website: <http://mastersign.github.io/bench>

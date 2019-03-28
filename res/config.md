# Default Configuration

* VersionFile: `res\version.txt`
* Website: <https://winbench.org>
* VersionUrl: <https://github.com/winbench/bench/raw/master/res/version.txt>
* UpdateUrlTemplate: `https://github.com/winbench/bench/releases/download/v#VERSION#/Bench.zip`
* BootstrapUrlTemplate: `https://github.com/winbench/bench/raw/v#VERSION#/res/bench-install.bat`
* AutoUpdateCheck: `true`
* UseProxy: `false`
* ProxyBypass: `localhost`
* HttpProxy: `http://127.0.0.1:80`
* HttpsProxy: `http://127.0.0.1:443`
* DownloadAttempts: 3
* ParallelDownloads: 4
* HttpsSecurityProtocols:
    + `Tls`
    + `Tls11`
    + `Tls12`
* LogLevel: `Info`
* UserName: user
* UserEmail: user@localhost
* UserConfigTemplateFile: `res\config.template.md`
* SiteConfigTemplateFile: `res\bench-site.template.md`
* UserAppIndexTemplateFile: `res\apps.template.md`
* AppActivationTemplateFile: `res\apps-activated.template.txt`
* AppDeactivationTemplateFile: `res\apps-deactivated.template.txt`
* ConEmuConfigTemplateFile: `res\ConEmu.template.xml`
* LibDir: `lib`
* AppLibsInstallDir: `$LibDir$\applibs`
* AppsInstallDir: `$LibDir$\apps`
* AppsVersionIndexDir: `$LibDir$\versions`
* AppsAdornmentBaseDir: `$LibDir$\proxies`
* LauncherScriptDir: `$LibDir$\launcher`
* UserConfigDir: `config`
* UserConfigFile: `$UserConfigDir$\config.md`
* AppActivationFile: `$UserConfigDir$\apps-activated.txt`
* AppDeactivationFile: `$UserConfigDir$\apps-deactivated.txt`
* ConEmuConfigFile: `$UserConfigDir$\ConEmu.xml`
* CacheDir: `cache`
* AppsCacheDir: `$CacheDir$\apps`
* AppLibsCacheDir: `$CacheDir$\applibs`
* TempDir: `tmp`
* AppLibs:
    + `core`: `github:winbench/apps-core`
    + `default`: `github:winbench/apps-default`
* SiteConfigFileName: `bench-site.md`
* AppLibIndexFileName: `apps.md`
* AppLibCustomScriptDirName: `scripts`
* AppLibResourceDirName: `res`
* KnownLicenses:
    + `AFL-1.1`: <https://spdx.org/licenses/AFL-1.1.html>
    + `AFL-1.2`: <https://spdx.org/licenses/AFL-1.2.html>
    + `AFL-2.0`: <https://spdx.org/licenses/AFL-2.0.html>
    + `AFL-2.1`: <https://spdx.org/licenses/AFL-2.1.html>
    + `AFL-3.0`: <https://spdx.org/licenses/AFL-3.0.html>
    + `Artistic-1.0`: <https://spdx.org/licenses/Artistic-1.0.html>
    + `Artistic-2.0`: <https://spdx.org/licenses/Artistic-2.0.html>
    + `Apache-1.1`: <https://spdx.org/licenses/Apache-1.1.html>
    + `Apache-2.0`: <https://spdx.org/licenses/Apache-2.0.html>
    + `BSD-2-Clause`: <https://spdx.org/licenses/BSD-2-Clause.html>
    + `BSD-3-Clause`: <https://spdx.org/licenses/BSD-3-Clause.html>
    + `CCDL-1.0`: <https://spdx.org/licenses/CDDL-1.0.html>
    + `CPL-1.0`: <https://spdx.org/licenses/CPL-1.0.html>
    + `CPAL-1.0`: <https://spdx.org/licenses/CPAL-1.0.html>
    + `EPL-1.0`: <https://spdx.org/licenses/EPL-1.0.html>
    + `GPL-2.0`: <https://spdx.org/licenses/GPL-2.0.html>
    + `GPL-3.0`: <https://spdx.org/licenses/GPL-3.0.html>
    + `AGPL-3.0`: <https://spdx.org/licenses/AGPL-3.0.html>
    + `IPL-1.0`: <https://spdx.org/licenses/IPL-1.0.html>
    + `Intel`: <https://spdx.org/licenses/Intel.html>
    + `LGPL-2.0`: <https://spdx.org/licenses/LGPL-2.0.html>
    + `LGPL-2.1`: <https://spdx.org/licenses/LGPL-2.1.html>
    + `LGPL-3.0`: <https://spdx.org/licenses/LGPL-3.0.html>
    + `MIT`: <https://spdx.org/licenses/MIT.html>
    + `MPL-1.0`: <https://spdx.org/licenses/MPL-1.0.html>
    + `MPL-1.1`: <https://spdx.org/licenses/MPL-1.1.html>
    + `MPL-2.0`: <https://spdx.org/licenses/MPL-2.0.html>
    + `MS-PL`: <https://spdx.org/licenses/MS-PL.html>
    + `MS-RL`: <https://spdx.org/licenses/MS-RL.html>
    + `OSL-1.0`: <https://spdx.org/licenses/OSL-1.0.html>
    + `OSL-2.1`: <https://spdx.org/licenses/OSL-2.1.html>
    + `OSL-3.0`: <https://spdx.org/licenses/OSL-3.0.html>
* LogDir: `log`
* LauncherDir: `launcher`
* HomeDir: `home`
* AppDataDir: `$HomeDir$\AppData\Roaming`
* LocalAppDataDir: `$HomeDir$\AppData\Local`
* AppsRegistryBaseDir: `$HomeDir$\registry_isolation`
* OverrideHome: `true`
* OverrideTemp: `true`
* IgnoreSystemPath: `true`
* RegisterInUserProfile: `false`
* UseRegistryIsolation: `true`
* Allow64Bit: `true`
* ProjectRootDir: `projects`
* ProjectArchiveDir: `archive`
* ProjectArchiveFormat: `zip`
* WizzardStartAutoSetup: `true`
* WizzardApps:
    + Web: `Bench.Group.WebDevelopment`
    + JavaScript: `Bench.Group.JavaScriptDevelopment`
    + Java: `Bench.Group.JavaDevelopment`
    + Clojure: `Bench.Group.ClojureDevelopment`
    + PHP: `Bench.Group.PHPDevelopment`
    + Python 2: `Bench.Group.Python2Development`
    + Python 3: `Bench.Group.Python3Development`
    + Ruby: `Bench.Group.RubyDevelopment`
    + C/C++: `Bench.Group.CppDevelopment`
    + LaTeX: `Bench.Group.LaTeXWriting`
* QuickAccessCmd: `true`
* QuickAccessPowerShell: `false`
* QuickAccessBash: `false`
* TextEditorApp: `Bench.BenchNpp`
* DashboardSetupAppListColumns: `Order`, `Label`, `Version`, `Active`, `Deactivated`, `Status`, `Typ`, `Comment`
* DashboardSavePositions: `true`

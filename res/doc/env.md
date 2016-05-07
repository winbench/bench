## Isolated Environment

The setup process of _Bench_ generates a file which is called `auto/env.cmd`.
This CMD script initializes the execution environment for programs to run inside of _Bench_.

The following environment variables are set by `auto/env.cmd`.

* `BENCH_HOME` is set to the root path of _Bench_ (e.g. `D:\bench`)
* `BENCH_APPS` is set to the root path of the _Bench_ apps, which is `%BENCH_HOME%\lib` per default
* `BENCH_PATH` is the list with the paths to the registered apps in _Bench_
* `BENCH_DRIVE` is set to the drive letter `%BENCH_HOME%` (e.g. `D:`)  
* `USERNAME` is set to the config value `UserName` from `config.ps1`
* `USEREMAIL` is set to the config value `UserEmail`from `config.ps1`
* `USERPROFILE` and `HOME` are set to `HomeDir`, which is `%BENCH_HOME%\home` per default
* `HOMEDRIVE` is set to the drive letter of `%HOME%` (e.g. `D:`)
* `HOMEPATH` is set to the path  without the drive letter of `%HOME%` (e.g. `\bench\home`)
* `APPDATA` is set to `%HOME%\AppData\Roaming`
* `LOCALAPPDATA` is set to `%HOME%\AppData\Local`
* `TEMP` and `TMP` are set to the path of the config value `TempDir`, which is `%BENCH_HOME%\tmp` per default
* `PATH` is set to `%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0`

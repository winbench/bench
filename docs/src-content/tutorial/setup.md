+++
date = "2018-02-09T12:00:00+02:00"
description = "Install Bench from scratch and initialize the configuration"
title = "Setting-Up Bench"
weight = 1
+++

[bench-install]: https://github.com/winbench/bench/raw/master/res/bench-install.bat
[bench-cli-transfer-install]: /ref/bench-cli/#cmd_bench-transfer-install
[config]: /ref/config
[select-apps]: /tutorial/apps
[Bench CLI]: /ref/bench-cli
[.NET45]: https://www.microsoft.com/download/details.aspx?id=30653

Setting up Bench means downloading and extracting the Bench system files,
initializing the configuration, and setting up the required apps for Bench.
The setup process is supported by a graphical initialization wizard.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

A shortened version of this tutorial can be found in the
[Quickstart](/start/install).

## Prerequisites {#prerequisites}
The following conditions must be fulfilled to use Bench.

* Microsoft Windows 7 or higher
* [Microsoft .NET 4.5][.NET45]
* Internet access for HTTP/HTTPS (possibly via proxy, but without credentials)
* A hard drive or a removable drive with at least 2 GB free memory  
  (How much space Bench requires, depends on the apps you select.)
* The Windows user account does _not_ need admin privileges

## Getting Bench {#download}
There are three ways to bootstrap the Bench setup process:

* Using the
  <a class="setup-download-link"
     href="https://github.com/winbench/bench/releases/latest">
    `BenchSetup.exe`
  </a>
  program,
* Using the [`bench-install.bat`][bench-install] script,
* Download and extract
  <a class="archive-download-link"
     href="https://github.com/winbench/bench/releases/latest">
    `Bench.zip`
  </a>
  manually, and kick-off the setup on the command line.

If you are not sure which way to go, use `BenchSetup.exe`.

### Bench Setup Program {#setup-program}
The preferred way to bootstrap the Bench setup, is to use the
<a class="setup-download-link"
   href="https://github.com/winbench/bench/releases/latest">
  `BenchSetup.exe`
</a>.

* Create a folder for Bench
* Download
  <a class="setup-download-link"
     href="https://github.com/winbench/bench/releases/latest">
    `BenchSetup.exe`
  </a>
  and save it anywhere you like
* The most current browsers flag downloaded executable files,
  to inform Windows about the potentially untrusted origin of the downloaded file.
  Therefore, to allow the execution of the `BenchSetup.exe`,
  you have to open the property dialog for the downloaded file
  in the Windows Explorer and approve of the execution of the file.
* Run `BenchSetup.exe`
* Choose your created folder as the root directory for the new Bench environment
* Follow the [Initialization Wizard](#setup-wizard)

If you run `BenchSetup.exe` on the command line, you can provide arguments,
which will be passed to [`bench.exe transfer install`][bench-cli-transfer-install]
after the extraction.

### Batch Script {#batch-script}
The predecessor of the setup program and still an alternative is the
[`bench-install.bat`][bench-install].

* Create a folder for Bench
* Download [`bench-install.bat`][bench-install]
  and save it inside the Bench folder
* The most current browsers flag downloaded executable files,
  to inform Windows about the potentially untrusted origin of the downloaded file.
  Mozilla Firefox even adds the `.txt` filename extension to the batch file.
  Therefore, to allow the execution of the `bench-install.bat`,
  you have to do two things:
    + Remove the `.txt` extension, if it was added.
    + Open the property dialog for the downloaded file in the Windows Explorer
      and approve of the execution of the file.
* Run `bench-install.bat`
* Follow the [Initialization Wizard](#setup-wizard)

### Manual Download {#manual}
If you want to have full control over the setup process, you can always
bootstrap the Bench setup process manually by downloading the `Bench.zip`
and running the `initialize` action via the [Bench CLI][].

* Download `Bench.zip` from
  <https://github.com/winbench/bench/releases/latest>
* Create a folder for Bench
* Extract the content of `Bench.zip` into the created folder
* Optionally prepare the Bench environment with some customized files like
    + `bench-site.md`
    + `config/config.md`
    + `config/app-activated.md`
* Run `auto\bin\bench.exe -v initialize`
* Follow the [Initialization Wizard](#setup-wizard)

## Initializing the Configuration {#setup-wizard}
The setup of the Bench environment is guided by a graphical wizard.

### Site Configuration
There are two groups of settings, which are usually set in the
[site configuration][config].
Therefore, if the setup wizard can not find a `bench-site.md`
in the Bench folder or its parents, it shows the following pages:

* Proxy  
  _Optionally set a HTTP(S) proxy address._
* User Identification  
  _Set the user name and email, which are set as environment variables
  and used e.g. in the Git configuration._

### User Configuration
As next step the configuration folder is initialized.
Therefore, if the setup wizard can not find the `config\config.md` file,
it shows a page for the initialization of the configuration.
There are two possibilities to initialize the configuration folder
with the initialization wizard:

* Initialize a standard configuration
* Checkout an existing configuration from a Git URL

If the standard configuration was chosen, some basic options can be set
on the next pages, before the setup process starts.

* The isolation settings allow to choose between a fully isolated
  Bench environment and a more comfortable integration into
  the Windows user profile.
* The app/group selection allows to preselect some apps or app groups,
  respectively, which will be setup right away.

### Advanced Settings
The last page of the setup wizard shows some additional settings
for the setup process.
Currently you can decide if the Bench Dashboard is started immediately
with the setup action after setting up Bench.

## Setting Up Required Apps
After stepping through the initialization wizard, the required apps for
the Bench system are downloaded and installed.
This initial app setup is performed in a standard Windows console window.

When finished installing the required apps, the [Bench Dashboard](/ref/dashboard)
is started.
In case it was activated in the initialization wizard, the setup window of the
Bench Dashboard is opened immediately, and all additionally activated apps
from the configuration are installed.

<script type="application/javascript">GetLatestReleaseInfo();</script>

## Next
Now you can [select and install your desired apps][select-apps].

## See Also

Tech Guides

* [Bench Setup and Upgrade](/guide/setup)
* [Isolation and Portability](/guide/isolation)

Reference Docs

* [Bench CLI](/ref/bench-cli)
* [File Structure](/ref/file-structure)
* [Configuration Markup Syntax](/ref/markup-syntax)
* [Configuration Properties](/ref/config)

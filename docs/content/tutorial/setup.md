+++
date = "2016-06-21T16:34:24+02:00"
description = "Install Bench from scratch and initialize the configuration"
title = "Setting-Up Bench"
weight = 1
+++

[bench-install]: https://github.com/mastersign/bench/raw/master/res/bench-install.bat
[config]: /ref/config
[select-apps]: /tutorial/apps
[Bench CLI]: /ref/bench-ctl
[.NET45]: https://www.microsoft.com/download/details.aspx?id=30653

Setting up Bench means downloading and extracting the Bench system files,
initializing the configuration, and setting up the required apps for Bench.
The setup process is supported by the bootstrap file `bench-install.bat`
and a graphical initialization wizzard.
<!--more-->

**Overview**

* [Prerequisites](#prerequisites)
* [Getting Bench](#download)
* [Initializing the Configuration](#setup-wizzard)
* [Setting Up Required Apps](#setting-up-required-apps)
* [Next](#next)
* [See Also](#see-also)

A shortened version of this tutorial can be found in the
[Quickstart](/start/install).

## Prerequisites {#prerequisites}
The following conditions must be fullfilled to use Bench.

* Microsoft Windows 7 or higher ([Microsoft .NET 4.5][.NET45] for GUI)
* Internet access for HTTP/HTTPS (possibly via proxy, but without credentials)
* A harddrive or a removable drive with at least 2 GB free memory  
  (How much space Bench requires, depends on the apps you select.)
* The Windows user account does _not_ need admin priviledges

## Getting Bench {#download}
There are two ways to bootstrap the Bench setup process:

* Use the `bench-install.bat` script,
  which is more easy in most of the cases but can fail in some situations
* Download and extract Bench manually,
  which is a bit more cumbersome, but can solve issues
  with the `bench-install.bat`

### Easy Download {#guided}
The preferred way to bootstrap the Bench setup, is to use the
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
      and approve the execution of the file.
* Run `bench-install.bat`
* Follow the [Initialization Wizzard](#setup-wizzard)

### Manual Download {#manual}
If the `bench-install.bat` failes to download the `Bench.zip`,
or failes in another way, you can allways bootstrap the Bench setup process
manually by downloading the `Bench.zip` and running the `initialize` action
via the [Bench CLI][].

* Download `Bench.zip` from
  <https://github.com/mastersign/bench/releases/latest>
* Create a folder for Bench
* Extract the content of `Bench.zip` into the created folder
* Run `actions\bench-ctl.cmd initialize`
* Follow the [Initialization Wizzard](#setup-wizzard)

## Initializing the Configuration {#setup-wizzard}
The setup of the Bench environment is guided by a graphical wizzard.

### Site Configuration
There are two groups of settings, which are usually set in the
[site configuration][config].
Therefore, if the setup wizzard can not find a `bench-site.md`
in the Bench folder or its parents, it shows the following pages:

* Proxy  
  _Optionally set a HTTP(S) proxy address._
* User Identification  
  _Set the username and email, which are set as environment variables
  and used e.g. in the Git configuration._

### Custom Configuration
As next step the configuration folder is initialized.
Therefore, if the setup wizzards can not find the `config\config.md` file,
it shows a page for the initialization of the configuration.
There are two possibilities to initialize the configuration folder
with the initialization wizzard:

* Initialize a standard configuration
* Checkout an existing configuration from a Git URL

### Advanced Settings
The last page of the setup wizzard shows some additional settings
for the setup process.
Currently you can decide if the Bench Dashboard is started immediatly
with the setup action after setting up Bench.

## Setting Up Required Apps
After stepping through the initialization wizzard, the required apps for
the Bench system are downloaded and installed.
This initial app setup is performed in a standard Windows console window.

When finished installing the required apps, the [Bench Dashboard](/ref/dashboard)
is started.
Incase it was activated in the initialization wizzard, the setup window of the
Bench Dashboard is opened immediately, and all additionally activated apps
from the configuration are installed.
This is escpecially handy, if you are re-using an existing configuration
with a number of activated apps.

If the Dashboard does not show up, one reason could be, that the
Microsoft .NET Framework 4.5 is not installed.
You can download the installer [here][.NET45].

## Next
Now you can [select and install your desired apps][select-apps].

## See Also

Tech Guides

* [Bench Setup and Upgrade](/guide/setup)
* [Isolation and Portability](/guide/isolation)

Reference Docs

* [Bench CLI](/ref/bench-ctl)
* [File Structure](/ref/file-structure)
* [Configuration Markup Syntax](/ref/markup-syntax)
* [Configuration Properties](/ref/config)

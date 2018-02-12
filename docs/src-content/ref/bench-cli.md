+++
date = "2018-02-12T18:00:11+01:00"
description = "The command-line interface: bench.exe"
title = "Bench CLI"
weight = 2
+++

Version: 0.18.0

The  _Bench CLI_ allows to interact with a Bench environment on the command line.

It supports a hierarchy of sub-commands with flags and options, which can be specified as command line arguments.
Additionally it supports an  _interactive mode_ when called without a sub-command specified.
Help texts can be displayed for each sub-command with the  `-?` argument. The help texts can be printed in  _different formats_.

Take a look at [the project website](https://winbench.org) for a description of the Bench system.

### Commands {#index}

* [ `bench`](#cmd_bench)
* [ `bench`  `app`](#cmd_bench-app)
* [ `bench`  `app`  `activate`](#cmd_bench-app-activate)
* [ `bench`  `app`  `deactivate`](#cmd_bench-app-deactivate)
* [ `bench`  `app`  `download`](#cmd_bench-app-download)
* [ `bench`  `app`  `execute`](#cmd_bench-app-execute)
* [ `bench`  `app`  `info`](#cmd_bench-app-info)
* [ `bench`  `app`  `install`](#cmd_bench-app-install)
* [ `bench`  `app`  `list-properties`](#cmd_bench-app-list-properties)
* [ `bench`  `app`  `property`](#cmd_bench-app-property)
* [ `bench`  `app`  `reinstall`](#cmd_bench-app-reinstall)
* [ `bench`  `app`  `uninstall`](#cmd_bench-app-uninstall)
* [ `bench`  `app`  `upgrade`](#cmd_bench-app-upgrade)
* [ `bench`  `dashboard`](#cmd_bench-dashboard)
* [ `bench`  `help`](#cmd_bench-help)
* [ `bench`  `list`](#cmd_bench-list)
* [ `bench`  `list`  `applibs`](#cmd_bench-list-applibs)
* [ `bench`  `list`  `apps`](#cmd_bench-list-apps)
* [ `bench`  `list`  `files`](#cmd_bench-list-files)
* [ `bench`  `manage`](#cmd_bench-manage)
* [ `bench`  `manage`  `config`](#cmd_bench-manage-config)
* [ `bench`  `manage`  `config`  `edit`](#cmd_bench-manage-config-edit)
* [ `bench`  `manage`  `config`  `get`](#cmd_bench-manage-config-get)
* [ `bench`  `manage`  `config`  `set`](#cmd_bench-manage-config-set)
* [ `bench`  `manage`  `downloads`](#cmd_bench-manage-downloads)
* [ `bench`  `manage`  `initialize`](#cmd_bench-manage-initialize)
* [ `bench`  `manage`  `load-app-libs`](#cmd_bench-manage-load-app-libs)
* [ `bench`  `manage`  `reinstall`](#cmd_bench-manage-reinstall)
* [ `bench`  `manage`  `renew`](#cmd_bench-manage-renew)
* [ `bench`  `manage`  `setup`](#cmd_bench-manage-setup)
* [ `bench`  `manage`  `update`](#cmd_bench-manage-update)
* [ `bench`  `manage`  `update-env`](#cmd_bench-manage-update-env)
* [ `bench`  `manage`  `upgrade`](#cmd_bench-manage-upgrade)
* [ `bench`  `project`](#cmd_bench-project)
* [ `bench`  `transfer`](#cmd_bench-transfer)
* [ `bench`  `transfer`  `clone`](#cmd_bench-transfer-clone)
* [ `bench`  `transfer`  `export`](#cmd_bench-transfer-export)
* [ `bench`  `transfer`  `install`](#cmd_bench-transfer-install)

## bench {#cmd_bench}
The  `bench` command is the executable of the Bench CLI.
You can call it without a sub-command to enter the  _interactive mode_.

### Usage {#cmd_bench_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;command&gt;_ ...

### Help {#cmd_bench_help}
Showing the help can be triggered by one of the following flags:  `/?`,  `-?`,  `-h`,  `--help`.

*  `bench`  `-?`
*  `bench`  _&lt;command&gt;_  `-?`

### Flags {#cmd_bench_flags}

####  `--verbose` |  `-v`
Activate verbose output.

####  `--yes` |  `--force` |  `-y`
Suppress all assurance questions.

### Options {#cmd_bench_options}

####  `--help-format` |  `-f`  _&lt;value&gt;_
Specify the output format of help texts.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

####  `--logfile` |  `--log` |  `-l`  _&lt;value&gt;_
Specify a custom location for the log file.

Expected: A path to the log file.  
Default: Auto generated filename in  _&lt;bench root&gt;_\log\.  

####  `--root` |  `--base` |  `-r`  _&lt;value&gt;_
Specify the root directory of the Bench environment.

Expected: A path to a valid Bench root directory.  
Default: The root directory of the Bench environment, this Bench CLI belongs to.  

### Commands {#cmd_bench_commands}

#### [ `app`,  `a`](#cmd_bench-app)
Manage individual apps.

Syntax:  `bench`  `app`  _&lt;sub-command&gt;_  

#### [ `dashboard`,  `gui`,  `b`](#cmd_bench-dashboard)
Start the  _Bench Dashboard_.

#### [ `help`,  `h`](#cmd_bench-help)
Display the full help for all commands.

#### [ `list`,  `l`](#cmd_bench-list)
List different kinds of objects in the Bench environment.

#### [ `manage`,  `m`](#cmd_bench-manage)
Manage the Bench environment and its configuration.

#### [ `project`,  `prj`,  `p`](#cmd_bench-project)
Manage projects in the Bench environment.

Syntax:  `bench`  `project`  

#### [ `transfer`,  `t`](#cmd_bench-transfer)
Copy or export this Bench environment.

## bench app {#cmd_bench-app}
Command:  `bench`  `app`

The  `app` command allows interacting with Bench apps.

Use the sub-commands to select the kind of interaction.

### Usage {#cmd_bench-app_usage}

*  `bench`  `app`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  _&lt;command&gt;_ ...

### Commands {#cmd_bench-app_commands}

#### [ `activate`,  `enable`,  `a`](#cmd_bench-app-activate)
Activates an app.

Syntax:  `bench`  `app`  `activate`  _&lt;App ID&gt;_  

#### [ `deactivate`,  `disable`,  `d`](#cmd_bench-app-deactivate)
Deactivates an app.

Syntax:  `bench`  `app`  `deactivate`  _&lt;App ID&gt;_  

#### [ `download`,  `cache`,  `c`](#cmd_bench-app-download)
Downloads an apps resource.

Syntax:  `bench`  `app`  `download`  _&lt;App ID&gt;_  

#### [ `execute`,  `exec`,  `launch`,  `run`,  `e`](#cmd_bench-app-execute)
Starts an apps main executable.

Syntax:  `bench`  `app`  `execute`  _&lt;flag&gt;_\*  _&lt;App ID&gt;_  

#### [ `info`,  `i`](#cmd_bench-app-info)
Shows a detailed, human readable info of an app.

Syntax:  `bench`  `app`  `info`  _&lt;option&gt;_\*  _&lt;App ID&gt;_  

#### [ `install`,  `setup`,  `s`](#cmd_bench-app-install)
Installs an app, regardless of its activation state.

Syntax:  `bench`  `app`  `install`  _&lt;App ID&gt;_  

#### [ `list-properties`,  `list`,  `l`](#cmd_bench-app-list-properties)
Lists the properties of an app.

Syntax:  `bench`  `app`  `list-properties` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;App ID&gt;_  

#### [ `property`,  `prop`,  `p`](#cmd_bench-app-property)
Reads an app property value.

Syntax:  `bench`  `app`  `property`  _&lt;App ID&gt;_  _&lt;Property Name&gt;_  

#### [ `reinstall`,  `r`](#cmd_bench-app-reinstall)
Reinstalls an app.

Syntax:  `bench`  `app`  `reinstall`  _&lt;App ID&gt;_  

#### [ `uninstall`,  `remove`,  `x`](#cmd_bench-app-uninstall)
Uninstalls an app, regardless of its activation state.

Syntax:  `bench`  `app`  `uninstall`  _&lt;App ID&gt;_  

#### [ `upgrade`,  `u`](#cmd_bench-app-upgrade)
Upgrades an app.

Syntax:  `bench`  `app`  `upgrade`  _&lt;App ID&gt;_  

## bench app activate {#cmd_bench-app-activate}
Command:  `bench`  `app`  `activate`

The  `activate` command marks an app as activated.

To actually install the app, you have to run the  `setup` command.

If the app is currently active as a dependency, it is marked as activated anyways.
If the app is required by Bench, it is not marked as activated.
If the app is marked as deactivated, this mark is removed.

### Usage {#cmd_bench-app-activate_usage}

*  `bench`  `app`  `activate`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `activate`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-activate_positionals}

####  1. App ID
Specifies the app to activate.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app deactivate {#cmd_bench-app-deactivate}
Command:  `bench`  `app`  `deactivate`

The  `deactivate` command removes an app from the activation list or marks it as deactivated.

To actually uninstall the app, you have to run the  `setup` command.

If the app is currently on the activation list, it is removed from it.
If the app is required by Bench, or as a dependency, it is marked as deactivated.

### Usage {#cmd_bench-app-deactivate_usage}

*  `bench`  `app`  `deactivate`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `deactivate`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-deactivate_positionals}

####  1. App ID
Specifies the app to deactivate.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app download {#cmd_bench-app-download}
Command:  `bench`  `app`  `download`

The  `download` command downloads the app resources, in case it is not cached already.

### Usage {#cmd_bench-app-download_usage}

*  `bench`  `app`  `download`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `download`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-download_positionals}

####  1. App ID
Specifies the app to download the resource for.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app execute {#cmd_bench-app-execute}
Command:  `bench`  `app`  `execute`

The  `execute` command starts the main executable of the specified app.

### Usage {#cmd_bench-app-execute_usage}

*  `bench`  `app`  `execute`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `execute`  _&lt;flag&gt;_\*  _&lt;App ID&gt;_

### Flags {#cmd_bench-app-execute_flags}

####  `--detached` |  `--async` |  `-d`
Do not wait for the end of the process.

### Positional Arguments {#cmd_bench-app-execute_positionals}

####  1. App ID
Specifies the app to execute.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app info {#cmd_bench-app-info}
Command:  `bench`  `app`  `info`

The  `info` command displayes a detailed description for an app in human readable form.

### Usage {#cmd_bench-app-info_usage}

*  `bench`  `app`  `info`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `info`  _&lt;option&gt;_\*  _&lt;App ID&gt;_

### Options {#cmd_bench-app-info_options}

####  `--format` |  `-f`  _&lt;value&gt;_
Specify the output format.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

### Positional Arguments {#cmd_bench-app-info_positionals}

####  1. App ID
Specifies the app to display the description for.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app install {#cmd_bench-app-install}
Command:  `bench`  `app`  `install`

The  `install` command installes the specified app, regardless of its activation state.

Missing app resources are downloaded automatically.

### Usage {#cmd_bench-app-install_usage}

*  `bench`  `app`  `install`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `install`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-install_positionals}

####  1. App ID
Specifies the app to install.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app list-properties {#cmd_bench-app-list-properties}
Command:  `bench`  `app`  `list-properties`

The  `list-properties` command displayes the properties of an app.

This command supports different output formats. And you can choose between the expanded or the raw properties.

### Usage {#cmd_bench-app-list-properties_usage}

*  `bench`  `app`  `list-properties`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `list-properties` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;App ID&gt;_

### Flags {#cmd_bench-app-list-properties_flags}

####  `--raw` |  `-r`
Shows the raw properties without expansion and default values.

### Options {#cmd_bench-app-list-properties_options}

####  `--format` |  `-f`  _&lt;value&gt;_
Specify the output format.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

### Positional Arguments {#cmd_bench-app-list-properties_positionals}

####  1. App ID
Specifies the app of which the properties are to be listed.

Expected: The apps ID, an alphanumeric string without whitespace.  

## bench app property {#cmd_bench-app-property}
Command:  `bench`  `app`  `property`

The  `property` command reads the value of an app property.

### Usage {#cmd_bench-app-property_usage}

*  `bench`  `app`  `property`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `property`  _&lt;App ID&gt;_  _&lt;Property Name&gt;_

### Positional Arguments {#cmd_bench-app-property_positionals}

####  1. App ID
Specifies the app to get the property from.

Expected: The apps ID, an alphanumeric string without whitespace.  

####  2. Property Name
Specifies the property to read.

Expected: The property name, an alphanumeric string without whitespace.  

## bench app reinstall {#cmd_bench-app-reinstall}
Command:  `bench`  `app`  `reinstall`

The  `reinstall` command reinstalles the specified app.

### Usage {#cmd_bench-app-reinstall_usage}

*  `bench`  `app`  `reinstall`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `reinstall`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-reinstall_positionals}

####  1. App ID
Specifies the app to reinstall.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app uninstall {#cmd_bench-app-uninstall}
Command:  `bench`  `app`  `uninstall`

The  `uninstall` command uninstalles the specified app, regardless of its activation state.

### Usage {#cmd_bench-app-uninstall_usage}

*  `bench`  `app`  `uninstall`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `uninstall`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-uninstall_positionals}

####  1. App ID
Specifies the app to install.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app upgrade {#cmd_bench-app-upgrade}
Command:  `bench`  `app`  `upgrade`

The  `upgrade` command upgrades the specified app to the most current release.

Updates app resources are downloaded automatically.

### Usage {#cmd_bench-app-upgrade_usage}

*  `bench`  `app`  `upgrade`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `upgrade`  _&lt;App ID&gt;_

### Positional Arguments {#cmd_bench-app-upgrade_positionals}

####  1. App ID
Specifies the app to upgrade.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench dashboard {#cmd_bench-dashboard}
Command:  `bench`  `dashboard`

The  `dashboard` command starts the graphical user interface  _Bench Dashboard_.

### Usage {#cmd_bench-dashboard_usage}

*  `bench`  `dashboard`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `dashboard`

## bench help {#cmd_bench-help}
Command:  `bench`  `help`

The  `help` command displays the full help for all commands.

### Usage {#cmd_bench-help_usage}

*  `bench`  `help`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `help` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*

### Flags {#cmd_bench-help_flags}

####  `--append` |  `-a`
Append to an existing file, in case a target file is specified.

####  `--no-index` |  `-i`
Suppress the index of the commands.

####  `--no-title` |  `-t`
Suppress the output of the tool name as the document title.

####  `--no-version` |  `-v`
Suppress the output of the tool version number.

### Options {#cmd_bench-help_options}

####  `--target-file` |  `--out` |  `-o`  _&lt;value&gt;_
Specifies a target file to write the help content to.

Expected: A path to a writable file. The target file will be created or overridden.  
Default: None  

## bench list {#cmd_bench-list}
Command:  `bench`  `list`

The  `list` command lists different kinds of objects from the Bench environment.

Choose a sub-command to specify the kind of object, you want to list.

### Usage {#cmd_bench-list_usage}

*  `bench`  `list`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;command&gt;_ ...

### Flags {#cmd_bench-list_flags}

####  `--table` |  `-t`
Prints properties of the listed objects as a table. Otherwise only a short form is printed.

### Options {#cmd_bench-list_options}

####  `--format` |  `-f`  _&lt;value&gt;_
Specifies the output format of the listed data.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

### Commands {#cmd_bench-list_commands}

#### [ `applibs`,  `l`](#cmd_bench-list-applibs)
List app libraries with ID and URL.

#### [ `apps`,  `a`](#cmd_bench-list-apps)
List apps from the app library.

#### [ `files`,  `f`](#cmd_bench-list-files)
List configuration and app library index files.

## bench list applibs {#cmd_bench-list-applibs}
Command:  `bench`  `list`  `applibs`

The  `applibs` command lists all loaded app libraries.

### Usage {#cmd_bench-list-applibs_usage}

*  `bench`  `list`  `applibs`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `applibs`

## bench list apps {#cmd_bench-list-apps}
Command:  `bench`  `list`  `apps`

The  `apps` command lists apps from the app library.

You can specify the base set of apps and filter the apps to list.

### Usage {#cmd_bench-list-apps_usage}

*  `bench`  `list`  `apps`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `apps`  _&lt;option&gt;_\*

### Options {#cmd_bench-list-apps_options}

####  `--filter` |  `-f`  _&lt;value&gt;_
Specifies a filter to reduce the number of listed apps.

Expected: A comma separated list of criteria. E.g.  `ID=JDK*,!IsInstalled,IsCached`.  
Default: no filter  

####  `--properties` |  `-p`  _&lt;value&gt;_
Specifies the properties to display in the listed output. This option only has an effect, if the flag  `list`  `--table` is set.

Expected: A comma separated list of property names.  

####  `--set` |  `-s`  _&lt;value&gt;_
Specifies the set of apps to list.

Expected:  `All` |  `Active` |  `NotActive` |  `Activated` |  `Deactivated` |  `NotSupported` |  `Installed` |  `NotInstalled` |  `Cached` |  `NotCached` |  `DefaultApps` |  `MetaApps` |  `ManagedPackages`  
Default:  `All`  

####  `--sort-by` |  `-o`  _&lt;value&gt;_
Specifies a property to sort the apps by.

Expected: The name of an app property.  
Default: ID  

## bench list files {#cmd_bench-list-files}
Command:  `bench`  `list`  `files`

The  `files` command lists the paths of all loaded configuration files.

### Usage {#cmd_bench-list-files_usage}

*  `bench`  `list`  `files`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `files`  _&lt;option&gt;_\*

### Options {#cmd_bench-list-files_options}

####  `--type` |  `-t`  _&lt;value&gt;_
Specify the type of files to show.

Expected:  `BenchConfig` |  `UserConfig` |  `SiteConfig` |  `Config` |  `BenchAppLib` |  `UserAppLib` |  `AppLib` |  `Activation` |  `Deactivation` |  `AppSelection` |  `All`  
Default:  `All`  

## bench manage {#cmd_bench-manage}
Command:  `bench`  `manage`

The  `manage` command manages the Bench environment via a number of sub-commands.

### Usage {#cmd_bench-manage_usage}

*  `bench`  `manage`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  _&lt;command&gt;_ ...

### Commands {#cmd_bench-manage_commands}

#### [ `config`,  `cfg`,  `c`](#cmd_bench-manage-config)
Read or write values from the user configuration.

Syntax:  `bench`  `manage`  `config`  _&lt;sub-command&gt;_  

#### [ `initialize`,  `init`,  `i`](#cmd_bench-manage-initialize)
Initialize the Bench configuration and start the setup process.

#### [ `load-app-libs`,  `l`](#cmd_bench-manage-load-app-libs)
Load the latest app libraries.

Syntax:  `bench`  `manage`  `load-app-libs`  _&lt;flag&gt;_\*  

#### [ `reinstall`,  `r`](#cmd_bench-manage-reinstall)
Remove all installed apps, then install all active apps.

#### [ `renew`,  `n`](#cmd_bench-manage-renew)
Redownload all app resources, remove all installed apps, then install all active apps.

#### [ `setup`,  `s`](#cmd_bench-manage-setup)
Run the auto-setup for the active Bench apps.

#### [ `update`,  `u`](#cmd_bench-manage-update)
Update the app libraries and upgrades all apps.

#### [ `update-env`,  `e`](#cmd_bench-manage-update-env)
Update the paths in the Bench environment.

#### [ `upgrade`,  `g`](#cmd_bench-manage-upgrade)
Download and extract the latest Bench release, then run the auto-setup.

## bench manage config {#cmd_bench-manage-config}
Command:  `bench`  `manage`  `config`

The  `config` command gives access to the Bench user configuration.

### Usage {#cmd_bench-manage-config_usage}

*  `bench`  `manage`  `config`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`  _&lt;command&gt;_ ...

### Commands {#cmd_bench-manage-config_commands}

#### [ `edit`,  `e`](#cmd_bench-manage-config-edit)
Opens the user configuration in the default Markdown editor.

Syntax:  `bench`  `manage`  `config`  `edit`  _&lt;flag&gt;_\*  

#### [ `get`,  `read`,  `g`](#cmd_bench-manage-config-get)
Reads a configuration value.

Syntax:  `bench`  `manage`  `config`  `get`  _&lt;Property Name&gt;_  

#### [ `set`,  `write`,  `s`](#cmd_bench-manage-config-set)
Writes a configuration value.

Syntax:  `bench`  `manage`  `config`  `set`  _&lt;Property Name&gt;_  _&lt;New Value&gt;_  

## bench manage config edit {#cmd_bench-manage-config-edit}
Command:  `bench`  `manage`  `config`  `edit`

The  `edit` command opens the user configuration in the default Markdown editor.

### Usage {#cmd_bench-manage-config-edit_usage}

*  `bench`  `manage`  `config`  `edit`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`  `edit`  _&lt;flag&gt;_\*

### Flags {#cmd_bench-manage-config-edit_flags}

####  `--detached` |  `--async` |  `-d`
Do not wait for the editor to be closed.

## bench manage config get {#cmd_bench-manage-config-get}
Command:  `bench`  `manage`  `config`  `get`

The  `get` command reads a configuration value.

### Usage {#cmd_bench-manage-config-get_usage}

*  `bench`  `manage`  `config`  `get`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`  `get`  _&lt;Property Name&gt;_

### Positional Arguments {#cmd_bench-manage-config-get_positionals}

####  1. Property Name
The name of the configuration property to read.

## bench manage config set {#cmd_bench-manage-config-set}
Command:  `bench`  `manage`  `config`  `set`

The  `set` command writes a configuration value to the user configuration file.

### Usage {#cmd_bench-manage-config-set_usage}

*  `bench`  `manage`  `config`  `set`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`  `set`  _&lt;Property Name&gt;_  _&lt;New Value&gt;_

### Positional Arguments {#cmd_bench-manage-config-set_positionals}

####  1. Property Name
The name of the configuration property to write.

####  2. New Value
The new value for the configuration property.

## bench manage downloads {#cmd_bench-manage-downloads}
Command:  `bench`  `manage`  `downloads`

The  `downloads` command manages the cached app resources.

### Usage {#cmd_bench-manage-downloads_usage}

*  `bench`  `manage`  `downloads`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `downloads`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `downloads`  _&lt;command&gt;_ ...

### Commands {#cmd_bench-manage-downloads_commands}

#### [ `clean`,  `cl`,  `c`](#cmd_bench-manage-downloads-clean)
Deletes obsolete app resources.

#### [ `download`,  `dl`,  `d`](#cmd_bench-manage-downloads-download)
Downloads the app resources for all active apps.

#### [ `purge`,  `x`](#cmd_bench-manage-downloads-purge)
Deletes all cached app resources.

## bench manage initialize {#cmd_bench-manage-initialize}
Command:  `bench`  `manage`  `initialize`

The  `initialize` command initializes the Bench configuration and starts the setup process.

### Usage {#cmd_bench-manage-initialize_usage}

*  `bench`  `manage`  `initialize`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `initialize`

## bench manage load-app-libs {#cmd_bench-manage-load-app-libs}
Command:  `bench`  `manage`  `load-app-libs`

The  `load-app-libs` command loads missing app libraries.

### Usage {#cmd_bench-manage-load-app-libs_usage}

*  `bench`  `manage`  `load-app-libs`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `load-app-libs`  _&lt;flag&gt;_\*

### Flags {#cmd_bench-manage-load-app-libs_flags}

####  `--update` |  `-u`
Clears the cache and loads the latest version of all app libraries.

## bench manage reinstall {#cmd_bench-manage-reinstall}
Command:  `bench`  `manage`  `reinstall`

The  `reinstall` command removes all installed apps, then installs all active apps.

### Usage {#cmd_bench-manage-reinstall_usage}

*  `bench`  `manage`  `reinstall`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `reinstall`

## bench manage renew {#cmd_bench-manage-renew}
Command:  `bench`  `manage`  `renew`

The  `renew` command redownloads all app resources, removes all installed apps, then installs all active apps.

### Usage {#cmd_bench-manage-renew_usage}

*  `bench`  `manage`  `renew`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `renew`

## bench manage setup {#cmd_bench-manage-setup}
Command:  `bench`  `manage`  `setup`

The  `setup` command runs the auto-setup for the active Bench apps.

### Usage {#cmd_bench-manage-setup_usage}

*  `bench`  `manage`  `setup`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `setup`

## bench manage update {#cmd_bench-manage-update}
Command:  `bench`  `manage`  `update`

The  `update` command updates the app libraries and upgrades all apps.

### Usage {#cmd_bench-manage-update_usage}

*  `bench`  `manage`  `update`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `update`

## bench manage update-env {#cmd_bench-manage-update-env}
Command:  `bench`  `manage`  `update-env`

The  `update-env` command updates the paths in the Bench environment.

### Usage {#cmd_bench-manage-update-env_usage}

*  `bench`  `manage`  `update-env`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `update-env`

## bench manage upgrade {#cmd_bench-manage-upgrade}
Command:  `bench`  `manage`  `upgrade`

The  `upgrade` command checks if a new version of Bench is available and installs it.

### Usage {#cmd_bench-manage-upgrade_usage}

*  `bench`  `manage`  `upgrade`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `upgrade`

## bench project {#cmd_bench-project}
Command:  `bench`  `project`

The  `project` command allows you to perform certain tasks on projects in the Bench environment.

 **WARNING: This command is not implemented yet.**

### Usage {#cmd_bench-project_usage}

*  `bench`  `project`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `project`

## bench transfer {#cmd_bench-transfer}
Command:  `bench`  `transfer`

The  `transfer` command supports different kinds of a copying the whole Bench environment.

### Usage {#cmd_bench-transfer_usage}

*  `bench`  `transfer`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `transfer`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `transfer`  _&lt;command&gt;_ ...

### Commands {#cmd_bench-transfer_commands}

#### [ `clone`,  `c`](#cmd_bench-transfer-clone)
Copy this Bench environment to a different place.

#### [ `export`,  `e`](#cmd_bench-transfer-export)
Create a transfer package of this Bench environment

Syntax:  `bench`  `transfer`  `export`  _&lt;option&gt;_\*  _&lt;target-file&gt;_  

#### [ `install`,  `i`](#cmd_bench-transfer-install)
Install a Bench environment from an extracted Bench transfer package

Syntax:  `bench`  `transfer`  `install` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  

## bench transfer clone {#cmd_bench-transfer-clone}
Command:  `bench`  `transfer`  `clone`

The  `clone` command creates and initializes a clone of this Bench environment in a different location.

### Usage {#cmd_bench-transfer-clone_usage}

*  `bench`  `transfer`  `clone`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `transfer`  `clone`  _&lt;option&gt;_\*  _&lt;target-dir&gt;_

### Options {#cmd_bench-transfer-clone_options}

####  `--include` |  `-i`  _&lt;value&gt;_
Specifies the content included in the export.

Expected: A comma separated list of the following keywords:  `SystemOnly`,  `Config`,  `Home`,  `Projects`,  `AppLibs`,  `RequiredCache`,  `Cache`,  `RequiredApps`,  `Apps`,  `All`  
Default: Config,AppLibs,Cache  

### Positional Arguments {#cmd_bench-transfer-clone_positionals}

####  1. target-dir
The target directory for the clone.

Expected: A path to a directory. The directory must not exist yet.  

## bench transfer export {#cmd_bench-transfer-export}
Command:  `bench`  `transfer`  `export`

The  `export` command creates a transfer package of this Bench environment.

### Usage {#cmd_bench-transfer-export_usage}

*  `bench`  `transfer`  `export`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `transfer`  `export`  _&lt;option&gt;_\*  _&lt;target-file&gt;_

### Options {#cmd_bench-transfer-export_options}

####  `--include` |  `-i`  _&lt;value&gt;_
Specifies the content included in the export.

Expected: A comma separated list of the following keywords:  `SystemOnly`,  `Config`,  `Home`,  `Projects`,  `AppLibs`,  `RequiredCache`,  `Cache`,  `RequiredApps`,  `Apps`,  `All`  
Default: Config,AppLibs  

### Positional Arguments {#cmd_bench-transfer-export_positionals}

####  1. target-file
A path to the output file.

Expected: The filename can have one of the following extensions:  `.zip`,  `.7z`,  `.exe`  

## bench transfer install {#cmd_bench-transfer-install}
Command:  `bench`  `transfer`  `install`

The  `install` command installs a Bench environment from an extracted Bench transfer package.

### Usage {#cmd_bench-transfer-install_usage}

*  `bench`  `transfer`  `install`  `-?`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `transfer`  `install` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*

### Flags {#cmd_bench-transfer-install_flags}

####  `--extract-only` |  `--extract` |  `--no-init` |  `-e`
Deactivates automatic initialization and setup after the transfer.

### Options {#cmd_bench-transfer-install_options}

####  `--target-dir` |  `--target` |  `--dir` |  `-d`  _&lt;value&gt;_
Specifies the target directory for the installation.
If left empty, a directory browser will be displayed to choose the target directory.

Expected: A path to a directory. The directory must not exist yet.  


+++
date = "2016-12-22T16:35:10+01:00"
description = "The command-line interface: bench.exe"
title = "Bench CLI"
weight = 2
+++

Version: 0.14.0

The  _Bench CLI_ allows to interact with a Bench environment on the command line.

It supports a hierarchy of sub-commands with flags and options, which can be specified as command line arguments.
Additionally it supports an  _interactive mode_ when called without a sub-command specified.
Help texts can be displayed for each sub-command with the  `-?` argument. The help texts can be printed in  _different formats_.

Take a look at [the project website](http://mastersign.github.io/bench) for a description of the Bench system.

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
* [ `bench`  `list`  `apps`](#cmd_bench-list-apps)
* [ `bench`  `manage`](#cmd_bench-manage)
* [ `bench`  `manage`  `config`](#cmd_bench-manage-config)
* [ `bench`  `manage`  `downloads`](#cmd_bench-manage-downloads)
* [ `bench`  `manage`  `initialize`](#cmd_bench-manage-initialize)
* [ `bench`  `manage`  `reinstall`](#cmd_bench-manage-reinstall)
* [ `bench`  `manage`  `renew`](#cmd_bench-manage-renew)
* [ `bench`  `manage`  `setup`](#cmd_bench-manage-setup)
* [ `bench`  `manage`  `update-env`](#cmd_bench-manage-update-env)
* [ `bench`  `manage`  `upgrade`](#cmd_bench-manage-upgrade)
* [ `bench`  `project`](#cmd_bench-project)

## bench {#cmd_bench}

The  `bench` command is the executable of the Bench CLI.
You can call it without a sub-command to enter the  _interactive mode_.

### Usage {#cmd_bench_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;command&gt;_ ...

### Help {#cmd_bench_help}

*  `bench` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Flags {#cmd_bench_flags}

####  `--verbose` |  `-v`

Activates verbose output.

####  `--yes` |  `--force` |  `-y`

Suppresses all assurance questions.

### Options {#cmd_bench_options}

####  `--help-format` |  `-f`  _&lt;value&gt;_

Specifies the output format of help texts.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

####  `--logfile` |  `--log` |  `-l`  _&lt;value&gt;_

Specifies a custom location for the log file.

Expected: A path to the log file.  
Default: Auto generated filename in  _&lt;bench root&gt;_\log\.  

####  `--root` |  `--base` |  `-r`  _&lt;value&gt;_

Specifies the root directory of the Bench environment.

Expected: A path to a valid Bench root directory.  
Default: The root directory of the Bench environment, this Bench CLI belongs to.  

### Commands {#cmd_bench_commands}

####  `app`,  `a`

Manage individual apps.

Syntax:  `bench`  `app`  _&lt;sub-command&gt;_  

####  `dashboard`,  `gui`,  `b`

Starts the  _Bench Dashboard_.

####  `help`,  `h`

Displays the full help for all commands.

####  `list`,  `l`

Lists different kinds of objects in the Bench environment.

####  `manage`,  `m`

Manages the Bench environment and its configuration.

####  `project`,  `prj`,  `p`

Manage projects in the Bench environment.

Syntax:  `bench`  `project`  

## bench app {#cmd_bench-app}

Command:  `bench`  `app`

The  `app` command allows interacting with Bench apps.

Use the sub-commands to select the kind of interaction.

### Usage {#cmd_bench-app_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  _&lt;command&gt;_ ...

### Help {#cmd_bench-app_help}

*  `bench`  `app` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  `app`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Commands {#cmd_bench-app_commands}

####  `activate`,  `enable`,  `a`

Activates an app.

Syntax:  `bench`  `app`  `activate`  _&lt;App ID&gt;_  

####  `deactivate`,  `disable`,  `d`

Deactivates an app.

Syntax:  `bench`  `app`  `deactivate`  _&lt;App ID&gt;_  

####  `download`,  `cache`,  `c`

Downloads an apps resource.

Syntax:  `bench`  `app`  `download`  _&lt;App ID&gt;_  

####  `execute`,  `exec`,  `launch`,  `run`,  `e`

Starts an apps main executable.

Syntax:  `bench`  `app`  `execute`  _&lt;flag&gt;_\*  _&lt;App ID&gt;_  

####  `info`,  `i`

Shows a detailed, human readable info of an app.

Syntax:  `bench`  `app`  `info`  _&lt;option&gt;_\*  _&lt;App ID&gt;_  

####  `install`,  `setup`,  `s`

Installs an app, regardless of its activation state.

Syntax:  `bench`  `app`  `install`  _&lt;App ID&gt;_  

####  `list-properties`,  `list`,  `l`

Lists the properties of an app.

Syntax:  `bench`  `app`  `list-properties` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;App ID&gt;_  

####  `property`,  `prop`,  `p`

Reads an app property value.

Syntax:  `bench`  `app`  `property`  _&lt;App ID&gt;_  _&lt;Property Name&gt;_  

####  `reinstall`,  `r`

Reinstalls an app.

Syntax:  `bench`  `app`  `reinstall`  _&lt;App ID&gt;_  

####  `uninstall`,  `remove`,  `x`

Uninstalls an app, regardless of its activation state.

Syntax:  `bench`  `app`  `uninstall`  _&lt;App ID&gt;_  

####  `upgrade`,  `u`

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `activate`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-activate_help}

*  `bench`  `app`  `activate` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `deactivate`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-deactivate_help}

*  `bench`  `app`  `deactivate` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-deactivate_positionals}

####  1. App ID

Specifies the app to deactivate.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app download {#cmd_bench-app-download}

Command:  `bench`  `app`  `download`

The  `download` command downloads the app resources, in case it is not cached already.

### Usage {#cmd_bench-app-download_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `download`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-download_help}

*  `bench`  `app`  `download` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-download_positionals}

####  1. App ID

Specifies the app to download the resource for.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app execute {#cmd_bench-app-execute}

Command:  `bench`  `app`  `execute`

The  `execute` command starts the main executable of the specified app.

### Usage {#cmd_bench-app-execute_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `execute`  _&lt;flag&gt;_\*  _&lt;App ID&gt;_

### Help {#cmd_bench-app-execute_help}

*  `bench`  `app`  `execute` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `info`  _&lt;option&gt;_\*  _&lt;App ID&gt;_

### Help {#cmd_bench-app-info_help}

*  `bench`  `app`  `info` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `install`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-install_help}

*  `bench`  `app`  `install` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-install_positionals}

####  1. App ID

Specifies the app to install.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app list-properties {#cmd_bench-app-list-properties}

Command:  `bench`  `app`  `list-properties`

The  `list-properties` command displayes the properties of an app.

This command supports different output formats. And you can choose between the expanded or the raw properties.

### Usage {#cmd_bench-app-list-properties_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `list-properties` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;App ID&gt;_

### Help {#cmd_bench-app-list-properties_help}

*  `bench`  `app`  `list-properties` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `property`  _&lt;App ID&gt;_  _&lt;Property Name&gt;_

### Help {#cmd_bench-app-property_help}

*  `bench`  `app`  `property` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `reinstall`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-reinstall_help}

*  `bench`  `app`  `reinstall` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-reinstall_positionals}

####  1. App ID

Specifies the app to reinstall.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app uninstall {#cmd_bench-app-uninstall}

Command:  `bench`  `app`  `uninstall`

The  `uninstall` command uninstalles the specified app, regardless of its activation state.

### Usage {#cmd_bench-app-uninstall_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `uninstall`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-uninstall_help}

*  `bench`  `app`  `uninstall` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-uninstall_positionals}

####  1. App ID

Specifies the app to install.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench app upgrade {#cmd_bench-app-upgrade}

Command:  `bench`  `app`  `upgrade`

The  `upgrade` command upgrades the specified app to the most current release.

Updates app resources are downloaded automatically.

### Usage {#cmd_bench-app-upgrade_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `app`  `upgrade`  _&lt;App ID&gt;_

### Help {#cmd_bench-app-upgrade_help}

*  `bench`  `app`  `upgrade` ( `/?` |  `-?` |  `-h` |  `--help`)

### Positional Arguments {#cmd_bench-app-upgrade_positionals}

####  1. App ID

Specifies the app to upgrade.

Expected: An app ID is an alphanumeric string without whitespace.  

## bench dashboard {#cmd_bench-dashboard}

Command:  `bench`  `dashboard`

The  `dashboard` command starts the graphical user interface  _Bench Dashboard_.

### Usage {#cmd_bench-dashboard_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `dashboard`

### Help {#cmd_bench-dashboard_help}

*  `bench`  `dashboard` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench help {#cmd_bench-help}

Command:  `bench`  `help`

Displays the full help for all commands.### Usage {#cmd_bench-help_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `help` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*

### Help {#cmd_bench-help_help}

*  `bench`  `help` ( `/?` |  `-?` |  `-h` |  `--help`)

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

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  _&lt;command&gt;_ ...

### Help {#cmd_bench-list_help}

*  `bench`  `list` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  `list`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Flags {#cmd_bench-list_flags}

####  `--table` |  `-t`

Prints properties of the listed objects as a table. Otherwise only the ID is printed.

### Options {#cmd_bench-list_options}

####  `--format` |  `-f`  _&lt;value&gt;_

Specifies the output format of the listed data.

Expected:  `Plain` |  `Markdown`  
Default:  `Plain`  

### Commands {#cmd_bench-list_commands}

####  `apps`,  `a`

List apps from the app library.

## bench list apps {#cmd_bench-list-apps}

Command:  `bench`  `list`  `apps`

The  `apps` command lists apps from the app library.

You can specify the base set of apps and filter the apps to list.

### Usage {#cmd_bench-list-apps_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `list` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `apps`  _&lt;option&gt;_\*

### Help {#cmd_bench-list-apps_help}

*  `bench`  `list`  `apps` ( `/?` |  `-?` |  `-h` |  `--help`)

### Options {#cmd_bench-list-apps_options}

####  `--filter` |  `-f`  _&lt;value&gt;_

Specifies a filter to reduce the number of listed apps.

Expected: A comma separated list of criteria. E.g.  `ID=JDK\*,!IsInstalled,IsCached`.  
Default: no filter  

####  `--properties` |  `-p`  _&lt;value&gt;_

Specifies the properties to display in the listed output. This option only has an effect, if the flag  `list`  `--table` is set.

Expected: A comma separated list of property names.  

####  `--set` |  `-s`  _&lt;value&gt;_

Specifies the set of apps to list.

Expected:  `All` |  `Active` |  `NotActive` |  `Activated` |  `Deactivated` |  `Installed` |  `NotInstalled` |  `Cached` |  `NotCached` |  `DefaultApps` |  `MetaApps` |  `ManagedPackages`  
Default:  `All`  

####  `--sort-by` |  `-o`  _&lt;value&gt;_

Specifies a property to sort the apps by.

Expected: The name of an app property.  
Default: ID  

## bench manage {#cmd_bench-manage}

Command:  `bench`  `manage`

The  `manage` command manages the Bench environment via a number of sub-commands.

### Usage {#cmd_bench-manage_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  _&lt;command&gt;_ ...

### Help {#cmd_bench-manage_help}

*  `bench`  `manage` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  `manage`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Commands {#cmd_bench-manage_commands}

####  `config`,  `cfg`,  `c`

Read or write values from the user configuration.

Syntax:  `bench`  `manage`  `config`  _&lt;sub-command&gt;_  

####  `initialize`,  `init`,  `i`

Initialize the Bench configuration and start the setup process.

####  `reinstall`,  `r`

Remove all installed apps, then install all active apps.

####  `renew`,  `n`

Redownload all app resources, remove all installed apps, then install all active apps.

####  `setup`,  `s`

Run the auto-setup for the active Bench apps.

####  `update-env`,  `e`

Update the paths in the Bench environment.

####  `upgrade`,  `u`

Download and extract the latest Bench release, then run the auto-setup.

## bench manage config {#cmd_bench-manage-config}

Command:  `bench`  `manage`  `config`

The  `config` command gives access to the Bench user configuration.

### Usage {#cmd_bench-manage-config_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `config`  _&lt;command&gt;_ ...

### Help {#cmd_bench-manage-config_help}

*  `bench`  `manage`  `config` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  `manage`  `config`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Commands {#cmd_bench-manage-config_commands}

####  `get`,  `g`

Reads a configuration value.

Syntax:  _&lt;property name&gt;_  

## bench manage downloads {#cmd_bench-manage-downloads}

Command:  `bench`  `manage`  `downloads`

The  `downloads` command manages the cached app resources.

### Usage {#cmd_bench-manage-downloads_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `downloads`
*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `downloads`  _&lt;command&gt;_ ...

### Help {#cmd_bench-manage-downloads_help}

*  `bench`  `manage`  `downloads` ( `/?` |  `-?` |  `-h` |  `--help`)
*  `bench`  `manage`  `downloads`  _&lt;command&gt;_ ( `/?` |  `-?` |  `-h` |  `--help`)

### Commands {#cmd_bench-manage-downloads_commands}

####  `clean`,  `cl`,  `c`

Deletes obsolete app resources.

####  `download`,  `dl`,  `d`

Downloads the app resources for all active apps.

####  `purge`,  `x`

Deletes all cached app resources.

## bench manage initialize {#cmd_bench-manage-initialize}

Command:  `bench`  `manage`  `initialize`

The  `initialize` command initializes the Bench configuration and starts the setup process.

### Usage {#cmd_bench-manage-initialize_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `initialize`

### Help {#cmd_bench-manage-initialize_help}

*  `bench`  `manage`  `initialize` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench manage reinstall {#cmd_bench-manage-reinstall}

Command:  `bench`  `manage`  `reinstall`

The  `reinstall` command removes all installed apps, then installs all active apps.

### Usage {#cmd_bench-manage-reinstall_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `reinstall`

### Help {#cmd_bench-manage-reinstall_help}

*  `bench`  `manage`  `reinstall` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench manage renew {#cmd_bench-manage-renew}

Command:  `bench`  `manage`  `renew`

The  `renew` command redownloads all app resources, removes all installed apps, then installs all active apps.

### Usage {#cmd_bench-manage-renew_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `renew`

### Help {#cmd_bench-manage-renew_help}

*  `bench`  `manage`  `renew` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench manage setup {#cmd_bench-manage-setup}

Command:  `bench`  `manage`  `setup`

The  `setup` command runs the auto-setup for the active Bench apps.

### Usage {#cmd_bench-manage-setup_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `setup`

### Help {#cmd_bench-manage-setup_help}

*  `bench`  `manage`  `setup` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench manage update-env {#cmd_bench-manage-update-env}

Command:  `bench`  `manage`  `update-env`

The  `update-env` command updates the paths in the Bench environment.

### Usage {#cmd_bench-manage-update-env_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `update-env`

### Help {#cmd_bench-manage-update-env_help}

*  `bench`  `manage`  `update-env` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench manage upgrade {#cmd_bench-manage-upgrade}

Command:  `bench`  `manage`  `upgrade`

The  `upgrade` command checks if a new version of Bench is available and installs it.

### Usage {#cmd_bench-manage-upgrade_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `manage`  `upgrade`

### Help {#cmd_bench-manage-upgrade_help}

*  `bench`  `manage`  `upgrade` ( `/?` |  `-?` |  `-h` |  `--help`)

## bench project {#cmd_bench-project}

Command:  `bench`  `project`

The  `project` command allows you to perform certain tasks on projects in the Bench environment.

 **WARNING: This command is not implemented yet.**

### Usage {#cmd_bench-project_usage}

*  `bench` ( _&lt;flag&gt;_ |  _&lt;option&gt;_)\*  `project`

### Help {#cmd_bench-project_help}

*  `bench`  `project` ( `/?` |  `-?` |  `-h` |  `--help`)


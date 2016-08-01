---
title: "BenchLib - DefaultBenchManager"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## DefaultBenchManager Class
This is the default implementation of a  [`IBenchManager`](/clr-api/mastersign-bench-ibenchmanager/). 

**Absolute Name:** `Mastersign.Bench.DefaultBenchManager`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0

### Remarks
This implementation currently also acts as a facade for  [`BenchTasks`](/clr-api/mastersign-bench-benchtasks/)to be used by `auto\lib\Setup-Bench.ps1`, which is certainly an indicator for a bad design decision, and should be refactored. 



### Overview
**Constructors**

* [DefaultBenchManager(BenchConfiguration)](#ctor)

**Properties**

* [Config](#config)
* [Downloader](#downloader)
* [Env](#env)
* [ProcessExecutionHost](#processexecutionhost)
* [UI](#ui)
* [Verbose](#verbose)

**Methods**

* [SetupRequiredApps()](#setuprequiredapps)
* [AutoSetup()](#autosetup)
* [UpdateEnvironment()](#updateenvironment)
* [DownloadAppResources()](#downloadappresources)
* [DeleteAppResources()](#deleteappresources)
* [InstallApps()](#installapps)
* [UninstallApps()](#uninstallapps)
* [ReinstallApps()](#reinstallapps)
* [UpgradeApps()](#upgradeapps)

### Constructors {#ctors}

#### DefaultBenchManager(BenchConfiguration) {#ctor}
Initializes a new instance of  [`DefaultBenchManager`](/clr-api/mastersign-bench-defaultbenchmanager/)with a  [`DefaultExecutionHost`](/clr-api/mastersign-bench-defaultexecutionhost/) and a  [`ConsoleUserInterface`](/clr-api/mastersign-bench-consoleuserinterface/). 

##### Parameter

* **config**  
  The Bench configuration.

### Properties {#properties}

#### Config {#config}
The configuration of the Bench system. 

#### Downloader {#downloader}
The downloader for downloading app resources. 

#### Env {#env}
The environment variables of the Bench system. 

#### ProcessExecutionHost {#processexecutionhost}
The host for starting and running Windows processes. 

#### UI {#ui}
The user interface to communicate with the user. 

#### Verbose {#verbose}
A flag, controlling if non error messages should be displayed to the user. If it is set to `true`, all messages are displayed; otherwise only error messages are displayed. 

### Methods {#methods}

#### SetupRequiredApps() {#setuprequiredapps}
Sets up only the apps required by Bench. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### AutoSetup() {#autosetup}
Runs a full automatic setup, including app resource download, app installation of all active apps and setup of the environment. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### UpdateEnvironment() {#updateenvironment}
Sets up the environment, including the `env.cmd`, the launcher scripts and launcher shortcuts. It also runs all custom environment scripts. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### DownloadAppResources() {#downloadappresources}
Downloads the app resources for all active apps, in case they are not already cached. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### DeleteAppResources() {#deleteappresources}
Deletes all cached app resources. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### InstallApps() {#installapps}
Installs all active apps, in case they are not already installed. This also downloads missing app resources. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### UninstallApps() {#uninstallapps}
Uninstalls all installed apps. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### ReinstallApps() {#reinstallapps}
Uninstalls all installed apps, and then installs all active apps again. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.

#### UpgradeApps() {#upgradeapps}
Upgrades all active apps, which can be upgraded. 

##### Return Value
`true` if the execution of the task was successful; otherwise `false`.


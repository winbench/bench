---
title: "BenchLib - BenchTasks"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## BenchTasks Class
This class implements the core logic of Bench tasks. It is designed as a static class and acts hereby as kind of function library. 

**Absolute Name:** `Mastersign.Bench.BenchTasks`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Properties**

* [IsDashboardSupported](#isdashboardsupported)

**Methods**

* [InitializeCustomConfiguration(IBenchManager)](#initializecustomconfiguration)
* [InitializeDownloader(BenchConfiguration)](#initializedownloader)
* [LaunchApp(BenchConfiguration, BenchEnvironment, string, string)](#launchapp)
* [DoSetupRequiredApps(IBenchManager, TaskInfo}, Cancelation)](#dosetuprequiredapps)
* [DoAutoSetup(IBenchManager, TaskInfo}, Cancelation)](#doautosetup)
* [DoDownloadAppResources(IBenchManager, TaskInfo}, Cancelation)](#dodownloadappresources-b543e4cc)
* [DoDownloadAppResources(IBenchManager, string, TaskInfo}, Cancelation)](#dodownloadappresources-ddf8bec8)
* [DoDownloadAllAppResources(IBenchManager, TaskInfo}, Cancelation)](#dodownloadallappresources)
* [DoDeleteAppResources(IBenchManager, TaskInfo}, Cancelation)](#dodeleteappresources-ff51455e)
* [DoDeleteAppResources(IBenchManager, string, TaskInfo}, Cancelation)](#dodeleteappresources-4873ce60)
* [DoCleanUpAppResources(IBenchManager, TaskInfo}, Cancelation)](#docleanupappresources)
* [DoInstallApps(IBenchManager, TaskInfo}, Cancelation)](#doinstallapps-c48a583e)
* [DoInstallApps(IBenchManager, string, TaskInfo}, Cancelation)](#doinstallapps-77146881)
* [DoUninstallApps(IBenchManager, TaskInfo}, Cancelation)](#douninstallapps-29a29c35)
* [DoUninstallApps(IBenchManager, string, TaskInfo}, Cancelation)](#douninstallapps-e392ea67)
* [DoReinstallApps(IBenchManager, TaskInfo}, Cancelation)](#doreinstallapps-a96625db)
* [DoReinstallApps(IBenchManager, string, TaskInfo}, Cancelation)](#doreinstallapps-16b3e7f5)
* [DoUpgradeApps(IBenchManager, TaskInfo}, Cancelation)](#doupgradeapps-745a9af5)
* [DoUpgradeApps(IBenchManager, string, TaskInfo}, Cancelation)](#doupgradeapps-d027e46e)
* [DoUpdateEnvironment(IBenchManager, TaskInfo}, Cancelation)](#doupdateenvironment)

### Properties {#properties}

#### IsDashboardSupported {#isdashboardsupported}
Checks, whether the installed .NET framework has at least the version 4.5, and therefore supports the BenchDashboard user interface. 

### Methods {#methods}

#### InitializeCustomConfiguration(IBenchManager) {#initializecustomconfiguration}
##### Remarks
Precondition: Git must be set up. 

#### InitializeDownloader(BenchConfiguration) {#initializedownloader}
Creates a new instance of  [`Downloader`](/clr-api/mastersign-bench-downloader/) properly configured, according to the given Bench configuration. 

##### Parameter

* **config**  
  The Bench configuration.

##### Return Value
The created downloader instance.

#### LaunchApp(BenchConfiguration, BenchEnvironment, string, string) {#launchapp}
Launches the launcher executable of the specified app. 

##### Parameter

* **config**  
  The Bench configuration.
* **env**  
  The environment variables of the Bench system.
* **appId**  
  The ID of the app to launch.
* **args**  
  An array with additional command line arguments, to pass to the launcher executable.

##### Return Value
The  [`Process`](/clr-api/system-diagnostics-process/) object of the started executable.

#### DoSetupRequiredApps(IBenchManager, TaskInfo}, Cancelation) {#dosetuprequiredapps}
Runs the Bench task of setting up only the apps, required of the Bench system itself. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoAutoSetup(IBenchManager, TaskInfo}, Cancelation) {#doautosetup}
Runs the Bench task of automatically setting up all active apps including downloading missing resources, and setting up the environment afterwards. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoDownloadAppResources(IBenchManager, TaskInfo}, Cancelation) {#dodownloadappresources-b543e4cc}
Runs the Bench task of downloading the missing app resources of all active apps. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoDownloadAppResources(IBenchManager, string, TaskInfo}, Cancelation) {#dodownloadappresources-ddf8bec8}
Runs the Bench task of downloading the missing app resource of a specific app. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoDownloadAllAppResources(IBenchManager, TaskInfo}, Cancelation) {#dodownloadallappresources}
Runs the Bench task of downloading the missing app resources of all apps known to Bench, whether tey are active or not. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoDeleteAppResources(IBenchManager, TaskInfo}, Cancelation) {#dodeleteappresources-ff51455e}
Runs the Bench task of deleting all cached app resources. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoDeleteAppResources(IBenchManager, string, TaskInfo}, Cancelation) {#dodeleteappresources-4873ce60}
Runs the Bench task of deleting the cached app resource of a specific app. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoCleanUpAppResources(IBenchManager, TaskInfo}, Cancelation) {#docleanupappresources}
Runs the Bench task of deleting all cached app resources, not referenced by any app anymore. These app resources where typically downloaded before the definition of some apps where updated, now referencing to newer release versions. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoInstallApps(IBenchManager, TaskInfo}, Cancelation) {#doinstallapps-c48a583e}
Runs the Bench task of installing all active apps, not installed already. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoInstallApps(IBenchManager, string, TaskInfo}, Cancelation) {#doinstallapps-77146881}
Runs the Bench task of installing a specific app, including all of its dependencies. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoUninstallApps(IBenchManager, TaskInfo}, Cancelation) {#douninstallapps-29a29c35}
Runs the Bench task of uninstalling all installed apps. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoUninstallApps(IBenchManager, string, TaskInfo}, Cancelation) {#douninstallapps-e392ea67}
Runs the Bench task of uninstalling a specific app, including all apps, depending on the specified one. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoReinstallApps(IBenchManager, TaskInfo}, Cancelation) {#doreinstallapps-a96625db}
Runs the Bench task of uninstalling all installed apps and then installing all active apps. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoReinstallApps(IBenchManager, string, TaskInfo}, Cancelation) {#doreinstallapps-16b3e7f5}
Runs the Bench task of uninstalling a specific app and all apps, depending on it, and then installing the specified app. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoUpgradeApps(IBenchManager, TaskInfo}, Cancelation) {#doupgradeapps-745a9af5}
Runs the Bench task of upgrading all upgradable apps. An app is upgradable if  [`CanUpgrade`](/clr-api/mastersign-bench-appfacade/#canupgrade) is `true`. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoUpgradeApps(IBenchManager, string, TaskInfo}, Cancelation) {#doupgradeapps-d027e46e}
Runs the Bench task of upgrading a specific app. 

##### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.

#### DoUpdateEnvironment(IBenchManager, TaskInfo}, Cancelation) {#doupdateenvironment}
Runs the Bench task of setting up the environment, including the generation of `env.cmd`, (re)creating all launcher scripts and shortcuts, and running the custom environment scripts of all active apps. 

##### Parameter

* **man**  
  The Bench manager.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

##### Return Value
The result of running the task, in shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.


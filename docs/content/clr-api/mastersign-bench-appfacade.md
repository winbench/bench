---
title: "BenchLib - AppFacade"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## AppFacade Class
This class is a facade to the properties and the state of an app. It is initialized with the  [`IConfiguration`](/clr-api/mastersign-bench-iconfiguration/) object, holding the apps properties and the ID of the app. 

**Absolute Name:** `Mastersign.Bench.AppFacade`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [AppFacade(IConfiguration, string)](#ctor)

**Properties**

* [ID](#id)
* [Label](#label)
* [Category](#category)
* [Typ](#typ)
* [IsManagedPackage](#ismanagedpackage)
* [Version](#version)
* [IsVersioned](#isversioned)
* [Website](#website)
* [Docs](#docs)
* [Dependencies](#dependencies)
* [Responsibilities](#responsibilities)
* [IsActivated](#isactivated)
* [IsDeactivated](#isdeactivated)
* [IsRequired](#isrequired)
* [IsDependency](#isdependency)
* [Url](#url)
* [DownloadHeaders](#downloadheaders)
* [DownloadCookies](#downloadcookies)
* [ResourceFileName](#resourcefilename)
* [ResourceArchiveName](#resourcearchivename)
* [ResourceArchivePath](#resourcearchivepath)
* [ResourceArchiveTyp](#resourcearchivetyp)
* [Force](#force)
* [PackageName](#packagename)
* [Dir](#dir)
* [Exe](#exe)
* [SetupTestFile](#setuptestfile)
* [Path](#path)
* [Register](#register)
* [Environment](#environment)
* [AdornedExecutables](#adornedexecutables)
* [AdornmentProxyBasePath](#adornmentproxybasepath)
* [RegistryKeys](#registrykeys)
* [Launcher](#launcher)
* [LauncherExecutable](#launcherexecutable)
* [LauncherArguments](#launcherarguments)
* [LauncherIcon](#launchericon)
* [IsActive](#isactive)
* [HasResource](#hasresource)
* [CanCheckInstallation](#cancheckinstallation)
* [IsInstalled](#isinstalled)
* [IsResourceCached](#isresourcecached)
* [ShortStatus](#shortstatus)
* [LongStatus](#longstatus)
* [StatusIcon](#statusicon)
* [CanDownloadResource](#candownloadresource)
* [CanDeleteResource](#candeleteresource)
* [CanInstall](#caninstall)
* [CanUninstall](#canuninstall)
* [CanReinstall](#canreinstall)
* [CanUpgrade](#canupgrade)
* [ShouldBeInstalled](#shouldbeinstalled)
* [ShouldBeRemoved](#shouldberemoved)

**Methods**

* [LoadCachedValues()](#loadcachedvalues)
* [DiscardCachedValues()](#discardcachedvalues)
* [IsExecutableAdorned(string)](#isexecutableadorned)
* [GetExecutableProxy(string)](#getexecutableproxy)
* [Deactivate()](#deactivate)
* [SetupAutoConfiguration()](#setupautoconfiguration)
* [TrackResponsibilities()](#trackresponsibilities)
* [ToString()](#tostring)

### Constructors {#ctors}

#### AppFacade(IConfiguration, string) {#ctor}
Initializes a new instance of  [`AppFacade`](/clr-api/mastersign-bench-appfacade/). 

##### Parameter

* **source**  
  The configuration, containing the properties of the app.
* **appName**  
  The ID of the app.

### Properties {#properties}

#### ID {#id}
Gets the ID of the app. 

#### Label {#label}
Gets the label of the app. 

#### Category {#category}
Gets the category, this app belongs to. E.g. there are `Required` and `Optional` apps. 

#### Typ {#typ}
The typ of this app.

See for  [`AppTyps`](/clr-api/mastersign-bench-apptyps/) to compare and list the app typs.



#### IsManagedPackage {#ismanagedpackage}
Checks, if this app is a packaged managed by some kind of package manager. 

##### Value
`true` if this app is managed by a package manager; otherwise `false`.
#### Version {#version}
Gets the version string of the app, or `null` if the app has no specified version. 

##### Remarks
If the app has the version `"latest"` it is considered to have no specified version. 

##### See Also

* [Mastersign.Bench.AppFacade.IsVersioned](/clr-api/mastersign-bench-appfacade/#isversioned)

#### IsVersioned {#isversioned}
Checks, if this app has a specified version. 

#### Website {#website}
Gets the URL of the project or vendor website of this app, or `null` if no website was specified. 

#### Docs {#docs}
Gets a dictionary with labels and URLs for help and documentation. If an URL is relative, it is considered to be relative to the apps  [`Dir`](/clr-api/mastersign-bench-appfacade/#dir). 

#### Dependencies {#dependencies}
An array with app IDs which are necessary to be installed for this app to work. 

#### Responsibilities {#responsibilities}
An array of app IDs which depend on this app to be installed. 

#### IsActivated {#isactivated}
Checks, whether this app is marked as activated by the user, or not. 

##### Value
`true` if the apps ID is marked as activated by the user; otherwise `false`.
#### IsDeactivated {#isdeactivated}
Checks, whether this app is marked as deactivated by the user, or not. 

##### Value
`true` if the apps ID is marked as deactivated by the user; otherwise `false`.
#### IsRequired {#isrequired}
Checks, whether this app is required by the Bench system, or not. 

##### Value
`true` if the app is required by Bench; otherwise `false`.
#### IsDependency {#isdependency}
Checks, whether this app is dependency of another app. 

##### Value
`true` if the app is required by another app; otherwise `false`.
#### Url {#url}
Gets the URL of the apps resource, or `null` if the app has no downloadable resource. 

#### DownloadHeaders {#downloadheaders}
Gets a dictionary with HTTP header fields for the download request. 

#### DownloadCookies {#downloadcookies}
Gets a dictionary with HTTP cookies for the download request. 

#### ResourceFileName {#resourcefilename}
Gets the name of the apps file resource, or `null`in case the app has an archive resource or no downloadable resource at all. 

#### ResourceArchiveName {#resourcearchivename}
Gets the name of the apps archive resource, or `null`in case the app has a file resource or no downloadable resource at all. 

#### ResourceArchivePath {#resourcearchivepath}
Gets the sub path inside of the resource archive, or `null`in case the whole archive can be extracted or the app has no archive resource. 

#### ResourceArchiveTyp {#resourcearchivetyp}
Gets the typ of the resource archive, or `null` if the app has no archive resource. See  [`AppArchiveTyps`](/clr-api/mastersign-bench-apparchivetyps/) to compare or list the possible typs of an archive resource. 

#### Force {#force}
Gets a value, which specifies if the app will be installed even if it is already installed. 

#### PackageName {#packagename}
The name of the package represented by this app, or `null` in case  [`IsManagedPackage`](/clr-api/mastersign-bench-appfacade/#ismanagedpackage) is `false`. 

#### Dir {#dir}
The name of the target directory for this app. The target directory is the directory where the app resources are installed. 

#### Exe {#exe}
The relative path of the main executable file of the app, or `null`in case the app has no executable (e.g. the app is just a group). The path is relative to the target  [`Dir`](/clr-api/mastersign-bench-appfacade/#dir) of this app. 

#### SetupTestFile {#setuptestfile}
The relative path to a file, which existence can be used to check if the app is installed, or `null` e.g. in case the app is a package managed by a package manager. The path is relative to the target  [`Dir`](/clr-api/mastersign-bench-appfacade/#dir) of this app. 

#### Path {#path}
An array with relative or absolute paths, which will be added to the environment variable `PATH` when this app is activated. If a path is relative, it is relative to the target  [`Dir`](/clr-api/mastersign-bench-appfacade/#dir) of this app. 

##### See Also

* [Mastersign.Bench.AppFacade.Register](/clr-api/mastersign-bench-appfacade/#register)

#### Register {#register}
A flag to control if the  [`Path`](/clr-api/mastersign-bench-appfacade/#path)s of this app will be added to the environment variable `PATH`. 

#### Environment {#environment}
A dictionary with additional environment variables to setup, when this app is activated. 

#### AdornedExecutables {#adornedexecutables}
An array with paths to executables, which must be adorned. 

#### AdornmentProxyBasePath {#adornmentproxybasepath}
Gets the base path of the directory containing the adornmend proxy scripts for the executables of this app. 

#### RegistryKeys {#registrykeys}
An array with registry paths relative to the `HKCU` (current user) hive, which must be considered for registry isolation. 

#### Launcher {#launcher}
The label for the apps launcher, or `null` if the app has no launcher. 

#### LauncherExecutable {#launcherexecutable}
The path to the main executable, to be started by the apps launcher, or `null` if the app has no launcher. 

#### LauncherArguments {#launcherarguments}
An array with command line arguments to be sent to the  [`LauncherExecutable`](/clr-api/mastersign-bench-appfacade/#launcherexecutable)by the launcher. The last entry in this array must be `%*` if additional arguments shell be passed from the launcher to the executable. This is also necessary for drag-and-drop of files onto the launcher to work. 

#### LauncherIcon {#launchericon}
A path to an `*.ico` or `*.exe` file with the icon for the apps launcher, or `null` if the app has no launcher. 

#### IsActive {#isactive}
Checks, whether is app is active. An app can be active, because it was marked by the user to be activated, or because it is required by Bench or it is a dependency for another app. 

An app is **not active** if it was marked by the user as deactivated, regardless whether it is required by Bench or another app or marked as activated. 



#### HasResource {#hasresource}
Checks, whether this app has a downloadable app resource, or not. 

#### CanCheckInstallation {#cancheckinstallation}
Checks, whether the installation state of this app can be checked, or not. 

#### IsInstalled {#isinstalled}
Checks, whether this app is currently installed, or not. 

##### Remarks
This state is cached for performance reasons. To be shure to get the real current state, call  [`DiscardCachedValues()`](/clr-api/mastersign-bench-appfacade/#discardcachedvalues)upfront. 

##### See Also

* [Mastersign.Bench.AppFacade.DiscardCachedValues()](/clr-api/mastersign-bench-appfacade/#discardcachedvalues)
* [Mastersign.Bench.AppFacade.LoadCachedValues()](/clr-api/mastersign-bench-appfacade/#loadcachedvalues)

#### IsResourceCached {#isresourcecached}
Checks, whether this apps resource is currently cached, or not. 

Returns always `false`, if the apps  [`Typ`](/clr-api/mastersign-bench-appfacade/#typ) is not  [`Default`](/clr-api/mastersign-bench-apptyps/#default). Returns `true` if the apps  [`Typ`](/clr-api/mastersign-bench-appfacade/#typ) is  [`Default`](/clr-api/mastersign-bench-apptyps/#default), but the app has no downloadable resource. 



##### Remarks
This state is cached for performance reasons. To be shure to get the real current state, call  [`DiscardCachedValues()`](/clr-api/mastersign-bench-appfacade/#discardcachedvalues)upfront. 

##### See Also

* [Mastersign.Bench.AppFacade.DiscardCachedValues()](/clr-api/mastersign-bench-appfacade/#discardcachedvalues)
* [Mastersign.Bench.AppFacade.LoadCachedValues()](/clr-api/mastersign-bench-appfacade/#loadcachedvalues)

#### ShortStatus {#shortstatus}
Returns a short string, describing the overall state of the app. 

#### LongStatus {#longstatus}
Returns a string with a virtually complete description of the apps overall state. 

#### StatusIcon {#statusicon}
Returns a code for an icon, describing the overall state of this app. 

#### CanDownloadResource {#candownloadresource}
Checks, whether the app has a resource and the resource is not cached. 

#### CanDeleteResource {#candeleteresource}
Checks, whether the app has cached resource. 

#### CanInstall {#caninstall}
Checks, whether this app can be installed. 

#### CanUninstall {#canuninstall}
Checks, whether this app can be uninstalled. 

#### CanReinstall {#canreinstall}
Checks, whether this app can be reinstalled. 

#### CanUpgrade {#canupgrade}
Checks, whether this app can be upgraded to a more recent version. 

##### Remarks
This method does not check if there really is more recent version of this app. 

#### ShouldBeInstalled {#shouldbeinstalled}
Checks, whether this app is active (activated or required) but not installed. 

#### ShouldBeRemoved {#shouldberemoved}
Checks, whether this app is not activated or even deactivated but installed. 

### Methods {#methods}

#### LoadCachedValues() {#loadcachedvalues}
Does some expensive checks for the app and caches the result for later requests. These checks involve interaction with the file system. 

#### DiscardCachedValues() {#discardcachedvalues}
Clears cached values from the state of the app. If the state of an app was possibly changed, this method has to be called, to allow determining the new state from the file system and not just showing the last cached state. 

##### See Also

* [Mastersign.Bench.AppFacade.LoadCachedValues()](/clr-api/mastersign-bench-appfacade/#loadcachedvalues)

#### IsExecutableAdorned(string) {#isexecutableadorned}
Checks, whether an executable of this app is marked as adorned, or not. 

##### Parameter

* **exePath**  
  The path to the executable in question.

##### Return Value
`true` if the executable must be adorned, otherwise `false`.

#### GetExecutableProxy(string) {#getexecutableproxy}
Gets the path to the adornment wrapper script for a given executable fo this app. 

##### Parameter

* **exePath**  
  The path to the executable.

##### Return Value
The path to the adornment script.

#### Deactivate() {#deactivate}
Marks this app as deactivetd. 

#### SetupAutoConfiguration() {#setupautoconfiguration}
Make some implicit values and relationships explicit in the property values. 

#### TrackResponsibilities() {#trackresponsibilities}
Track down all apps, which refer to this app as a dependency and store the list app IDs in the app property `Responsibilities`. 

#### ToString() {#tostring}
Returns a string, containing the apps typ and ID. 

##### Return Value
A short string representation of the app.


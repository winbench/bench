---
title: "BenchLib - AppIndexFacade"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## AppIndexFacade Class
A collection of Bench apps.

This class provides a facade to handle the properties and the states of the bench apps in an object-oriented fashion.



**Absolute Name:** `Mastersign.Bench.AppIndexFacade`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [AppIndexFacade(IConfiguration)](#ctor)

**Properties**

* [Item[string]](#item)
* [ActiveApps](#activeapps)
* [InactiveApps](#inactiveapps)
* [RequiredApps](#requiredapps)
* [EnvironmentPath](#environmentpath)
* [Environment](#environment)

**Methods**

* [GetApps(String})](#getapps)
* [Exists(string)](#exists)
* [ByCategory(string)](#bycategory)
* [GetEnumerator()](#getenumerator)

### Constructors {#ctors}

#### AppIndexFacade(IConfiguration) {#ctor}
Initializes a new instance of  [`AppIndexFacade`](/clr-api/mastersign-bench-appindexfacade/). 

##### Parameter

* **appIndex**  
  An instance of  [`IConfiguration`](/clr-api/mastersign-bench-iconfiguration/) holding the configuration of Bench apps.

### Properties {#properties}

#### Item[string] {#item}
Gets an instance of  [`AppFacade`](/clr-api/mastersign-bench-appfacade/) for the specified app. 

##### Parameter

* **appName**  
  The ID of an app.

##### Return Value
The facade for the app, or `null`.

#### ActiveApps {#activeapps}
Gets an array with facades for all active apps. 

#### InactiveApps {#inactiveapps}
Gets an array with facades for all inactive apps. 

#### RequiredApps {#requiredapps}
Gets an array with facades for all apps required by Bench itself. 

#### EnvironmentPath {#environmentpath}
Gets an array with the paths for environment registration of all activated apps. 

#### Environment {#environment}
Gets a dictionary with the merged environment variables of all activated apps. That excludes the `PATH` environment variable, which is handeled separatly in  [`EnvironmentPath`](/clr-api/mastersign-bench-appindexfacade/#environmentpath). 

### Methods {#methods}

#### GetApps(String}) {#getapps}
Gets a collection with  [`AppFacade`](/clr-api/mastersign-bench-appfacade/) objects for multiple apps. 

##### Parameter

* **appNames**  
  An enumeration with app IDs.

##### Return Value
A collection with facades.

##### Remarks
If an app ID can not be found, a `null` is placed in the returned collection. Therefore, the returned collection has always the same number of items as the given enumeration. 

#### Exists(string) {#exists}
Checks whether an app ID exists in the app index. 

##### Parameter

* **appName**  
  The app ID.

##### Return Value
`true` if the app was found; otherwise `false`.

#### ByCategory(string) {#bycategory}
Gets all apps of a given category. 

##### Parameter

* **category**  
  The app category.

##### Return Value
An array with facades for all apps in the given category.

#### GetEnumerator() {#getenumerator}
Gets the facades for all apps in the index. 

##### Return Value
An enumerator with  [`AppFacade`](/clr-api/mastersign-bench-appfacade/) objects.


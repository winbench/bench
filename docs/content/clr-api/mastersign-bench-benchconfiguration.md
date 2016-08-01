---
title: "BenchLib - BenchConfiguration"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## BenchConfiguration Class
The merged configuration and app library for a Bench environment.

The configuration is merged by loading the following files: 



* **default**  
  `res\config.md`
* **custom**  
  `config\config.md`
* **site**  
  `bench-site.md` files (filename can be changed via default/custom config)

The app library is merged by loading the following files: 



* **default**  
  `res\apps.md`
* **custom**  
  `config\apps.md`



**Absolute Name:** `Mastersign.Bench.BenchConfiguration`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [BenchConfiguration(string)](#ctor-edef40b9)
* [BenchConfiguration(string, bool, bool, bool)](#ctor-5b37eb43)

**Fields**

* [DefaultAppCategory](#defaultappcategory)

**Properties**

* [BenchRootDir](#benchrootdir)
* [WithAppIndex](#withappindex)
* [WithCustomConfiguration](#withcustomconfiguration)
* [WithSiteConfiguration](#withsiteconfiguration)
* [Apps](#apps)

**Methods**

* [FindSiteConfigFiles()](#findsiteconfigfiles)
* [Reload()](#reload)

### Constructors {#ctors}

#### BenchConfiguration(string) {#ctor-edef40b9}
Initializes a new instance of  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/)loading all configuration and app library files. 

##### Parameter

* **benchRootDir**  
  The absolute path to the root directory of Bench.

#### BenchConfiguration(string, bool, bool, bool) {#ctor-5b37eb43}
Initializes a new instance of  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/)loading the specified set of configuration and app library files. 

##### Parameter

* **benchRootDir**  
  The absolute path to the root directory of Bench.
* **loadAppIndex**  
  A flag to control if app library files are going to be loaded.
* **loadCustomConfiguration**  
  A flag to control if custom configuration files are going to be loaded.
* **loadSiteConfiguration**  
  A flag to control if site configuration files are going to be loaded.

### Fields {#fields}

#### DefaultAppCategory {#defaultappcategory}
The property group category, which contains app definitions of required apps. 

### Properties {#properties}

#### BenchRootDir {#benchrootdir}
The absolute path to the root directory of Bench. 

#### WithAppIndex {#withappindex}
A flag which indicates if the app library was loaded during initialization of the  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/). 

#### WithCustomConfiguration {#withcustomconfiguration}
A flag which indicates if the custom configuration was loaded during initialization of the  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/). 

#### WithSiteConfiguration {#withsiteconfiguration}
A flag which indicates if the site configuration was loaded during the initialization of the  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/). 

#### Apps {#apps}
The merged definition of the Bench apps as a  [`AppIndexFacade`](/clr-api/mastersign-bench-appindexfacade/). 

### Methods {#methods}

#### FindSiteConfigFiles() {#findsiteconfigfiles}
Search for all existing site configuration files in the root directory of Bench and its parents. 

##### Return Value
An array with the absolute paths of the found site configuration files.

#### Reload() {#reload}
Reloads the set of configuration files, specified during construction. Call this method to create an updated instance of  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/)after one of the configuration files was changed. 

##### Return Value
A new instance of  [`BenchConfiguration`](/clr-api/mastersign-bench-benchconfiguration/), which has loaded the same set of configuration files as this instance.


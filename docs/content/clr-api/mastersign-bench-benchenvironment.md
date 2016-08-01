---
title: "BenchLib - BenchEnvironment"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## BenchEnvironment Class
This class represents all environment variables manipulated by the Bench system. 

It provides methods to load the environment variables in the current process, or write a batch file containing the variables for loading from another batch script. 



**Absolute Name:** `Mastersign.Bench.BenchEnvironment`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [BenchEnvironment(BenchConfiguration)](#ctor)

**Methods**

* [Load(DictionaryEntryHandler)](#load-edc43ab4)
* [Load()](#load-830f8d1c)
* [Load(String, String})](#load-68646687)
* [Load(StringDictionary)](#load-f0858848)
* [WriteEnvironmentFile()](#writeenvironmentfile)
* [RegisterInUserProfile()](#registerinuserprofile)
* [UnregisterFromUserProfile()](#unregisterfromuserprofile)

### Constructors {#ctors}

#### BenchEnvironment(BenchConfiguration) {#ctor}
Initializes a new instance of  [`BenchEnvironment`](/clr-api/mastersign-bench-benchenvironment/). 

##### Parameter

* **config**  
  The configuration of the Bench system.

### Methods {#methods}

#### Load(DictionaryEntryHandler) {#load-edc43ab4}
Calls the given handler for every environment variable in the configuration. 

##### Parameter

* **set**  
  The handler for an individual variable.

#### Load() {#load-830f8d1c}
Loads the environment variables in the current process. 

#### Load(String, String}) {#load-68646687}
Stores the environment variables in the given generic dictionary. 

##### Parameter

* **dict**  
  A dictionary with the variable names as keys.

#### Load(StringDictionary) {#load-f0858848}
Stores the environment variables in the given string dictionary. 

##### Parameter

* **dict**  
  A dictionary with the variable names as keys.

#### WriteEnvironmentFile() {#writeenvironmentfile}
Writes the environment file of the Bench system. 

##### Remarks
The environment file is stored in the root of the Bench directory structure and called `env.cmd`. 

#### RegisterInUserProfile() {#registerinuserprofile}
Registers the Bench environment in the Windows user profile. 

Stores the following environment variables are set or updated in the user profile: 



* `BENCH_VERSION`
* `BENCH_HOME`
* `BENCH_PATH`
* `PATH` is changed by adding `%BENCH_PATH%`



#### UnregisterFromUserProfile() {#unregisterfromuserprofile}
Unregisters the Bench environment from the Windows user profile. 

The following environment variables are deleted or updated in the user profile: 



* `BENCH_VERSION`
* `BENCH_HOME`
* `BENCH_PATH`
* `PATH` is changed by removing `%BENCH_PATH%`




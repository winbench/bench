---
title: "BenchLib - ActionResult"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ActionResult Class
This class represents the result of a Bench task. 

**Absolute Name:** `Mastersign.Bench.ActionResult`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [ActionResult(TaskInfo}, bool)](#ctor)

**Properties**

* [Success](#success)
* [Canceled](#canceled)
* [AffectedApps](#affectedapps)
* [Infos](#infos)
* [Errors](#errors)

### Constructors {#ctors}

#### ActionResult(TaskInfo}, bool) {#ctor}
Initializes a new instance of  [`ActionResult`](/clr-api/mastersign-bench-actionresult/). 

##### Parameter

* **infos**  
  An enumeration of  [`TaskInfo`](/clr-api/mastersign-bench-taskinfo/) objects or `null`.
* **canceled**  
  A flag, indicating if the task execution was canceled.

### Properties {#properties}

#### Success {#success}
Is `true` if the execution of the task was successful. 

#### Canceled {#canceled}
Is `true` if the execution of the task was canceled. 

#### AffectedApps {#affectedapps}
An array with the IDs of all apps affected by the executed task. 

#### Infos {#infos}
A number of info objects created during the task execution. The info objects can be progress messages, infos or errors. 

#### Errors {#errors}
A filtered enumeration, containing only errors from  [`Infos`](/clr-api/mastersign-bench-actionresult/#infos). 


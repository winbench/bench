---
title: "BenchLib - TaskError"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## TaskError Class
This class represents an error which occurred during the execution of a Bench task. 

**Absolute Name:** `Mastersign.Bench.TaskError`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [TaskError(string, string, string, Exception)](#ctor)

**Properties**

* [Exception](#exception)

### Constructors {#ctors}

#### TaskError(string, string, string, Exception) {#ctor}
Initializes a new instance of  [`TaskError`](/clr-api/mastersign-bench-taskerror/). 

##### Parameter

* **message**  
  A short message describing the error.
* **appId**  
  Th ID of the affected app or `null`.
* **detailedMessage**  
  A detailed message describing the error or `null`.
* **exception**  
  The execption that caused the error or `null`.

### Properties {#properties}

#### Exception {#exception}
The exception causing the error, or `null`. 


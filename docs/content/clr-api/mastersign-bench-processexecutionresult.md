---
title: "BenchLib - ProcessExecutionResult"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ProcessExecutionResult Class
This class represents the result from a Windows process execution. 

**Absolute Name:** `Mastersign.Bench.ProcessExecutionResult`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0

### See Also

* [Mastersign.Bench.IProcessExecutionHost](/clr-api/mastersign-bench-iprocessexecutionhost/)


### Overview
**Constructors**

* [ProcessExecutionResult(int, string)](#ctor-76050ab9)
* [ProcessExecutionResult(int)](#ctor-8b7228ee)

**Properties**

* [ExitCode](#exitcode)
* [Output](#output)

### Constructors {#ctors}

#### ProcessExecutionResult(int, string) {#ctor-76050ab9}
Initializes a new instance of  [`ProcessExecutionResult`](/clr-api/mastersign-bench-processexecutionresult/). 

##### Parameter

* **exitCode**  
  The exit code of the process.
* **output**  
  The output of the process decoded as a string.

#### ProcessExecutionResult(int) {#ctor-8b7228ee}
Initializes a new instance of  [`ProcessExecutionResult`](/clr-api/mastersign-bench-processexecutionresult/). 

##### Parameter

* **exitCode**  
  The exit code of the process.

### Properties {#properties}

#### ExitCode {#exitcode}
The exit code of the process. If this value is `0`, he process is considered to be successfull. 

#### Output {#output}
The process output as string, or `null`. 


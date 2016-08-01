---
title: "BenchLib - IProcessExecutionHost"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IProcessExecutionHost Interface
This interface describes the capability to execute Windows processes in a synchronous or asynchronous fashion. 

**Absolute Name:** `Mastersign.Bench.IProcessExecutionHost`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [StartProcess(BenchEnvironment, string, string, string, ProcessExitCallback, ProcessMonitoring)](#startprocess)
* [RunProcess(BenchEnvironment, string, string, string, ProcessMonitoring)](#runprocess)

### Methods {#methods}

#### StartProcess(BenchEnvironment, string, string, string, ProcessExitCallback, ProcessMonitoring) {#startprocess}
Starts a Windows process in an asynchronous fashion. 

##### Parameter

* **env**  
  The environment variables of Bench.
* **cwd**  
  The working directory, to start the process in.
* **executable**  
  The path to the executable.
* **arguments**  
  The string with the command line arguments.
* **cb**  
  The handler method to call when the execution of the process finishes.
* **monitoring**  
  A flag to control the level of monitoring.

##### See Also

* [Mastersign.Bench.CommandLine.FormatArgumentList(System.String)](/clr-api/mastersign-bench-commandline/#formatargumentlist)

#### RunProcess(BenchEnvironment, string, string, string, ProcessMonitoring) {#runprocess}
Starts a Windows process in a synchronous fashion. 

##### Parameter

* **env**  
  The environment variables of Bench.
* **cwd**  
  The working directory, to start the process in.
* **executable**  
  The path to the executable.
* **arguments**  
  The string with the command line arguments.
* **monitoring**  
  A flag to control the level of monitoring.

##### Return Value
An instance of  [`ProcessExecutionResult`](/clr-api/mastersign-bench-processexecutionresult/) with the exit code and optionally the output of the process.

##### See Also

* [Mastersign.Bench.CommandLine.FormatArgumentList(System.String)](/clr-api/mastersign-bench-commandline/#formatargumentlist)


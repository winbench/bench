---
title: "BenchLib - ProcessExecutionFailedException"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ProcessExecutionFailedException Class
This exception class is used to describe a failed process execution. This class contains the command line, which started the process, the exit code of the failed process and optionally the process output. 

**Absolute Name:** `Mastersign.Bench.ProcessExecutionFailedException`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [ProcessExecutionFailedException(string, string, int, string)](#ctor)

**Properties**

* [CommandLine](#commandline)
* [ExitCode](#exitcode)
* [ProcessOutput](#processoutput)

### Constructors {#ctors}

#### ProcessExecutionFailedException(string, string, int, string) {#ctor}
Initializes a new instance of  [`ProcessExecutionFailedException`](/clr-api/mastersign-bench-processexecutionfailedexception/). 

##### Parameter

* **message**  
  The error message, associated with the failing process.
* **commandLine**  
  The command line which started the process.
* **exitCode**  
  The exit code from the process.
* **processOutput**  
  The process output or `null`.

### Properties {#properties}

#### CommandLine {#commandline}
The command line, which started the process. 

#### ExitCode {#exitcode}
The exit code of the process. 

#### ProcessOutput {#processoutput}
The output of the process, or `null`. 


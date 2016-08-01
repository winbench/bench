---
title: "BenchLib - LogLevels"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## LogLevels Class
This static class knows the different possible log levels, while running Bench tasks. It helps to recognize a log level from a string. 

**Absolute Name:** `Mastersign.Bench.LogLevels`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Fields**

* [None](#none)
* [Info](#info)
* [Error](#error)

**Methods**

* [GuessLevel(string)](#guesslevel)

### Fields {#fields}

#### None {#none}
Log nothing. 

#### Info {#info}
Log errors and informational messages. 

#### Error {#error}
Only log error messages. 

### Methods {#methods}

#### GuessLevel(string) {#guesslevel}
Try to recognize a known log level in the given string. 

##### Parameter

* **value**  
  The string, which possibly describes a log level.

##### Return Value
One of the known log levels.


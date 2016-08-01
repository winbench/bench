---
title: "BenchLib - CommandLine"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## CommandLine Class
This static class provides a number of methods to deal with command line arguments. 

**Absolute Name:** `Mastersign.Bench.CommandLine`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [SubstituteArgumentList(string, string)](#substituteargumentlist)
* [SubstituteArgument(string, string)](#substituteargument)
* [FormatArgumentList(string)](#formatargumentlist)
* [EscapeArgument(string, bool)](#escapeargument)

### Methods {#methods}

#### SubstituteArgumentList(string, string) {#substituteargumentlist}
Given an array of strings, containing batch style placeholders for environment variables and numbered parameters, the placeholders are replaced by the referred parameter values. 

##### Parameter

* **values**  
  A number of strings possibly with placeholders for environment varibales and numbered parameters. Environment variables are written as `%NAME%`, and numbered parameters are written as `%x`, with `x` beeing a digit from `0` to `9`. 
* **parameters**  
  An array with ordered parameter strings.

##### Return Value
An array with the substituted strings.

##### Example
Given the environment variable `MY_DIR` with a value of `C:\my-dir`, and an array with `["a", "b"]` for  `parameters`, the array with `["-out", "%MY_DIR%\result", "-msg", "%2 + %1 = ?"]`for  `values` is substituted into the following array: `["-out", "C:\my-dir\result", "-msg", "b + a = ?"]`. 

#### SubstituteArgument(string, string) {#substituteargument}
Given a string, containing batch style placeholders for environment variables and numbered parameters, the placeholders are replaced by the referred parameter values. 

##### Parameter

* **value**  
  A string possibly with placeholders for environment varibales and numbered parameters. Environment variables are written as `%NAME%`, and numbered parameters are written as `%x`, with `x` beeing a digit from `0` to `9`. 
* **parameters**  
  An array with ordered parameter strings.

##### Return Value
An array with the substituted strings.

##### Example
Given the environment variable `MY_DIR` with a value of `C:\my-dir`, and an array with `["a", "b"]` for  `parameters`, a string with `"-out %MY_DIR%\%1-%2.txt"`for  `value` is substituted into the following result: `"-out C:\my-dir\a-b.txt"`. 

#### FormatArgumentList(string) {#formatargumentlist}
Creates a string with mutlitple command line arguments, compatible with the Windows CMD interpreter. 

All characters of the given strings are preserved and if necessary escaped. 



##### Parameter

* **args**  
  Multiple arguments.

##### Return Value
A command line argument string, containing all given arguments.

#### EscapeArgument(string, bool) {#escapeargument}
Escapes an argument string for the Windows CMD interpreter. If the string contains special cahracters or white space, it is quoted. 

##### Parameter

* **arg**  
  The argument string.
* **alwaysQuote**  
  `true` if the string will be quoted regardless of quoting is necessary; otherwise `false`.

##### Return Value
A string which can be safely passed to the CMD interpreter as an argument.

##### Remarks
Usually you will use  [`FormatArgumentList(string)`](/clr-api/mastersign-bench-commandline/#formatargumentlist) instead of calling this method directly.


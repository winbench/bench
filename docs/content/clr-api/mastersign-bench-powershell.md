---
title: "BenchLib - PowerShell"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## PowerShell Class
A static class, containing some helper properties and methods, for dealing with the PowerShell. 

**Absolute Name:** `Mastersign.Bench.PowerShell`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Properties**

* [Executable](#executable)

**Methods**

* [FormatStringList(string)](#formatstringlist)

### Properties {#properties}

#### Executable {#executable}
An absolute path to the PowerShell executable. 

### Methods {#methods}

#### FormatStringList(string) {#formatstringlist}
Formats the given strings as a string array in PowerShell syntax. 

##### Parameter

* **args**  
  The strings, to be formatted as a PowerShell array.

##### Return Value
A string with the formatted values.

##### Example
The call `PowerShell.FormatString("ab", "cd ef", gh")` yields the result string `"@(\"ab\", \"cd ef\", \"gh\")"`. 


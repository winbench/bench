---
title: "BenchLib - ClrInfo"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ClrInfo Class
This static class contains methods to investigate the installed .NET framework versions. 

**Absolute Name:** `Mastersign.Bench.Windows.ClrInfo`  
**Namespace:** Mastersign.Bench.Windows  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [IsVersionSupported(Version)](#isversionsupported)
* [GetInstalledVersions()](#getinstalledversions)

### Methods {#methods}

#### IsVersionSupported(Version) {#isversionsupported}
Checks whether the given .NET framework version is supported and installed under the current Windows system. 

##### Parameter

* **v**  
  The .NET framework version required.

##### Return Value
`true` if the installed .NET framework can support the requested .NET framework version; othwise `false`.

#### GetInstalledVersions() {#getinstalledversions}
Gets the version number of all installed .NET framework versions. 

##### Return Value
An array with the version numbers.


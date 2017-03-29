+++
title = "MinGW"
weight = 45
app_library = "default"
app_category = "Languages and Platforms"
app_typ = "meta"
app_ns = "Bench"
app_id = "Bench.MinGW"
app_version = "0.6.2"
app_categories = ["Languages and Platforms"]
app_libraries = ["default"]
app_types = ["meta"]
+++

**ID:** `Bench.MinGW`  
**Version:** 0.6.2  
<!--more-->

[Back to all apps](/apps/)

## Description
MinGW provides a GNU development environment for Windows,
including compilers for C/C++, Objective-C, Fortran, Ada, ...


You can adapt the preselected MinGW packages
by putting something like this in your user app library in `config\apps.md`:

```Markdown
* ID: `Bench.MinGW`
* Packages:
    + `mingw32-base`
    + `mingw32-gcc-g++`
    + `mingw32-autotools`
    + `msys-bash`
    + `msys-grep`
```

After the automatic setup by _Bench_, you can use the launcher shortcut
_MinGW Package Manager_ to start the GUI for _MinGW Get_
and install more MinGW packages.

## Source

* Library: [`default`](/app_libraries/default)
* Category: [Languages and Platforms](/app_categories/languages-and-platforms)
* Order Index: 45

## Properties

* Namespace: Bench
* Name: MinGW
* Typ: `meta`
* Website: <http://www.mingw.org/>
* Dependencies: [MinGwGet](/apps/Bench.MinGwGet), [MinGwGetGui](/apps/Bench.MinGwGetGui)
* Responsibilities: [C/C++ Development](/apps/Bench.Group.CppDevelopment)


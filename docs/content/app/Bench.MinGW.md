+++
title = "MinGW"
weight = 30
app_lib = "default"
app_category = "Platforms and Programming Languages"
app_ns = "Bench"
app_id = "Bench.MinGW"
app_version = "0.6.2"
+++

**ID:** `Bench.MinGW`  
**Version:** 0.6.2  
<!--more-->

## Description
(http://www.mingw.org/) provides a GNU development environment for Windows, including compilers for C/C++, Objective-C, Fortran, Ada, ...

The MinGW package manager MinGW Get:


Graphical user interface for MinGW Get:


Meta app MinGW with package manager and graphical user interface:


You can adapt the preselected MinGW packages by putting something like this in your `config\config.md`:

```Markdown

## Source

* Library: `default`
* Category: Platforms and Programming Languages
* Order Index: 30

## Properties

* Namespace: Bench
* Name: MinGW
* Typ: `meta`
* Website: <http://www.mingw.org/>
* Dependencies: [MinGwGet](/app/Bench.MinGwGet), [MinGwGetGui](/app/Bench.MinGwGetGui)


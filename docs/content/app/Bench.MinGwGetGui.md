+++
date = "2017-01-06T16:00:10+01:00"
title = "MinGwGetGui"
weight = 29
app_lib = "default"
app_category = "Platforms and Programming Languages"
app_ns = "Bench"
app_id = "Bench.MinGwGetGui"
app_version = "0.6.2"
+++

**ID:** `Bench.MinGwGetGui`  
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
* Order Index: 29

## Properties

* Namespace: Bench
* Name: MinGwGetGui
* Typ: `default`
* Dependencies: [MinGwGet](/app/Bench.MinGwGet)
* Responsibilities: [MinGW](/app/Bench.MinGW)


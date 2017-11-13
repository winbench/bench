+++
title = "LLVM Clang"
weight = 47
app_library = "default"
app_category = "Languages and Platforms"
app_typ = "default"
app_ns = "Bench"
app_id = "Bench.Clang"
app_version = "5.0.0"
app_categories = ["Languages and Platforms"]
app_libraries = ["default"]
app_types = ["default"]
+++

**ID:** `Bench.Clang`  
**Version:** 5.0.0  
<!--more-->

[Back to all apps](/apps/)

## Description
The Clang compiler can act as drop-in replacement for the GCC compilers.

This app sets the environment variables `CC` and `CXX` to inform _CMake_
about the C/C++ compiler path. Therefore, if you build your C/C++ projects
with _CMake_, it is sufficient to just activate the _Clang_ app and _CMake_
will use _Clang_ instead of the GCC compiler from _MinGW_.

If you want to use the Clang compiler with Eclipse, you must manually
install the LLVM-Plugin for Eclipse CDT.

## Source

* Library: [`default`](/app_libraries/default)
* Category: [Languages and Platforms](/app_categories/languages-and-platforms)
* Order Index: 47

## Properties

* Namespace: Bench
* Name: Clang
* Typ: `default`
* Website: <http://clang.llvm.org/>


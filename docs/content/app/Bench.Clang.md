+++
title = "LLVM Clang"
weight = 31
app_lib = "default"
app_category = "Platforms and Programming Languages"
app_ns = "Bench"
app_id = "Bench.Clang"
app_version = "3.8.1"
+++

**ID:** `Bench.Clang`  
**Version:** 3.8.1  
<!--more-->

## Description
The Clang compiler can act as drop-in replacement for the GCC compilers.

This app sets the environment variables `CC` and `CXX` to inform _CMake_
about the C/C++ compiler path. Therefore, if you build your C/C++ projects
with _CMake_, it is sufficient to just activate the _Clang_ app and _CMake_
will use _Clang_ instead of the GCC compiler from _MinGW_.

If you want to use the Clang compiler with Eclipse, you must manually
install the LLVM-Plugin for Eclipse CDT.

## Source

* Library: `default`
* Category: Platforms and Programming Languages
* Order Index: 31

## Properties

* Namespace: Bench
* Name: Clang
* Typ: `default`
* Website: <http://clang.llvm.org/>


+++
title = "CMake"
weight = 53
app_lib = "default"
app_category = "Software Development Utilities"
app_ns = "Bench"
app_id = "Bench.CMake"
app_version = "3.6.1"
+++

**ID:** `Bench.CMake`  
**Version:** 3.6.1  
<!--more-->

## Description
CMake is an open-source, cross-platform family of tools designed to build,
test and package software. CMake is used to control the software compilation process
using simple platform and compiler independent configuration files, and generate native
makefiles and workspaces that can be used in the compiler environment of your choice.
The suite of CMake tools were created by Kitware in response to the need for a powerful,
cross-platform build environment for open-source projects such as ITK and VTK.

Usually you want to use this app with _MinGW_.

To setup a C/C++ project with CMake and MinGW (`mingw32-make`), you have to activate the _MinGW_ app with the `mingw32-make` package.
Setup your project with a `CMakeLists.txt` file and run `cmake -G "MinGW Makefiles" <project folder>` to generate the `Makefile`. Run `cmake --build <project folder>` to compile the project.

## Source

* Library: `default`
* Category: Software Development Utilities
* Order Index: 53

## Properties

* Namespace: Bench
* Name: CMake
* Typ: `default`
* Website: <https://cmake.org/>


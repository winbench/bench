---
title: "BenchLib - FileSystem"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## FileSystem Class
A collection of static methods to help with file system operations. 

**Absolute Name:** `Mastersign.Bench.FileSystem`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Fields**

* [Default](#default)
* [Maximized](#maximized)
* [Minimized](#minimized)

**Properties**

* [WindowsScriptingHostShell](#windowsscriptinghostshell)

**Methods**

* [EmptyDir(string)](#emptydir)
* [AsureDir(string)](#asuredir)
* [PurgeDir(string)](#purgedir)
* [MoveContent(string, string)](#movecontent)
* [CreateShortcut(string, string, string, string, string, ShortcutWindowStyle)](#createshortcut)

### Fields {#fields}

#### Default {#default}
The created window is in normal state. 

#### Maximized {#maximized}
The created window is request to be maximized. 

#### Minimized {#minimized}
The created window is request to be minimized. 

### Properties {#properties}

#### WindowsScriptingHostShell {#windowsscriptinghostshell}
Returns an instance of the COM object `WshShell`. 

### Methods {#methods}

#### EmptyDir(string) {#emptydir}
Makes sure, the given path references an empty directory. If the directory does not exist, it is created, including missing parent folders. If the directory exists, all content is recursively deleted. 

##### Parameter

* **path**  
  An path to the directory.

##### Return Value
A path to the directory.

#### AsureDir(string) {#asuredir}
Makes sure a directory exists. Creates it, if it does not exist yet. 

##### Parameter

* **path**  
  A path to the directory.

##### Return Value
A path to the directory.

#### PurgeDir(string) {#purgedir}
Deletes a directory and all of its content. 

##### Parameter

* **path**  
  A path to the directory.

#### MoveContent(string, string) {#movecontent}
Moves all the content from one directory to another. 

##### Parameter

* **sourceDir**  
  A path to the source directory.
* **targetDir**  
  A path to the target directory.

#### CreateShortcut(string, string, string, string, string, ShortcutWindowStyle) {#createshortcut}
Creates a Windows shortcut, or link respectively. 

##### Parameter

* **file**  
  A path to the shortcut file (`*.lnk`).
* **target**  
  A path to the target file of the shortcut.
* **arguments**  
  An command line argument string to pass to the target file, or `null`.
* **workingDir**  
  A path to the working directory to run the target file in, or `null`.
* **iconPath**  
  A path to the icon for the shortcut (`*.exe` or `*.ico`), or `null`.
* **windowStyle**  
  The window style to start the target file with, or `null`.


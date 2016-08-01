---
title: "BenchLib - ActivationFile"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ActivationFile Class
Represents a text file with a list of app IDs.

The syntax of the text file follows the following rules: 



* Empty lines are ignored.
* Lines with nothing but white space are ignored.
* White space at the beginning and the end of lines is trimmed.
* Lines starting with `#` are ignored.
* The first word (contiguous non white space) in a line is considered to be an app ID.
* Additional characters after the first word are ignored, and can be used to commment the entry.

### Example
A text file represented by this class could look like this: 

```
# --- Activated Apps --- #

AppA
AppB (this app has a comment)
 AppC (this app ID is valid, despite the fact, that it is indended)

# AppD (this app is not activated, because the line is commented out)
AppE some arbitrary comment
```




**Absolute Name:** `Mastersign.Bench.ActivationFile`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [ActivationFile(string)](#ctor)

**Methods**

* [SignIn(string)](#signin)
* [SignOut(string)](#signout)
* [GetEnumerator()](#getenumerator)

### Constructors {#ctors}

#### ActivationFile(string) {#ctor}
Initializes a new instance of  [`ActivationFile`](/clr-api/mastersign-bench-activationfile/). 

##### Parameter

* **path**  
  An absolute path to the text file.

### Methods {#methods}

#### SignIn(string) {#signin}
Makes shure, the given app ID is listed active.  
The text file is updated immediately. 

##### Parameter

* **id**  
  An app ID. Must be a string without whitespace.

##### Remarks
If the given app ID is already listed, but commented out, the commenting `#` is removed. If the given app ID is not listed, it is added at the end of the file. 

#### SignOut(string) {#signout}
Makes shure, the given app ID is not listed active.  
The text file is updated immediately. 

##### Parameter

* **id**  
  An app ID. Must be a string without whitespace.

##### Remarks
If the given app ID is not listed, or commented out, the text file is not changed. If the given app ID is listed and not commented out, its line is prepended with a `# ` to comment it out. 

#### GetEnumerator() {#getenumerator}
Returns all app IDs listed as active. 

##### Return Value
An enumerator of strings.


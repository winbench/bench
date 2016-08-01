---
title: "BenchLib - TaskProgress"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## TaskProgress Class
This class represents a progress update of a Bench task. 

**Absolute Name:** `Mastersign.Bench.TaskProgress`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [TaskProgress(string, float, string, string)](#ctor)

**Properties**

* [Progress](#progress)

**Methods**

* [ScaleProgress(float, float)](#scaleprogress)

### Constructors {#ctors}

#### TaskProgress(string, float, string, string) {#ctor}
Initializes a new instance of  [`TaskProgress`](/clr-api/mastersign-bench-taskprogress/). 

##### Parameter

* **message**  
  A short message describing the event.
* **progress**  
  The new task progress.
* **appId**  
  Th ID of the affected app or `null`.
* **detailedMessage**  
  A detailed message describing the event or `null`.

### Properties {#properties}

#### Progress {#progress}
The new progress value int the interval between `0.0` and `1.0`. 

### Methods {#methods}

#### ScaleProgress(float, float) {#scaleprogress}
Projects the progress value of this progress info into a sub-interval. 

##### Parameter

* **globalBase**  
  The offset of the sub-interval.
* **factor**  
  The scale factor for the progress value.

##### Return Value
A new progress info with the scaled progress value.


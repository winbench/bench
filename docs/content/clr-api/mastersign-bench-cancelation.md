---
title: "BenchLib - Cancelation"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## Cancelation Class
This class represents a possible cancelation of a running task.

Using the method  [`Cancel()`](/clr-api/mastersign-bench-cancelation/#cancel), the running task can be cancelled. And with the property  [`IsCanceled`](/clr-api/mastersign-bench-cancelation/#iscanceled) can be checked whether the task was canceled. 



**Absolute Name:** `Mastersign.Bench.Cancelation`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0

### Remarks
This class is **not thead safe**.



### Overview
**Events**

* [Canceled](#canceled)

**Properties**

* [IsCanceled](#iscanceled)

**Methods**

* [Cancel()](#cancel)

### Events {#events}

#### Canceled {#canceled}
This event is fired, when the related task is cancelled. 

### Properties {#properties}

#### IsCanceled {#iscanceled}
Checks whether the related task is cancelled or not. 

### Methods {#methods}

#### Cancel() {#cancel}
Requests the cancelation of the related task. 

##### Remarks
This method can be called multiple times. 


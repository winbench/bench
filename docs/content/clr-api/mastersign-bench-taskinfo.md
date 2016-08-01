---
title: "BenchLib - TaskInfo"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## TaskInfo Class
This class represents an information about a Bench task. This class is the base class for a couple of specialized info types. 



*  [`TaskProgress`](/clr-api/mastersign-bench-taskprogress/)
*  [`TaskError`](/clr-api/mastersign-bench-taskerror/)



**Absolute Name:** `Mastersign.Bench.TaskInfo`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [TaskInfo(string, string, string)](#ctor)

**Properties**

* [Timestamp](#timestamp)
* [AppId](#appid)
* [Message](#message)
* [DetailedMessage](#detailedmessage)

### Constructors {#ctors}

#### TaskInfo(string, string, string) {#ctor}
Initializes a new instance of  [`TaskInfo`](/clr-api/mastersign-bench-taskinfo/). 

##### Parameter

* **message**  
  A short message desccribing the event.
* **appId**  
  The ID of the afected app or `null`.
* **detailedMessage**  
  A detailed message describing the event or `null`.

### Properties {#properties}

#### Timestamp {#timestamp}
The point in time, when this info was created. 

#### AppId {#appid}
The ID of the affected app, or `null`. 

#### Message {#message}
A short message describing the event. 

#### DetailedMessage {#detailedmessage}
A detailed message describing the event, or `null`. 


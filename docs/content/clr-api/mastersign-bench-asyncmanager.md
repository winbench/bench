---
title: "BenchLib - AsyncManager"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## AsyncManager Class
This static class contains convenience methods for handling threads. 

**Absolute Name:** `Mastersign.Bench.AsyncManager`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [StartTask(ThreadStart)](#starttask)

### Methods {#methods}

#### StartTask(ThreadStart) {#starttask}
Create a new thread with the given  [`ThreadStart`](/clr-api/system-threading-threadstart/) and start it immediately. 

##### Parameter

* **action**  
  The action to run in the created thread.

##### Return Value
The new  [`Thread`](/clr-api/system-threading-thread/) instance.


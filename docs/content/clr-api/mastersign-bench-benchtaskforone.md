---
title: "BenchLib - BenchTaskForOne"
categeories:
  - ".NET API"
  - "Delegate"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## BenchTaskForOne Delegate
The type of a method, executing a Bench task for one Bench app. 

**Absolute Name:** `Mastersign.Bench.BenchTaskForOne`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0

### Parameter

* **man**  
  The Bench manager.
* **appId**  
  The ID of the targeted app.
* **notify**  
  The notification handler.
* **cancelation**  
  A cancelation token.

### Return Value
The result of the executed task in the shape of an  [`ActionResult`](/clr-api/mastersign-bench-actionresult/) object.



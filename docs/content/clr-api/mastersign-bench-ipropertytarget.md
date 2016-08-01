---
title: "BenchLib - IPropertyTarget"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IPropertyTarget Interface
This interace describes the capability of storing property values. The properties are name with a unique strings. 

**Absolute Name:** `Mastersign.Bench.IPropertyTarget`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [SetValue(string, Object)](#setvalue)

### Methods {#methods}

#### SetValue(string, Object) {#setvalue}
Sets the value of the specified property. If the property did exist until now, it is created. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new value of the property.


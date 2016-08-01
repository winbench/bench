---
title: "BenchLib - IGroupedValueResolver"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IGroupedValueResolver Interface
This interface describes an object, that can resolve or transform a the values of group properties. 

**Absolute Name:** `Mastersign.Bench.IGroupedValueResolver`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [ResolveGroupValue(string, string, Object)](#resolvegroupvalue)

### Methods {#methods}

#### ResolveGroupValue(string, string, Object) {#resolvegroupvalue}
Returns the resolved or transformed value of the specified property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The original value of the specified property.

##### Return Value
The resolved or transformed value for the specified value.


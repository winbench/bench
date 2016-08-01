---
title: "BenchLib - IPropertySource"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IPropertySource Interface
This interface describes the capability of reading properties. The properties are named with a unique string. 

**Absolute Name:** `Mastersign.Bench.IPropertySource`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [GetValue(string)](#getvalue)
* [CanGetValue(string)](#cangetvalue)

### Methods {#methods}

#### GetValue(string) {#getvalue}
Gets the value of the specified property. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
The value of the specified property, or `null` if the property does not exist.

#### CanGetValue(string) {#cangetvalue}
Checks, whether this object can retrieve the value for the specified property, or not. 

##### Parameter

* **name**  
  The name of the property in question.

##### Return Value
`true` if this object can get the value for specified property; otherwise `false`.

##### Remarks
Even when this method returns `true`, it may be the case, that  [`GetValue(string)`](/clr-api/mastersign-bench-ipropertysource/#getvalue) returns `null`, because the property exists, but the value of the property is `null`. 


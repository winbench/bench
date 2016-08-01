---
title: "BenchLib - IPropertyCollection"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IPropertyCollection Interface
This interface describes an object, which stores properties. It is a combination of  [`IPropertySource`](/clr-api/mastersign-bench-ipropertysource/) and  [`IPropertyTarget`](/clr-api/mastersign-bench-ipropertytarget/). 

**Absolute Name:** `Mastersign.Bench.IPropertyCollection`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [PropertyNames()](#propertynames)
* [GetValue(string, Object)](#getvalue)
* [ContainsValue(string)](#containsvalue)

### Methods {#methods}

#### PropertyNames() {#propertynames}
Gets the names from all existing properties. 

##### Return Value
An enumeration of strings.

#### GetValue(string, Object) {#getvalue}
Gets the value of the specified property, or a given default value, in case the specified property does not exist. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
The value of the specified property, or  `def`in case the specified value does not exist.

#### ContainsValue(string) {#containsvalue}
Checks, whether this collection contains the specified property. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
`true` if the property exists; otherwise `false`.

##### See Also

* [Mastersign.Bench.IPropertySource.CanGetValue(System.String)](/clr-api/mastersign-bench-ipropertysource/#cangetvalue)


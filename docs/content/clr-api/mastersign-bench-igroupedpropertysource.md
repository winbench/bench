---
title: "BenchLib - IGroupedPropertySource"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IGroupedPropertySource Interface
This interface describes the capability of reading grouped properties. The properties are identified by a group and a name unique in this group. 

**Absolute Name:** `Mastersign.Bench.IGroupedPropertySource`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [GetGroupCategory(string)](#getgroupcategory)
* [GetGroupValue(string, string)](#getgroupvalue)
* [CanGetGroupValue(string, string)](#cangetgroupvalue)

### Methods {#methods}

#### GetGroupCategory(string) {#getgroupcategory}
Gets the category of the specified group, or `null` if the group has no category. 

##### Parameter

* **group**  
  The group in question.

##### Return Value
The category name of the given group, or `null`.

#### GetGroupValue(string, string) {#getgroupvalue}
Gets the value of the specified property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
The value of the specified property, or `null` if the property does not exist.

#### CanGetGroupValue(string, string) {#cangetgroupvalue}
Checks, whether this object can retrieve the value for the specified property, or not. 

##### Parameter

* **group**  
  The group of the property in question.
* **name**  
  The name of the property in question.

##### Return Value
`true` if this object can get the value for specified property; otherwise `false`.

##### Remarks
Even when this method returns `true`, it may be the case, that  [`GetGroupValue(string, string)`](/clr-api/mastersign-bench-igroupedpropertysource/#getgroupvalue) returns `null`, because the property exists, but the value of the property is `null`. 


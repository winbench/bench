---
title: "BenchLib - IGroupedPropertyTarget"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IGroupedPropertyTarget Interface
This interace describes the capability of storing values for grouped properties. The properties are identified by a group and a name unique in this group. 

**Absolute Name:** `Mastersign.Bench.IGroupedPropertyTarget`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [SetGroupCategory(string, string)](#setgroupcategory)
* [SetGroupValue(string, string, Object)](#setgroupvalue)

### Methods {#methods}

#### SetGroupCategory(string, string) {#setgroupcategory}
Marks a group with a category. 

##### Parameter

* **group**  
  The group to be marked.
* **category**  
  The new category for the group.

#### SetGroupValue(string, string, Object) {#setgroupvalue}
Sets the value of the specified property. If the property did exist until now, it is created. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new value for the property.


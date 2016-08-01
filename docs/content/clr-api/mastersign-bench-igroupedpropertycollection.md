---
title: "BenchLib - IGroupedPropertyCollection"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IGroupedPropertyCollection Interface
This interface describes an object, which stores grouped properties. It is a combination of  [`IGroupedPropertySource`](/clr-api/mastersign-bench-igroupedpropertysource/) and  [`IGroupedPropertyTarget`](/clr-api/mastersign-bench-igroupedpropertytarget/). 

**Absolute Name:** `Mastersign.Bench.IGroupedPropertyCollection`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [Groups()](#groups)
* [GroupsByCategory(string)](#groupsbycategory)
* [PropertyNames(string)](#propertynames)
* [GetGroupValue(string, string, Object)](#getgroupvalue)
* [ContainsGroup(string)](#containsgroup)
* [ContainsGroupValue(string, string)](#containsgroupvalue)

### Methods {#methods}

#### Groups() {#groups}
Gets the groups in this collection. 

##### Return Value
An enumeration of group names.

#### GroupsByCategory(string) {#groupsbycategory}
Gets all groups, marked with the specified category. 

##### Parameter

* **category**  
  The category.

##### Return Value
An enumeration of group names.

#### PropertyNames(string) {#propertynames}
Gets the property names in the specified group. 

##### Parameter

* **group**  
  The group name.

##### Return Value
An enumeration of property names.

#### GetGroupValue(string, string, Object) {#getgroupvalue}
Gets the value of the specified property, or a given default value, in case the specified property does not exist. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
The value of the specified property, or  `def`in case the specified value does not exist.

#### ContainsGroup(string) {#containsgroup}
Checks, whether this collection contains properties in the specified group. 

##### Parameter

* **group**  
  The name of the group.

##### Return Value
`true` if properties in the specified group exists; otherwise `false`.

##### See Also

* [Mastersign.Bench.IPropertySource.CanGetValue(System.String)](/clr-api/mastersign-bench-ipropertysource/#cangetvalue)

#### ContainsGroupValue(string, string) {#containsgroupvalue}
Checks, whether this collection contains the specified property in the specified group. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
`true` if this collection contains the specified property; otherwise `false`.

##### See Also

* [Mastersign.Bench.IPropertySource.CanGetValue(System.String)](/clr-api/mastersign-bench-ipropertysource/#cangetvalue)


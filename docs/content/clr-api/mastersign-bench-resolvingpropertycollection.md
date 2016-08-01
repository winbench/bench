---
title: "BenchLib - ResolvingPropertyCollection"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ResolvingPropertyCollection Class
This class is an extension of the  [`GroupedPropertyCollection`](/clr-api/mastersign-bench-groupedpropertycollection/), which allows to register mutliple property resolver. 

**Absolute Name:** `Mastersign.Bench.ResolvingPropertyCollection`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [AddResolver(IGroupedValueResolver)](#addresolver)
* [ResolveValue(string, Object)](#resolvevalue)
* [ResolveGroupValue(string, string, Object)](#resolvegroupvalue)

### Methods {#methods}

#### AddResolver(IGroupedValueResolver) {#addresolver}
Registers a number of property resolvers. 

##### Parameter

* **resolvers**  
  The property resolvers to register.

#### ResolveValue(string, Object) {#resolvevalue}
The implementation of  [`ResolveValue(string, Object)`](/clr-api/mastersign-bench-groupedpropertycollection/#resolvevalue), calling all registered resolvers in the order they were registered. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The original value of the property.

##### Return Value
The resolved or transformed value of the property.

#### ResolveGroupValue(string, string, Object) {#resolvegroupvalue}
The implementation of  [`ResolveGroupValue(string, string, Object)`](/clr-api/mastersign-bench-groupedpropertycollection/#resolvegroupvalue), calling all registered resolvers in the order they were registered. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The original value of the property.

##### Return Value
The resolved or transformed value of the property.


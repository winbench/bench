---
title: "BenchLib - GroupedVariableResolver"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## GroupedVariableResolver Class
This resolver for group property values, resolves grouped variable references in property values. 

The default syntax for a grouped variable reference is an expression like `$GROUP:NAME$`. Every occurance of such an expression is replaced by the value of the referenced property from  [`ValueSource`](/clr-api/mastersign-bench-groupedvariableresolver/#valuesource). This syntax can be changed by setting a custom expression for  [`GroupVariablePattern`](/clr-api/mastersign-bench-groupedvariableresolver/#groupvariablepattern). 

If the referenced property does not exists, the expression is transformed by replacing it with `#GROUP:NAME#`. 



**Absolute Name:** `Mastersign.Bench.GroupedVariableResolver`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [GroupedVariableResolver()](#ctor-86e9fb59)
* [GroupedVariableResolver(IGroupedPropertyCollection)](#ctor-8436c605)

**Properties**

* [GroupVariablePattern](#groupvariablepattern)
* [ValueSource](#valuesource)

**Methods**

* [ResolveGroupValue(string, string, Object)](#resolvegroupvalue)

### Constructors {#ctors}

#### GroupedVariableResolver() {#ctor-86e9fb59}
Initializes a new instance of  [`GroupedVariableResolver`](/clr-api/mastersign-bench-groupedvariableresolver/). 

#### GroupedVariableResolver(IGroupedPropertyCollection) {#ctor-8436c605}
Initializes a new instance of  [`GroupedVariableResolver`](/clr-api/mastersign-bench-groupedvariableresolver/). 

##### Parameter

* **valueSource**  
  The value source for the referenced properties.

### Properties {#properties}

#### GroupVariablePattern {#groupvariablepattern}
A regular expression, that detects variable references. The defaut expression is `\$(?<group>.*?):(?<name>.+?)\$`

##### Remarks
The regular expression needs two named capture groups called `group` and `name`. 

#### ValueSource {#valuesource}
A group property collection, which will be used as to retrieve the referenced property values. 

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


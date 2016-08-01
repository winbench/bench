---
title: "BenchLib - VariableResolver"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## VariableResolver Class
This resolver for group property values, resolves variable references in property values. 

The default syntax for a variable reference is an expression like `$NAME$`. Every occurance of such an expression is replaced by the value of the referenced property from  [`ValueSource`](/clr-api/mastersign-bench-variableresolver/#valuesource). This syntax can be changed by setting a custom expression for  [`VariablePattern`](/clr-api/mastersign-bench-variableresolver/#variablepattern). 

If the referenced property does not exists, the expression is transformed by replacing it with `#NAME#`. 



**Absolute Name:** `Mastersign.Bench.VariableResolver`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [VariableResolver()](#ctor-6c584f7d)
* [VariableResolver(IPropertyCollection)](#ctor-7ad5151c)

**Properties**

* [VariablePattern](#variablepattern)
* [ValueSource](#valuesource)

**Methods**

* [ResolveGroupValue(string, string, Object)](#resolvegroupvalue)

### Constructors {#ctors}

#### VariableResolver() {#ctor-6c584f7d}
Initializes a new instance of  [`VariableResolver`](/clr-api/mastersign-bench-variableresolver/). 

#### VariableResolver(IPropertyCollection) {#ctor-7ad5151c}
Initializes a new instance of  [`VariableResolver`](/clr-api/mastersign-bench-variableresolver/). 

##### Parameter

* **valueSource**  
  The value source for the referenced variables.

### Properties {#properties}

#### VariablePattern {#variablepattern}
A regular expression, that detects variable references. The defaut expression is `\$(?<name>.+?)\$`

##### Remarks
The regular expression needs a named capture group called `name`'. 

#### ValueSource {#valuesource}
A property collection, which will be used as to retrieve the referenced property values. 

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


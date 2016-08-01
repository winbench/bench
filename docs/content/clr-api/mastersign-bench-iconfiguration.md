---
title: "BenchLib - IConfiguration"
categeories:
  - ".NET API"
  - "Interface"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## IConfiguration Interface
This interface describes an object which is capable of storing ungrouped and grouped properties. Additionally it provides helper methods, to support type safe access to a limited number of simple types. 

**Absolute Name:** `Mastersign.Bench.IConfiguration`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [SetValue(string, string)](#setvalue-c83a880e)
* [SetValue(string, string)](#setvalue-f8654394)
* [SetValue(string, bool)](#setvalue-88f1cfc0)
* [SetValue(string, int)](#setvalue-8727c945)
* [SetGroupValue(string, string, string)](#setgroupvalue-8002c817)
* [SetGroupValue(string, string, string)](#setgroupvalue-c213e319)
* [SetGroupValue(string, string, bool)](#setgroupvalue-95acf388)
* [SetGroupValue(string, string, int)](#setgroupvalue-d99a4bf6)
* [GetStringValue(string)](#getstringvalue-79643df1)
* [GetStringValue(string, string)](#getstringvalue-85a4502f)
* [GetStringGroupValue(string, string)](#getstringgroupvalue-e15813ed)
* [GetStringGroupValue(string, string, string)](#getstringgroupvalue-9b26c242)
* [GetStringListValue(string)](#getstringlistvalue-6902f851)
* [GetStringListValue(string, string)](#getstringlistvalue-6907f027)
* [GetStringListGroupValue(string, string)](#getstringlistgroupvalue-27ea167c)
* [GetStringListGroupValue(string, string, string)](#getstringlistgroupvalue-e5741254)
* [GetBooleanValue(string)](#getbooleanvalue-b47a2655)
* [GetBooleanValue(string, bool)](#getbooleanvalue-ff4f0bc3)
* [GetBooleanGroupValue(string, string)](#getbooleangroupvalue-22d1a921)
* [GetBooleanGroupValue(string, string, bool)](#getbooleangroupvalue-e96f1869)
* [GetInt32Value(string)](#getint32value-8df561cb)
* [GetInt32Value(string, int)](#getint32value-bee3b198)
* [GetInt32GroupValue(string, string)](#getint32groupvalue-0eeb164f)
* [GetInt32GroupValue(string, string, int)](#getint32groupvalue-6a4cce20)

### Methods {#methods}

#### SetValue(string, string) {#setvalue-c83a880e}
Sets a string value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new string value for the property.

#### SetValue(string, string) {#setvalue-f8654394}
Sets a string array value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new string array value for the property.

#### SetValue(string, bool) {#setvalue-88f1cfc0}
Sets a boolean value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new boolean value for the property.

#### SetValue(string, int) {#setvalue-8727c945}
Sets an integer value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new integer value for the property.

#### SetGroupValue(string, string, string) {#setgroupvalue-8002c817}
Sets a string value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new string value for the property.

#### SetGroupValue(string, string, string) {#setgroupvalue-c213e319}
Sets a string array value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new string array value for the property.

#### SetGroupValue(string, string, bool) {#setgroupvalue-95acf388}
Sets a boolean value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new boolean value for the property.

#### SetGroupValue(string, string, int) {#setgroupvalue-d99a4bf6}
Sets an integer value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new integer value for the property.

#### GetStringValue(string) {#getstringvalue-79643df1}
Gets the value of a property as a string. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A string, or `null` if the property does not exist or its value can not be properly converted.

#### GetStringValue(string, string) {#getstringvalue-85a4502f}
Gets the value of a property as a string, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string or  `def` if the property does not exist, or its value can not be properly converted.

#### GetStringGroupValue(string, string) {#getstringgroupvalue-e15813ed}
Gets the value of a group property as a string. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A string or `null` if the property does not exist, or its value can not be properly converted.

#### GetStringGroupValue(string, string, string) {#getstringgroupvalue-9b26c242}
Gets the value of a group property as a string, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string or  `def` if the property does not exist, or its value can not be properly converted.

#### GetStringListValue(string) {#getstringlistvalue-6902f851}
Gets the value of a property as a string array. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A string array, that may be empty, if the property does not exist, or its value can not be properly converted.

#### GetStringListValue(string, string) {#getstringlistvalue-6907f027}
Gets the value of a property as a string array, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string array or  `def` if the property does not exist, or its value can not be properly converted.

#### GetStringListGroupValue(string, string) {#getstringlistgroupvalue-27ea167c}
Gets the value of a group property as a string array. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A string array, that may be empty if the property does not exist, or its value can not be properly converted.

#### GetStringListGroupValue(string, string, string) {#getstringlistgroupvalue-e5741254}
Gets the value of a group property as a string array, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string array or  `def` if the property does not exist, or its value can not be properly converted.

#### GetBooleanValue(string) {#getbooleanvalue-b47a2655}
Gets the value of a property as a boolean. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A boolean, or `false` if the property does not exist or its value can not be properly converted.

#### GetBooleanValue(string, bool) {#getbooleanvalue-ff4f0bc3}
Gets the value of a property as a boolean, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A boolean or  `def` if the property does not exist, or its value can not be properly converted.

#### GetBooleanGroupValue(string, string) {#getbooleangroupvalue-22d1a921}
Gets the value of a group property as a boolean. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A boolean or `false` if the property does not exist, or its value can not be properly converted.

#### GetBooleanGroupValue(string, string, bool) {#getbooleangroupvalue-e96f1869}
Gets the value of a group property as a boolean, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A boolean or  `def` if the property does not exist, or its value can not be properly converted.

#### GetInt32Value(string) {#getint32value-8df561cb}
Gets the value of a property as an integer. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
An integer, or `0` if the property does not exist or its value can not be properly converted.

#### GetInt32Value(string, int) {#getint32value-bee3b198}
Gets the value of a property as an integer, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
An integer or  `def` if the property does not exist, or its value can not be properly converted.

#### GetInt32GroupValue(string, string) {#getint32groupvalue-0eeb164f}
Gets the value of a group property as an integer. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
An integer or `0` if the property does not exist, or its value can not be properly converted.

#### GetInt32GroupValue(string, string, int) {#getint32groupvalue-6a4cce20}
Gets the value of a group property as an integer, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
An integer or  `def` if the property does not exist, or its value can not be properly converted.


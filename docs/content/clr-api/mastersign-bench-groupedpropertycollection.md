---
title: "BenchLib - GroupedPropertyCollection"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## GroupedPropertyCollection Class
This class is the default implementation for  [`IConfiguration`](/clr-api/mastersign-bench-iconfiguration/). It can be cascaded by setting a  [`DefaultValueSource`](/clr-api/mastersign-bench-groupedpropertycollection/#defaultvaluesource)and a  [`GroupedDefaultValueSource`](/clr-api/mastersign-bench-groupedpropertycollection/#groupeddefaultvaluesource) which are used in case this instance does not contain a specified property. 

**Absolute Name:** `Mastersign.Bench.GroupedPropertyCollection`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Properties**

* [DefaultValueSource](#defaultvaluesource)
* [GroupedDefaultValueSource](#groupeddefaultvaluesource)

**Methods**

* [Clear()](#clear)
* [SetGroupCategory(string, string)](#setgroupcategory)
* [GetGroupCategory(string)](#getgroupcategory)
* [ContainsGroup(string)](#containsgroup)
* [ContainsValue(string)](#containsvalue)
* [ContainsGroupValue(string, string)](#containsgroupvalue)
* [CanGetValue(string)](#cangetvalue)
* [CanGetGroupValue(string, string)](#cangetgroupvalue)
* [SetValue(string, string)](#setvalue-df342b13)
* [SetValue(string, string)](#setvalue-00e728aa)
* [SetValue(string, bool)](#setvalue-9fb343bb)
* [SetValue(string, int)](#setvalue-67dfb0af)
* [SetValue(string, Object)](#setvalue-1aa3697b)
* [SetGroupValue(string, string, string)](#setgroupvalue-4c83d057)
* [SetGroupValue(string, string, string)](#setgroupvalue-06bd5d53)
* [SetGroupValue(string, string, bool)](#setgroupvalue-741156bc)
* [SetGroupValue(string, string, int)](#setgroupvalue-f6c7bd2a)
* [SetGroupValue(string, string, Object)](#setgroupvalue-6aa206b6)
* [GetRawValue(string)](#getrawvalue)
* [GetRawGroupValue(string, string)](#getrawgroupvalue)
* [GetValue(string)](#getvalue-6cdb5697)
* [GetValue(string, Object)](#getvalue-ec90ed9b)
* [GetGroupValue(string, string)](#getgroupvalue-4d53ff88)
* [GetGroupValue(string, string, Object)](#getgroupvalue-3931ed60)
* [GetStringValue(string)](#getstringvalue-d4d167c8)
* [GetStringValue(string, string)](#getstringvalue-80578806)
* [GetStringGroupValue(string, string)](#getstringgroupvalue-6007a305)
* [GetStringGroupValue(string, string, string)](#getstringgroupvalue-ff4387e2)
* [GetStringListValue(string)](#getstringlistvalue-3cfa416a)
* [GetStringListValue(string, string)](#getstringlistvalue-eb59cfc7)
* [GetStringListGroupValue(string, string)](#getstringlistgroupvalue-934e3cca)
* [GetStringListGroupValue(string, string, string)](#getstringlistgroupvalue-44766780)
* [GetBooleanValue(string)](#getbooleanvalue-1afc7de1)
* [GetBooleanValue(string, bool)](#getbooleanvalue-e2e7a82e)
* [GetBooleanGroupValue(string, string)](#getbooleangroupvalue-578cb57a)
* [GetBooleanGroupValue(string, string, bool)](#getbooleangroupvalue-fe3cb809)
* [GetInt32Value(string)](#getint32value-8d8c7874)
* [GetInt32Value(string, int)](#getint32value-d74edf60)
* [GetInt32GroupValue(string, string)](#getint32groupvalue-ef1da9b3)
* [GetInt32GroupValue(string, string, int)](#getint32groupvalue-8e267073)
* [ResolveValue(string, Object)](#resolvevalue)
* [ResolveGroupValue(string, string, Object)](#resolvegroupvalue)
* [Groups()](#groups)
* [GroupsByCategory(string)](#groupsbycategory)
* [PropertyNames()](#propertynames-dfe1c3c2)
* [PropertyNames(string)](#propertynames-75e9c324)
* [ToString()](#tostring-c5262838)
* [ToString(bool)](#tostring-05006b94)

### Properties {#properties}

#### DefaultValueSource {#defaultvaluesource}
The backup value source for ungrouped properties. 

#### GroupedDefaultValueSource {#groupeddefaultvaluesource}
The back value source for group properties. 

### Methods {#methods}

#### Clear() {#clear}
Deletes all properties in this collection. 

#### SetGroupCategory(string, string) {#setgroupcategory}
Marks a group with a category. 

##### Parameter

* **group**  
  The group to be marked.
* **category**  
  The new category for the group.

#### GetGroupCategory(string) {#getgroupcategory}
Gets the category of the specified group, or `null` if the group has no category. 

##### Parameter

* **group**  
  The group in question.

##### Return Value
The category name of the given group, or `null`.

#### ContainsGroup(string) {#containsgroup}
Checks, whether this collection contains properties in the specified group. 

##### Parameter

* **group**  
  The name of the group.

##### Return Value
`true` if properties in the specified group exists; otherwise `false`.

##### See Also

* [Mastersign.Bench.IPropertySource.CanGetValue(System.String)](/clr-api/mastersign-bench-ipropertysource/#cangetvalue)

#### ContainsValue(string) {#containsvalue}
Checks, whether this collection contains the specified property. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
`true` if the property exists; otherwise `false`.

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

#### CanGetValue(string) {#cangetvalue}
Checks, whether this object can retrieve the value for the specified property, or not. 

##### Parameter

* **name**  
  The name of the property in question.

##### Return Value
`true` if this object can get the value for specified property; otherwise `false`.

##### Remarks
Even when this method returns `true`, it may be the case, that  [`GetValue(string)`](/clr-api/mastersign-bench-groupedpropertycollection/#getvalue-6cdb5697) returns `null`, because the property exists, but the value of the property is `null`. 

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
Even when this method returns `true`, it may be the case, that  [`GetGroupValue(string, string)`](/clr-api/mastersign-bench-groupedpropertycollection/#getgroupvalue-4d53ff88) returns `null`, because the property exists, but the value of the property is `null`. 

#### SetValue(string, string) {#setvalue-df342b13}
Sets a string value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new string value for the property.

#### SetValue(string, string) {#setvalue-00e728aa}
Sets a string array value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new string array value for the property.

#### SetValue(string, bool) {#setvalue-9fb343bb}
Sets a boolean value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new boolean value for the property.

#### SetValue(string, int) {#setvalue-67dfb0af}
Sets an integer value for the specified property. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new integer value for the property.

#### SetValue(string, Object) {#setvalue-1aa3697b}
Sets the value of the specified property. If the property did exist until now, it is created. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The new value of the property.

#### SetGroupValue(string, string, string) {#setgroupvalue-4c83d057}
Sets a string value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new string value for the property.

#### SetGroupValue(string, string, string) {#setgroupvalue-06bd5d53}
Sets a string array value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new string array value for the property.

#### SetGroupValue(string, string, bool) {#setgroupvalue-741156bc}
Sets a boolean value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new boolean value for the property.

#### SetGroupValue(string, string, int) {#setgroupvalue-f6c7bd2a}
Sets an integer value for the specified group property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new integer value for the property.

#### SetGroupValue(string, string, Object) {#setgroupvalue-6aa206b6}
Sets the value of the specified property. If the property did exist until now, it is created. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The new value for the property.

#### GetRawValue(string) {#getrawvalue}
Gets the unresolved and untransformed value of a property in this collection, without looking up the property in  [`DefaultValueSource`](/clr-api/mastersign-bench-groupedpropertycollection/#defaultvaluesource). 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
The value of the specified property or `null`.

##### Exceptions

* [**ArgumentNullException**](/clr-api/system-argumentnullexception/)
  Is thrown if `null` is passed for  `name`.
* [**ArgumentOutOfRangeException**](/clr-api/system-argumentoutofrangeexception/)
  Is thrown if an empty string is passed as  `name`.


#### GetRawGroupValue(string, string) {#getrawgroupvalue}
Gets the unresolved and untransformed value of a group property in this collection, without looking up the property in  [`GroupedDefaultValueSource`](/clr-api/mastersign-bench-groupedpropertycollection/#groupeddefaultvaluesource). 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
The value of the specified property or `null`.

##### Exceptions

* [**ArgumentNullException**](/clr-api/system-argumentnullexception/)
  Is thrown if `null` is passed for  `name`.
* [**ArgumentOutOfRangeException**](/clr-api/system-argumentoutofrangeexception/)
  Is thrown if an empty string is passed as  `name`.


#### GetValue(string) {#getvalue-6cdb5697}
Gets the value of the specified property. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
The value of the specified property, or `null` if the property does not exist.

#### GetValue(string, Object) {#getvalue-ec90ed9b}
Gets the value of the specified property, or a given default value, in case the specified property does not exist. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
The value of the specified property, or  `def`in case the specified value does not exist.

#### GetGroupValue(string, string) {#getgroupvalue-4d53ff88}
Gets the value of the specified property. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
The value of the specified property, or `null` if the property does not exist.

#### GetGroupValue(string, string, Object) {#getgroupvalue-3931ed60}
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

#### GetStringValue(string) {#getstringvalue-d4d167c8}
Gets the value of a property as a string. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A string, or `null` if the property does not exist or its value can not be properly converted.

#### GetStringValue(string, string) {#getstringvalue-80578806}
Gets the value of a property as a string, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string or  `def` if the property does not exist, or its value can not be properly converted.

#### GetStringGroupValue(string, string) {#getstringgroupvalue-6007a305}
Gets the value of a group property as a string. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A string or `null` if the property does not exist, or its value can not be properly converted.

#### GetStringGroupValue(string, string, string) {#getstringgroupvalue-ff4387e2}
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

#### GetStringListValue(string) {#getstringlistvalue-3cfa416a}
Gets the value of a property as a string array. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A string array, that may be empty, if the property does not exist, or its value can not be properly converted.

#### GetStringListValue(string, string) {#getstringlistvalue-eb59cfc7}
Gets the value of a property as a string array, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A string array or  `def` if the property does not exist, or its value can not be properly converted.

#### GetStringListGroupValue(string, string) {#getstringlistgroupvalue-934e3cca}
Gets the value of a group property as a string array. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A string array, that may be empty if the property does not exist, or its value can not be properly converted.

#### GetStringListGroupValue(string, string, string) {#getstringlistgroupvalue-44766780}
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

#### GetBooleanValue(string) {#getbooleanvalue-1afc7de1}
Gets the value of a property as a boolean. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
A boolean, or `false` if the property does not exist or its value can not be properly converted.

#### GetBooleanValue(string, bool) {#getbooleanvalue-e2e7a82e}
Gets the value of a property as a boolean, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
A boolean or  `def` if the property does not exist, or its value can not be properly converted.

#### GetBooleanGroupValue(string, string) {#getbooleangroupvalue-578cb57a}
Gets the value of a group property as a boolean. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
A boolean or `false` if the property does not exist, or its value can not be properly converted.

#### GetBooleanGroupValue(string, string, bool) {#getbooleangroupvalue-fe3cb809}
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

#### GetInt32Value(string) {#getint32value-8d8c7874}
Gets the value of a property as an integer. 

##### Parameter

* **name**  
  The name of the property.

##### Return Value
An integer, or `0` if the property does not exist or its value can not be properly converted.

#### GetInt32Value(string, int) {#getint32value-d74edf60}
Gets the value of a property as an integer, or a default value if the specified property does not exist or its value can not be properly converted. 

##### Parameter

* **name**  
  The name of the property.
* **def**  
  The default value.

##### Return Value
An integer or  `def` if the property does not exist, or its value can not be properly converted.

#### GetInt32GroupValue(string, string) {#getint32groupvalue-ef1da9b3}
Gets the value of a group property as an integer. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.

##### Return Value
An integer or `0` if the property does not exist, or its value can not be properly converted.

#### GetInt32GroupValue(string, string, int) {#getint32groupvalue-8e267073}
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

#### ResolveValue(string, Object) {#resolvevalue}
This method is a hook for child classes, to implement some kind of value resolution or transformation for ungrouped properties. 

##### Parameter

* **name**  
  The name of the property.
* **value**  
  The original value of the property.

##### Return Value
The resolved or transformed value.

#### ResolveGroupValue(string, string, Object) {#resolvegroupvalue}
This method is a hook for child classes, to implement some kind of value resolution or transformation for grouped properties. 

##### Parameter

* **group**  
  The group of the property.
* **name**  
  The name of the property.
* **value**  
  The original value of the property.

##### Return Value
The resolved or transformed value.

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

#### PropertyNames() {#propertynames-dfe1c3c2}
Gets the names from all existing properties. 

##### Return Value
An enumeration of strings.

#### PropertyNames(string) {#propertynames-75e9c324}
Gets the property names in the specified group. 

##### Parameter

* **group**  
  The group name.

##### Return Value
An enumeration of property names.

#### ToString() {#tostring-c5262838}
Returns a string represenation of this property collection. 

##### Return Value
A string containing all properties and thier values.

#### ToString(bool) {#tostring-05006b94}
Returns a string represenation of this property collection. 

##### Parameter

* **resolve**  
  A flag, controlling the resolution of property values.

##### Return Value
A string containing all properties and thier values.


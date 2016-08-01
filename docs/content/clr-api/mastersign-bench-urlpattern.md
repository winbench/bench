---
title: "BenchLib - UrlPattern"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## UrlPattern Class
This class helps to identify URLs which must be resolved before the download of an HTTP(S) resource is possible. 

**Absolute Name:** `Mastersign.Bench.UrlPattern`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [UrlPattern(Regex, Regex, String, Regex})](#ctor)

**Properties**

* [HostPattern](#hostpattern)
* [PathPattern](#pathpattern)
* [QueryPattern](#querypattern)

**Methods**

* [IsMatch(Uri)](#ismatch)

### Constructors {#ctors}

#### UrlPattern(Regex, Regex, String, Regex}) {#ctor}
Initializes a new instance of  [`UrlPattern`](/clr-api/mastersign-bench-urlpattern/). 

##### Parameter

* **host**  
  A regular expression for the host part of an URL or `null`.
* **path**  
  A regular expression for the path part of an URL or `null`.
* **query**  
  A dictionary with regular expressions, which must match the query arguments of an URL, or `null`.

### Properties {#properties}

#### HostPattern {#hostpattern}
A regular expression wich must match the host part of an URL, or `null`. 

#### PathPattern {#pathpattern}
A regular expression which must match the path part of an URL, or `null`. 

#### QueryPattern {#querypattern}
A dictionary with regular expressions, which must match the query arguments of an URL, or `null`. The key in the dictionary is the name of a query argument. The value in the dictionary is the regular expression which must match the value of the corresponding query argument. 

### Methods {#methods}

#### IsMatch(Uri) {#ismatch}
Checks, whether the given URL is a match. 

##### Parameter

* **url**  
  The URL in question.

##### Return Value
`true` if the given URL matches this pattern; othwerwise `false`.


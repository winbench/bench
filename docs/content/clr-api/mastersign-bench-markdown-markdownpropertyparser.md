---
title: "BenchLib - MarkdownPropertyParser"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## MarkdownPropertyParser Class
This class parses Markdown files for configuration properties. 

**Absolute Name:** `Mastersign.Bench.Markdown.MarkdownPropertyParser`  
**Namespace:** Mastersign.Bench.Markdown  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [MarkdownPropertyParser()](#ctor-81e2c6f5)
* [MarkdownPropertyParser(IGroupedPropertyTarget)](#ctor-53f9c32b)

**Properties**

* [ActivationCue](#activationcue)
* [DeactivationCue](#deactivationcue)
* [CategoryCue](#categorycue)
* [GroupBeginCue](#groupbegincue)
* [GroupEndCue](#groupendcue)
* [Target](#target)

**Methods**

* [Parse(Stream)](#parse-ecaba804)
* [Parse(TextReader)](#parse-08b9a642)

### Constructors {#ctors}

#### MarkdownPropertyParser() {#ctor-81e2c6f5}
Initializes a new instance of  [`MarkdownPropertyParser`](/clr-api/mastersign-bench-markdown-markdownpropertyparser/). 

#### MarkdownPropertyParser(IGroupedPropertyTarget) {#ctor-53f9c32b}
Initializes a new instance of  [`MarkdownPropertyParser`](/clr-api/mastersign-bench-markdown-markdownpropertyparser/). 

##### Parameter

* **target**  
  The target for recognized properties.

### Properties {#properties}

#### ActivationCue {#activationcue}
This regular expression is used to activate the property parsing. If this property is not `null`, the recognition of properties starts after a line matches this expression. 

#### DeactivationCue {#deactivationcue}
This regular expression is used to deactivate the property parsing. If this property is not `null`, the recognition of properties stops after a line matches this expression. 

#### CategoryCue {#categorycue}
This regular expression is used to detect the beginning of a group category in the Markdown file. If this property is not `null`, and a line matches this expression, the current category is changed for all further property groups in the file. 

##### Remarks
The regular expression needs a named capture group with name `category`. 

#### GroupBeginCue {#groupbegincue}
This regular expression is used to detect the beginning of a property group. Properties, which are detected before this expression matches a line, are stored as ungrouped properties. Properties, which are detected after this expression matches a line, are stored as group properties. 

##### Remarks
The regular expression needs a named capture group with name `group`. 

#### GroupEndCue {#groupendcue}
This regular expression is used to detect the end of a property group. Properties, which are recognized after this expression matches a line, are stored as ungrouped properties. 

#### Target {#target}
Gets or sets the property target, where recognized properties are stored in. 

### Methods {#methods}

#### Parse(Stream) {#parse-ecaba804}
Parses the data in the given stream as UTF8 encoded Markdown text and recognizes configuration properties. 

##### Parameter

* **source**  
  The source stream.

#### Parse(TextReader) {#parse-08b9a642}
Parses the given text input and recognizes configuration properties. 

##### Parameter

* **source**  
  The text input.


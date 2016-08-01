---
title: "BenchLib - MarkdownToHtmlConverter"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## MarkdownToHtmlConverter Class
This class allows to convert a Markdown text into a HTML page. It makes sure, that all headlines have anchors, and makes these anchors available to the user of this class, to support a navigation index. 

**Absolute Name:** `Mastersign.Bench.Markdown.MarkdownToHtmlConverter`  
**Namespace:** Mastersign.Bench.Markdown  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Fields**

* [Anchors](#anchors)

**Methods**

* [ConvertToHtml(Stream)](#converttohtml-3a3e92dc)
* [ConvertToHtml(TextReader)](#converttohtml-8cc721ac)

### Fields {#fields}

#### Anchors {#anchors}
All anchors in the converted HTML page. 

### Methods {#methods}

#### ConvertToHtml(Stream) {#converttohtml-3a3e92dc}
Converts the given data into a HTML page. The data is interpreted as UTF8 encoded Markdown text. 

##### Parameter

* **source**  
  The input data.

##### Return Value
A string with the HTML markup of the generated page.

#### ConvertToHtml(TextReader) {#converttohtml-8cc721ac}
Converts the given Markdown text into a HTML page. 

##### Parameter

* **source**  
  The input Markdown text.

##### Return Value
A string with the HTML markup of the generated page.


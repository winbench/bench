---
title: "BenchLib - DownloadProgressEventArgs"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## DownloadProgressEventArgs Class
This class defines the arguments of the  [`DownloadProgress`](/clr-api/mastersign-bench-downloader/#downloadprogress) event. 

**Absolute Name:** `Mastersign.Bench.DownloadProgressEventArgs`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [DownloadProgressEventArgs(DownloadTask, long, long, int)](#ctor)

**Properties**

* [LoadedBytes](#loadedbytes)
* [TotalBytes](#totalbytes)
* [Percentage](#percentage)

### Constructors {#ctors}

#### DownloadProgressEventArgs(DownloadTask, long, long, int) {#ctor}
Initializes a new instance of  [`DownloadProgressEventArgs`](/clr-api/mastersign-bench-downloadprogresseventargs/). 

##### Parameter

* **task**  
  The affected download task.
* **loadedBytes**  
  The number of already downloaded bytes.
* **totalBytes**  
  The total number of bytes, this HTTP(S) resource encompasses.
* **percentage**  
  The percentage of the progress.

### Properties {#properties}

#### LoadedBytes {#loadedbytes}
The number of bytes already downloaded. 

#### TotalBytes {#totalbytes}
The total number of bytes this HTTP(S) resource encompasses. 

#### Percentage {#percentage}
The percentage of the progress. 


---
title: "BenchLib - DownloadTask"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## DownloadTask Class
This class represents a HTTP(S) resource to be downloaded by a  [`Downloader`](/clr-api/mastersign-bench-downloader/). 

**Absolute Name:** `Mastersign.Bench.DownloadTask`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [DownloadTask(string, Uri, string)](#ctor)

**Properties**

* [Id](#id)
* [Url](#url)
* [Headers](#headers)
* [Cookies](#cookies)
* [TargetFile](#targetfile)
* [ResolvedUrl](#resolvedurl)
* [UrlResolutionFailed](#urlresolutionfailed)
* [DownloadedBytes](#downloadedbytes)
* [HasEnded](#hasended)
* [Success](#success)
* [FailedAttempts](#failedattempts)
* [ErrorMessage](#errormessage)

### Constructors {#ctors}

#### DownloadTask(string, Uri, string) {#ctor}
Initializes a new instance of  [`DownloadTask`](/clr-api/mastersign-bench-downloadtask/). 

##### Parameter

* **id**  
  The unique ID of this download task.
* **url**  
  The URL describing the HTTP(S) rsource.
* **targetFile**  
  A path to the target file.

### Properties {#properties}

#### Id {#id}
A string identifying the download uniquely amont all other downloads. 

#### Url {#url}
The URL of the HTTP(S) resource. 

#### Headers {#headers}
Additional HTTP header fields to send to the server when requesting the HTTP(S) resource. 

#### Cookies {#cookies}
Cookies to send to the server when requesting the HTTP(S) resource. 

#### TargetFile {#targetfile}
A path to the file, to store the data of the downloaded resource in. 

#### ResolvedUrl {#resolvedurl}
The resolve URL of the HTTP(S) resource. This property is set by the  [`Downloader`](/clr-api/mastersign-bench-downloader/) when resolving the URL. 

#### UrlResolutionFailed {#urlresolutionfailed}
A value indicating, whether the resolution of the URL failed, or not. 

#### DownloadedBytes {#downloadedbytes}
The number of downloaded bytes. 

#### HasEnded {#hasended}
A value indicating, whether this download has ended (successfully, or with failure), or not. 

#### Success {#success}
A value indicating, whether this download was completed successfully. 

#### FailedAttempts {#failedattempts}
The number of failed download attempts. 

#### ErrorMessage {#errormessage}
A string describing the last error, occured during the download, or `null` if no error occurred. 


---
title: "BenchLib - Downloader"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## Downloader Class
This class implements a download manager for downloading multiple HTTP(S) resources in parallel and monitoring their progress. 

**Absolute Name:** `Mastersign.Bench.Downloader`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Constructors**

* [Downloader()](#ctor-b303acc4)
* [Downloader(int)](#ctor-e155674a)

**Events**

* [DownloadStarted](#downloadstarted)
* [DownloadProgress](#downloadprogress)
* [DownloadEnded](#downloadended)
* [WorkFinished](#workfinished)
* [IsWorkingChanged](#isworkingchanged)

**Properties**

* [HttpProxy](#httpproxy)
* [HttpsProxy](#httpsproxy)
* [DownloadAttempts](#downloadattempts)
* [UrlResolver](#urlresolver)
* [IsWorking](#isworking)
* [IsDisposed](#isdisposed)

**Methods**

* [Enqueue(DownloadTask)](#enqueue)
* [CancelAll()](#cancelall)
* [Dispose()](#dispose)

### Constructors {#ctors}

#### Downloader() {#ctor-b303acc4}
Initializes a new instance of  [`Downloader`](/clr-api/mastersign-bench-downloader/) with a default configuration, allowing only sequential downloads. 

#### Downloader(int) {#ctor-e155674a}
Initializes a new instance of  [`Downloader`](/clr-api/mastersign-bench-downloader/) with a configuration, allowing the given number of parallel downloads. 

##### Parameter

* **parallelDownloads**  
  The maximum number of parallel downloads. This value must be in the interval of `1` and `9999`.

### Events {#events}

#### DownloadStarted {#downloadstarted}
This event is fired, when a download started. 

#### DownloadProgress {#downloadprogress}
This event is fired, when the progress of a running download was updated. 

#### DownloadEnded {#downloadended}
This event is fired, when a download ended. This can mean success or failure. 

#### WorkFinished {#workfinished}
This event is fired, when all queued download tasks have ended. 

#### IsWorkingChanged {#isworkingchanged}
This event is fired, when the downloader starts with the first download after doing nothing, or if the downloader finished the last queued download task. 

### Properties {#properties}

#### HttpProxy {#httpproxy}
Gets or sets the proxy configuration for HTTP connections. 

#### HttpsProxy {#httpsproxy}
Gets or sets the proxy configuration for HTTPS connections. 

#### DownloadAttempts {#downloadattempts}
Gets or sets the number of attempts per download task. 

#### UrlResolver {#urlresolver}
Gets a collection with  [`IUrlResolver`](/clr-api/mastersign-bench-iurlresolver/) objects. This collection can be changed by the user to control how URLs are resolved before the actual download of a resource starts. 

#### IsWorking {#isworking}
Checks, whether the downloader has queued download tasks, or if it currently does nothing. 

##### Value
`true` if at least download if in progress or queued; `false` if the downloader does nothing.
#### IsDisposed {#isdisposed}
Checks, whether this instance was disposed, or not. 

### Methods {#methods}

#### Enqueue(DownloadTask) {#enqueue}
Enqueues a new download task. 

##### Parameter

* **task**  
  The download task.

#### CancelAll() {#cancelall}
Request the downloader to cancel all pending download tasks and to clear the queue. 

#### Dispose() {#dispose}
Disposes the downloader by cancelling all pending download tasks and freeing all ocupied resource. 


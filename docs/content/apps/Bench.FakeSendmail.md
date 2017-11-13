+++
title = "FakeSendmail"
weight = 130
app_library = "default"
app_category = "Services"
app_typ = "default"
app_ns = "Bench"
app_id = "Bench.FakeSendmail"
app_version = "latest"
app_categories = ["Services"]
app_libraries = ["default"]
app_types = ["default"]
+++

**ID:** `Bench.FakeSendmail`  
**Version:** latest  
<!--more-->

[Back to all apps](/apps/)

## Description
`sendmail.exe` is a simple windows console application that emulates sendmail's "-t" option
to deliver emails piped via stdin.

It is intended to ease running unix code that has `sendmail` hardcoded as an email delivery means.
It doesn't support deferred delivery, and requires an smtp server to perform the actual delivery of the messages.

Alternatively it can store emails as files in a directory for debugging.

This app automatically registers itself in PHP 5 and PHP 7.

## Source

* Library: [`default`](/app_libraries/default)
* Category: [Services](/app_categories/services)
* Order Index: 130

## Properties

* Namespace: Bench
* Name: FakeSendmail
* Typ: `default`
* Website: <https://www.glob.com.au/sendmail/>


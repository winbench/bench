+++
title = "Android SDK"
weight = 91
app_library = "default"
app_category = "Mobile Development"
app_typ = "default"
app_ns = "Bench"
app_id = "Bench.AndroidSDK"
app_version = "3859397"
app_categories = ["Mobile Development"]
app_libraries = ["default"]
app_types = ["default"]
+++

**ID:** `Bench.AndroidSDK`  
**Version:** 3859397  
<!--more-->

[Back to all apps](/apps/)

## Description
Android SDK includes the complete set of development and debugging tools for Android.

**WARNING:**
The Android SDK will write into the home directory of your Windows user profile.
It will create the directory `.android` and will store the AVDs there,
which can reach a considerable size.

**WARNING:**
The setup script for Android SDK can _not_ be run unattended, because it requires user interaction to accept the license agreements.

**WARNING:**
The setup script includes the installation of the Intel _Hardware Accelerated eXecution Manager_, which requires admin privileges for installation and is not contained by the Bench isolation.

## Source

* Library: [`default`](/app_libraries/default)
* Category: [Mobile Development](/app_categories/mobile-development)
* Order Index: 91

## Properties

* Namespace: Bench
* Name: AndroidSDK
* Typ: `default`
* Website: <https://developer.android.com/studio/command-line/>
* Dependencies: [Java Runtime Environment 8](/apps/Bench.JRE8)
* Responsibilities: [Android Studio](/apps/Bench.AndroidStudio)


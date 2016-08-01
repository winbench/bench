---
title: "BenchLib - ConsoleUserInterface"
categeories:
  - ".NET API"
  - "Class"
date: 2016-08-01
author: "Tobias Kiertscher"
copyright: "Licensed by CC-BY-4.0"
---

## ConsoleUserInterface Class
An implementation for  [`IUserInterface`](/clr-api/mastersign-bench-iuserinterface/), which is based on the console. 

**Absolute Name:** `Mastersign.Bench.ConsoleUserInterface`  
**Namespace:** Mastersign.Bench  
**Assembly:** BenchLib, Version 0.12.1.0



### Overview
**Methods**

* [ShowInfo(string, string)](#showinfo)
* [ShowWarning(string, string)](#showwarning)
* [ShowError(string, string)](#showerror)
* [ReadUserInfo(string)](#readuserinfo)
* [ReadPassword(string)](#readpassword)
* [EditTextFile(string)](#edittextfile-57d6a5e5)
* [EditTextFile(string, string)](#edittextfile-e9bfe9bd)

### Methods {#methods}

#### ShowInfo(string, string) {#showinfo}
Displays an informational message to the user. 

##### Parameter

* **topic**  
  The topic of the message.
* **message**  
  The message.

#### ShowWarning(string, string) {#showwarning}
Displays a warning message to the user. 

##### Parameter

* **topic**  
  The topic of the message.
* **message**  
  The message.

#### ShowError(string, string) {#showerror}
Displays an error message to the user. 

##### Parameter

* **topic**  
  The topic of the message.
* **message**  
  The message.

#### ReadUserInfo(string) {#readuserinfo}
Prompts the user to input the necessary information about a Bench user. 

##### Parameter

* **prompt**  
  A string, shown to the user as prompt.

##### Return Value
A new instance of  [`BenchUserInfo`](/clr-api/mastersign-bench-benchuserinfo/) with the read input from the user.

#### ReadPassword(string) {#readpassword}
Reads a password from the user in a safe fashion. 

##### Parameter

* **prompt**  
  A string, shown to the user as prompt.

##### Return Value
An instance of  [`SecureString`](/clr-api/system-security-securestring/).

#### EditTextFile(string) {#edittextfile-57d6a5e5}
Allow the user to edit the given text file. This methods returns, when the user ends the editing. 

##### Parameter

* **path**  
  A path to the text file to edit.

#### EditTextFile(string, string) {#edittextfile-e9bfe9bd}
Allow the user to edit the given text file. This methods returns, when the user ends the editing. 

##### Parameter

* **path**  
  A path to the text file to edit.
* **prompt**  
  A string, shown to the user as prompt.


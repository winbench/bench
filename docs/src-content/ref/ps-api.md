+++
date = "2016-06-22T13:43:35+02:00"
description = "The programming interface for hooks and custom scripts"
draft = true
title = "PowerShell API"
weight = 9
+++

The _PowerShell API_ of Bench consists of a set of functions/commandlets,
available in custom and hook scripts.

## Variables

### `$global:BenchConfig` {#var-bench-config}

This variable in the global scope holds a reference to a
[`Mastersign.Bench.BenchConfiguration`](/ref/clr-api/class/Mastersign.Bench.BenchConfiguration)
object.
This object holds the merged Bench configuration from all configuration files.

## Functions

### Utilities

#### `Exit-OnError` {#fun-exit-onerror}

* Description: Checks an process exit code and exits the current script if it is not 0.
* Parameter
    + `exitCode` (optional)
      The exit code to check.
      Default value is `$LastExitCode`.

#### `Pause` {#fun-pause}

* Description: Prints a message and waits for an arbitrary key press before it returns.
* Parameter
    + `msg` (optional)
      The message to print.
      Default value is `"Press any key to exit ..."`.

#### `Run-Detached` {#fun-run-detached}

### File System

#### `Empty-Dir` {#fun-empty-dir}

#### `Find-File` {#fun-find-file}

#### `Find-Files` {#fun-find-files}

#### `Purge-Dir` {#fun-purge-dir}

#### `Safe-Dir` {#fun-safe-dir}

### Configuration Properties

#### `Get-ConfigValue`

#### `Get-ConfigBooleanValue`

#### `Get-ConfigListValue`

### App Properties

#### `Get-AppConfigValue`

#### `Get-AppConfigBooleanValue`

#### `Get-AppConfigListValue`

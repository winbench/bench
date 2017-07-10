+++
date = "2017-01-02T17:00:00+02:00"
description = "The programming interface for hooks and custom scripts"
title = "PowerShell API"
weight = 100
+++

The _PowerShell API_ of Bench consists of a set of global variables and functions,
available in custom and hook scripts.

[**Variables**](#vars)

<!-- #data-list /*/Variables/* -->

[**Utility Functions**](#util-funcs)

<!-- #data-list /*/Utility Functions/* -->

[**File System Functions**](#fs-funcs)

<!-- #data-list /*/File System Functions/* -->

[**Bench Configuration Functions**](#config-funcs)

<!-- #data-list /*/Bench Configuration Functions/* -->

[**App Property Functions**](#app-config-funcs)

<!-- #data-list /*/App Property Functions/* -->

## Variables {#vars}

### `$BenchConfig` {#var-bench-config}

This variable in the global scope a
[`Mastersign.Bench.BenchConfiguration`](/clr-api/html/T_Mastersign_Bench_BenchConfiguration.htm)
object.
This object holds the merged Bench configuration from all configuration files.

**Hint**: Access via `$global:BenchConfig`.

## Utility Functions {#util-funcs}

### `Exit-OnError` {#fun-exit-onerror}

* Description: Checks an process exit code and exits the current script if it is not 0.
* Parameter:
    + `exitCode` (optional)
      The exit code to check.
      Default value is `$LastExitCode`.

### `Pause` {#fun-pause}

* Description: Prints a message and waits for an arbitrary key press before it returns.
* Parameter:
    + `msg` (optional)
      The message to print.
      Default value is `"Press any key to exit ..."`.

### `Run-Detached` {#fun-run-detached}

* Description: Starts a detached process without blocking.
* Parameter:
    + `path` The absolute path to the executable.
    + ... An arbitrary number of command line arguments

## File System Functions {#fs-funcs}

### `Empty-Dir` {#fun-empty-dir}

* Description: Makes sure a path points to an empty directory.
* Parameter:
    + `path` The absolute path to the directory.
* Return Value: The absolute path to the directory.

If the directory exists, all of its children are deleted.
If the directory does not exist, it is created.

This function is a wrapper for [Mastersign.Bench.FileSystem.EmptyDir()](/ref/clr-api/method/Mastersign.Bench.FileSystem.EmptyDir).

### `Find-File` {#fun-find-file}

* Description: Searches for the first file that matches a given pattern, in a given directory.
* Parameter:
    + `dir` The absolute path to a directory.
    + `pattern` The pattern to search for, syntax according to
      [`System.IO.Directory.GetFiles()`](https://msdn.microsoft.com/de-de/library/wz42302f.aspx).
* Return Value: The absolute path of the found file or `$null`,
  if the given directory does not exist or no file matches.

If more than one file in the given directory matches the pattern,
it is not defined which one is returned.

### `Find-Files` {#fun-find-files}

* Description: Searches for files which match a given pattern, in a given directory.
* Parameter:
    + `dir` The absolute path to a directory.
    + `pattern` The pattern to search for, syntax according to
      [`System.IO.Directory.GetFiles()`](https://msdn.microsoft.com/de-de/library/wz42302f.aspx).
* Return Value: An array with the found files.

If the specified directory does not exist, an empty array is returned.

### `Purge-Dir` {#fun-purge-dir}

* Description: Deletes a directory and all of its content.
* Parameter:
    + `path` The absolute path to the directory.

Solves a problem with deleting directories recursively, if they contain read-only files.
This function is a wrapper for
[Mastersign.Bench.FileSystem.PurgeDir()](/ref/clr-api/method/Mastersign.Bench.FileSystem.PurgeDir).

### `Safe-Dir` {#fun-safe-dir}

* Description: Makes sure a directory exists.
* Parameter:
    + `path` The absolute path to the directory.
* Return Value: The absolute path to the directory.

This method is a wrapper for
[Mastersign.Bench.FileSystem.AsureDir()](/ref/clr-api/method/Mastersign.Bench.FileSystem.AsureDir).

## Bench Configuration Functions {#config-funcs}

### `Get-ConfigValue` {#fun-get-configvalue}

* Description: Returns the value of a [Bench configuration property](/ref/config).
* Parameter:
    + `name` The name of the configuration property.
* Return Value: The value of the property or `$null`.

The data type of the property can be string, boolean, array of strings,
or dictionary with string keys and values.
If you want to read a boolean value use [`Get-ConfigBooleanValue`](#fun-get-configbooleanvalue),
because strings can be interpreted as booleans.
If you want to read a list value use [`Get-ConfigListValue`](#fun-get-configlistvalue),
because strings can be interpreted as an array with one element.

### `Get-ConfigBooleanValue` {#fun-configbooleanvalue}

* Description: Returns the boolean value of a [Bench configuration property](/ref/config).
* Parameter:
    + `name` The name of the configuration property.
* Return Value: The value of the property as boolean.

If the property has no value, `$false` is returned.

### `Get-ConfigListValue` {#fun-configlistvalue}

* Description: Returns the list value of a [Bench configuration property](/ref/config).
* Parameter:
    + `name` The name of the configuration property.
* Return Value: The value of the property as array of strings.

If the property only contains a string, an array with one element is returned.
If the property has no value, an empty array is returned.

## App Property Functions {#app-config-funcs}

### `Get-AppConfigValue` {#fun-get-appconfigvalue}

* Description: Returns the value of an [Bench app property](/ref/app-properties).
* Parameter:
    + `app` The ID of an app.
    + `name` The name of the property.
* Return Value: The value of the property as string or `$null`.

If the property has no value, or the app ID is not defined, `$null` is returned.
If you want to read a boolean value use [`Get-AppConfigBooleanValue`](#fun-get-appconfigbooleanvalue),
because strings can be interpreted as booleans.
If you want to read a list value use [`Get-AppConfigListValue`](#fun-get-appconfiglistvalue),
because strings can be interpreted as an array with one element.

### `Get-AppConfigBooleanValue` {#fun-get-appconfigbooleanvalue}

* Description: Returns the boolean value of an [Bench app property](/ref/app-properties).
* Parameter:
    + `app` The ID of an app.
    + `name` The name of the property.
* Return Value: The value of the property as boolean.

If the property has no value, `$false` is returned.
If the app ID is not defined, `$null` is returned.

### `Get-AppConfigListValue` {#fun-get-appconfiglistvalue}

* Description: Returns the list value of an [Bench app property](/ref/app-properties).
+ `app` The ID of an app.
+ `name` The name of the property.
* Return Value: The value of the property as boolean.

If the property only contains a string, an array with one element is returned.
If the property has no value, an empty array is returned.
If the app ID is not defined, `$null` is returned.

### `App-Typ` {#fun-app-typ}

* Description: This function returns the value of the
  [`Typ`](/ref/app-properties/#Typ) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The string value of the property,
  or `$null` in case the app ID is not defined.

### `App-Version` {#fun-app-version}

* Description: This function returns the value of the
  [`Version`](/ref/app-properties/#Version) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Dependencies` {#fun-app-dependencies}

* Description: This function returns the list value of the
  [`Dependencies`](/ref/app-properties/#Dependencies) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as array of strings,
  or `$null` in case the app ID is not defined.

### `App-Url` {#fun-app-url}

* Description: This function returns the value of the
  [`Url`](/ref/app-properties/#Url) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-DownloadHeaders` {#fun-app-downloadheaders}

* Description: This function returns the dictionary value of the
  [`DownloadHeaders`](/ref/app-properties/#DownloadHeaders) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as dictionary,
  or `$null` in case the app ID is not defined.

### `App-DownloadCookies` {#fun-app-downloadcookies}

* Description: This function returns the dictionary value of the
  [`DownloadCookies`](/ref/app-properties/#DownloadCookies) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as dictionary,
  or `$null` in case the app ID is not defined.

### `App-ResourceFile` {#fun-app-resourcefile}

* Description: This function returns the string value of the
  [`ResourceName`](/ref/app-properties/#ResourceName) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-ResourceArchive` {#fun-app-resourcearchive}

* Description: This function returns the string value of the
  [`ArchiveName`](/ref/app-properties/#ArchiveName) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-ResourceArchiveTyp` {#fun-app-resourcearchivetyp}

* Description: This function returns the string value of the
  [`ResourceArchiveTyp`](/ref/app-properties/#ResourceArchiveTyp) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-ResourceArchivePath` {#fun-app-resourcearchivepath}

* Description: This function returns the string value of the
  [`ArchivePath`](/ref/app-properties/#ArchivePath) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Force` {#fun-app-force}

* Description: This function returns the boolean value of the
  [`Force`](/ref/app-properties/#Force) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as boolean,
  or `$null` in case the app ID is not defined.

### `App-PackageName` {#fun-app-packagename}

* Description: This function returns the string value of the
  [`PackageName`](/ref/app-properties/#PackageName) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Dir` {#fun-app-dir}

* Description: This function returns the string value of the
  [`Dir`](/ref/app-properties/#Dir) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Path` {#fun-app-path}

* Description: This function returns the first element in the list value of the
  [`Path`](/ref/app-properties/#Path) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The first item in the array value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Paths` {#fun-app-paths}

* Description: This function returns the list value of the
  [`Path`](/ref/app-properties/#Path) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as array of strings,
  or `$null` in case the app ID is not defined.

### `App-Exe` {#fun-app-exe}

* Description: This function returns the string value of the
  [`Exe`](/ref/app-properties/#Exe) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-Register` {#fun-app-register}

* Description: This function returns the boolean value of the
  [`Register`](/ref/app-properties/#Register) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as boolean,
  or `$null` in case the app ID is not defined.

### `App-Environment` {#fun-app-environment}

* Description: This function returns the dictionary value of the
  [`Environment`](/ref/app-properties/#Environment) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as dictionary,
  or `$null` in case the app ID is not defined.

### `App-AdornedExecutables` {#fun-app-adornedexecutables}

* Description: This function returns the list value of the
  [`AdornedExecutables`](/ref/app-properties/#AdornedExecutables) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as array of strings,
  or `$null` in case the app ID is not defined.

### `App-RegistryKeys` {#fun-app-registrykeys}

* Description: This function returns the list value of the
  [`RegistryKeys`](/ref/app-properties/#RegistryKeys) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as array of strings,
  or `$null` in case the app ID is not defined.

### `App-Launcher` {#fun-app-launcher}

* Description: This function returns the string value of the
  [`Launcher`](/ref/app-properties/#Launcher) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-LauncherExecutable` {#fun-app-launcherexecutable}

* Description: This function returns the string value of the
  [`LauncherExecutable`](/ref/app-properties/#LauncherExecutable) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-LauncherArguments` {#fun-app-launcherarguments}

* Description: This function returns the list value of the
  [`LauncherArguments`](/ref/app-properties/#LauncherArguments) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as array of strings,
  or `$null` in case the app ID is not defined.

### `App-LauncherIcon` {#fun-app-launchericon}

* Description: This function returns the string value of the
  [`LauncherIcon`](/ref/app-properties/#LauncherIcon) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `App-SetupTestFile` {#fun-app-setuptestfile}

* Description: This function returns the string value of the
  [`SetupTestFile`](/ref/app-properties/#SetupTestFile) property.
* Parameter:
    + `app`: The app ID.
* Return Value: The value of the property as string,
  or `$null` in case the app ID is not defined.

### `Check-App` {#fun-check-app}

* Description: This function returns a boolean value, indicating if this app is installed.
* Parameter:
    + `app`: The app ID.
* Return Value: `$true` if the app is installed,
  `$false` if the app is not installed,
  and `$null` if the app ID is not defined.

### `App-CustomScriptFile` {#fun-app-customscriptfile}

* Description: This function retrieves a path to a custom script.
* Parameter:
    + `app`: The app ID.
    + `type`: The type of cstom script (e.g. `setup` or `env`).
* Return Value: An absolute path to the custom script
  or `$null` if this type of custom script does not exists for the specified app.

### `App-SetupResource` {#fun-app-setupresource}

* Description: This function retrieves a path to a setup resource for an app.
* Parameter:
    + `app`: The app ID.
    + `relativePath`: A relative path or simply the filename of the resource.
* Return Value: An absolute path to the resource
  or `$null` if the resource does not exists for the specified app.

A setup resource can be a file or a directory, which is used by custom scripts.
The setup resources of an app are included in the app library.

+++
date = "2017-02-03T18:00:00+02:00"
description = "The properties for the definition of a project"
title = "Project Properties"
draft = true
weight = 9
+++

<!--
#data-table /*/*/Description
#column Property: name(..)
#column Required: value(../Required)
-->

### ID {#ID}

* Description: A unique identifier for the project in the Bench environment 
* Data Type: string
* Required: `true`
* Default: none

### Label {#Label}

* Description: A human friendly name for the project
* Data Type: string
* Required: `false`
* Default: `ID`

### Path {#Path}

* Description: A path to the project root directory
* Data Type: path
* Required: `false`
* Default: `ID`

The given path can be relative or absolute.
If the path is relative, it is interpreted relative to the [`ProjectRootDir`](/ref/config/#ProjectRootDir).

### RepositoryType {#RepositoryType}

* Description: The type of repository to download the project from
* Data Type: string
* Possible Values:
    + `git`
    + `download`
* Required: `false`
* Default: `git`

### GitRemoteUrl {#GitRemoteUrl}

* Description: The URL of the remote repository to clone
* Data Type: URL
* Required: `false`
* Default: empty

Is required, if the `RepositoryType` is `git`.

### GitMainBranch {#MainBranch}

* Description: An explicit override to specify the branch to checkout
* Data Type: string
* Required: `false`
* Default: empty

If this value is empty, the default of the remote repository will be used.

### BuildCommand {#BuildCommand}

* Description: A custom command to build the project
* Data Type: string
* Required: `false`
* Default: empty

Active facets can provide additional build commands.

### EditCommand {#EditCommand}

* Description: A custom command to edit the project
* Data Type: string
* Required: `false`
* Default: empty

Active facets can provide additional edit commands.

### RunCommand {#RunCommand}

* Description: A custom command to run the project
* Data Type: string
* Required: `false`
* Default: empty

Active facets can provide additional run commands.

### Facets {#Facets}

* Description: A list with project facets to enable for the project.
* Data Type: string list
* Possible Values:
    + `gulp`
    + `grunt`
    + `msbuild`
    + `maven`
    + ...
* Required: `false`
* Default: empty

If this list is empty, facets will be activated by the facet auto detection.

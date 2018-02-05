+++
date = "2017-07-25T16:00:00+02:00"
description = "Import an existing project in the Bench environment"
title = "Importing a Project"
draft = true
weight = 9
+++

[Project Directory]: /ref/file-structure/#projects-dir
[Bench Shell]: /guide/shell
[Launcher]: /guide/launcher

Using the [Launcher][] and [Bench Shells][Bench Shell], you can work in any arbitrary
project, no matter where the project is stored in you file system.
But if you store you project inside the [project directory][] of the
Bench environment, you get some extra benefits.
If you already have a Git versioned project, that you want to use
in the Bench environment, you can use an action script to checkout the
project directly into the Bench [project directory][].
<!--more-->

**WARNING:**  
_This tutorial is obsolete for now, because the responsible scripts in the `actions` folder were removed from Bench during an earlier refactoring._  
_The way Bench supports projects is planned to be reimplementated._

----

The support for projects in Bench is not very mature yet.
But some support already exists.

**Overview**

<!-- #data-list /*/* -->

## Cloning a Git Repository {#git-clone}
To clone a Git versioned project from a remote URL into the
Bench [project directory][], you just need to run the action script
`actions\clone-git-project.cmd`.

It asks for the URL of your remote repository and for the name of the
directory to clone into. Then it calls git for the rest.
When the project was cloned successfully, it opens a shell in the
project directory and if it recognizes a known project type,
the project is opened in the associated IDE or the default text editor.

Take the following trade-off into account, when you have the choice
between HTTP(S) and SSH based URLs.
With HTTP(S) URLs you have the inconvenience to provide you username
and password every time
&ndash; but SSH based URLs do not work behind a proxy firewall.

## Creating a SSH Key Pair {#ssh-key}
If you use a SSH remote URL, you must make sure to have an SSH
key pair ready in your `home\.ssh` directory.
And the public key of the key pair must be known to the remote host.

To create a new SSH key pair, open the CMD [Bench Shell][] and run:

```cmd
cd /D %BENCH_APPS%\git\usr\bin
ssh-keygen -t rsa -b 2048 -C "%USERNAME% (Bench) <%USEREMAIL%>"
```

Now you can provide the public key in `home\.ssh\id_rsa.pub` to your remote host.
Open the `id_rsa.pub` in your favorite text editor and copy the content to the clipboard.
Now you must provide the public key to the remote server.
If the remote host is GitHub or GitLab for example, login on the website of the server,
go to your account settings, and add a new SSH key.
As text for the key, paste the content from the clipboard.

## Importing a Project Manually {#manually}
You can always just move a project folder into the [project directory][]
of your Bench environment manually, or clone a Git project by yourself,
using a [Bench Shell][] and the `git` command.

Generally speaking, every sub-directory in the Bench [project directory][]
is recognized by Bench as a project.

## See Also

Tech Guides

* [Bench Shell][]
* [Launcher][]

Reference Docs

* [File Structure](/ref/file-structure)

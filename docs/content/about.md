+++
title = "About"
date = "2016-06-18"
description = "Why does Bench exist?"
+++

Bench is a portable environment for software development on Windows.

The recurring pain to install and configure numerous command-line tools
and other programs under Windows with all its pit falls and side-effects
led to the development of Bench.
It evolved from a collection of some PowerShell scripts, into a .NET library,
a CLI, and a GUI for quick and easy configuration of a development environment.
It aims to feel comfortable from the command-lines perspective but
at the same time to be easy to use via a graphical interface.

![Screenshot](/img/teaser.png)

Bench is hosted on [GitHub](https://github.com/winbench/bench/)
and is primarily developed by [Tobias Kiertscher](http://www.mastersign.de/).

## Everything is an App
Bench exspects a program to be defined by a small number of [properties](/ref/app-properties)
like name, version, download URL, archive type, executable file name and others.
The definition of a programm is called _app_.

Bench downloads, extracts, configures and links apps for you.
Bench by-passes the different setup and installation programs and works with
its own setup process.

## Batteries Included
Bench comes with a number of [predefined apps](/apps) you just need to activate:

Git, Node.js, Python, PHP, Ruby, Go, JDK, Maven, Leinigen, MinGW, Clang, MiKTeX,
Eclipse, Visual Studio Code, Sublime Text 3, Emacs, GIMP, Inkscape,
FFmpeg, GraphicsMagick, OpenSSL, GnuPG, ...

Take a look in the [list of included apps](/apps).

If the app you need is not included in the app library of Bench you can easily
define you own custom apps.

## Isolation
Bench, by default, only changes the content of its root folder.
It does not change a thing outside of its root folder if you do not want it to.

Bench uses different mechanisms to isolate its apps from your Windows installation
and your user account.
It overrides a number of environment variables like
`PATH`, `HOME`, `USERPROFILE`, `APPDATA`, `LOCALAPPDATA`, `TEMP`.
And it supports a transparent backup and restore mechanism for registry keys.

## Portability
Because Bench by default does not change your Windows installation
but only changes files in its root folder, it can be easily moved
and also used on a portable drive.

The only thing you have to do, when the path to the Bench root folder changes,
is run the [environment update action](/ref/bench-cli/#cmd_bench-manage-update-env).

+++
date = "2016-11-18T18:05:00+02:00"
description = "How do I get a project started?"
title = "Setting-Up a Project"
weight = 3
+++

[Launcher]: /guide/launcher
[Bench Shell]: /guide/shell
[Yeoman]: https://yeoman.io
[Generator]: http://yeoman.io/generators/
[Project Directory]: /ref/file-structure/#projects-dir
[Creating a New Project]: /tutorial/project-new/
[Importing an Existing Project]: /tutorial/project-import/

If you want to **create a new project with an IDE** like Eclipse,
open the IDE via its [Launcher][] and initialize the project
from within the IDE.

If you want to **scaffold a project with [Yeoman][]**,
first install the [generator][] of your choice in a [Bench Shell][] via `npm`.
Then you can run the action script `actions\new-project.cmd`:

* It will create the project directory
* Run Yeoman to scaffold the file structure
* Initialize your project as a Git working copy
* Commit the initial state to the `master` branch
* Open a [Bench Shell][] in your projects directory
* Open the appropriate IDE or the default text editor, when supported

If you want to **scaffold a project with another command line tool**
like NPM, Leiningen, or CMake,
you have to create the project folder in the Bench [project directory][]
and call the scaffolding tool from a [Bench Shell][].

If you want to **clone an existing Git project**,
run the action script `actions\clone-git-project.cmd`:

* It will ask you for the remote URL
* Propose a name for the project directory
* Clone the project by calling Git
* Open a [Bench Shell][] in the project directory
* Open the appropriate IDE or the default text editor, when supported

You can find a more detailed description of setting-up a project in the tutorials
[Creating a New Project][] and [Importing an Existing Project][].

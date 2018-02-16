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

If you want to **create a new project manually or with a command line tool**,
first open a shell from the Bench Dashboard.
Your current working path should be the `projects` directory in the Bench root directory.
If that is not the case use `cd` to switch to it.
Then create your project folder via `mkdir yourproject`, change your location
into it via `cd yourproject` and optionally call a scaffolding tool, e.g. `yo` for Yeoman.

If you want to **clone an existing Git project**,
first open a shell from the Bench Dashboard.
Your current working path should be the `projects` directory in the Bench root directory.
If that is not the case use `cd` to switch to it.
Then just use the `git clone` command to clone your project into a new project folder.

E.g. `git clone https://github.com/yourname/yourproject.git yourproject`

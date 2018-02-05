+++
date = "2017-07-25T16:00:00+02:00"
description = "Create and initialize a new project in the Bench environment"
title = "Creating a New Project"
draft = true
weight = 8
+++

[Yeoman]: https://yeoman.io
[Node.js]: https://nodejs.org
[Generators]: http://yeoman.io/generators/
[Project Directory]: /ref/file-structure/#projects-dir
[Bench Shell]: /guide/shell
[NPM Package Install]: https://docs.npmjs.com/getting-started/installing-npm-packages-locally
[Custom Apps]: /tutorial/apps-custom

To start a new project, you usually need to initialize some kind of
file structure. Depending on the IDE or editor, and the programming language,
you intend to use, the file structure might differ.
Because Bench does not know what kind of projects you want to create,
it trys to assist you with scaffolding apps.
<!--more-->

**WARNING:**  
_This tutorial is obsolete for now, because the responsible scripts in the `actions` folder were removed from Bench during an earlier refactoring._  
_The way Bench supports projects is planned to be reimplementated._

----

Currently the project support in Bench is quite limited.
To create a new project, Bench utilizes the scaffolding app [Yeoman][].
Therefore, to be able to create a scaffolded project, make sure you
have [activated and installed](/tutorial/apps) Yeoman in Bench.

**Overview**

<!-- #data-list /*/* -->

## Yeoman Generators {#yo-generators}
[Yeoman][] is a [Node.js][] package, which itself depends on [generators][],
responsible for scaffolding all sorts of projects.
So checkout the generators and use the [npm package manager][npm package install]
to install the generators you need.

Example for a generator scaffolding TypeScript projects for Node.JS modules:

```cmd
npm install -g generator-node-typescript
```

**Pro Tip:**
If you use some generators regularly, consider [defining an app][custom apps] for them.

## Scaffolding a Project with Yeoman {#yo}
To scaffold a new project in the Bench environment with Yeoman,
run the action script `actions\new-project.cmd`.
At first, you are ask for the name of the project, you want to initialize.
Then Yeoman is called.

When Yeoman is run the first time, he asks you politly to allow him to send
some telemetry home.
This out of the way, he gets to business:
He presents you with a menu to select one of the installed generators.
Depending on the generator, you get ask some more questions to configure your
new project.
Then Yeoman initializes the files and also downloads dependencies.
The project is stored as a sub-directory in the Bench [project directory][].

When Yeoman is done, your project is initialized as a Git working copy,
and the initial state is commited immediately to the `master` branch.
Then a [Bench Shell][] is opened with the current location
beeing you new project directory.
If Bench recognizes a known project type, it trys to open the project in
the appropriate IDE or the default text editor.

## See Also

Tutorials

* [Selecting and Installing Apps](/tutorial/apps)

Tech Guides

* [Bench Shell](/guide/shell)

Reference Docs

* [File Structure](/ref/file-structure)

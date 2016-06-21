# Documentation Structure

The documentation is organized in 5 levels of detail.
Each level uses its own page type.

1. Homepage `home`
2. Quick Start `start`
3. Tutorials `tutorial`
4. Guides `guide`
5. References `ref`

Additionally there are the following top-level pages.

* `about` Project description with link to GitHub  
  Icon: info
* `scenarios` A collection of scenarios where Bench is helpful  
  Icon: ?
* `overview` Explanation of the detail levels in the documentation  
  Icon: page/book
* `contact` Contact information for Tobias Kiertscher  
  Icon: telephone

## Homepage `home`

Icon: house  
Color: red

The homepage should tease the reader and answer the following questions.
The homepage is primary linked to the Quickstarts and top-level pages.

* `intro` What is Bench? &rarr; `about`
* `usage` What does Bench for me? &rarr; `scenarios`
* `install` How do I get it running? &rarr; `start/install`
* `docs` Where do I find more infos? &rarr; `overview`

## Quickstart `start`

_How do I get started?_

Icon: speed arrow  
Color: organge

The Quickstart should contain a short version of selected tutorials.
The Quickstarts are chained, to guide the user to the next step.
A Quickstart is primary linked to the tutorials for _step-by-step descritions_.

* `install` Installation
* `apps` Selecting and installing apps
* `project` Creating a new project
* `work` Working in the context of a project

## Tutorials `tutorial`

_How can I use it?_

Icon: tutor head/wizzard  
Color: green

Tutorials should be in step-by-step form and allow the user to complete the following tasks.
A tutorial is primary linked to the guides to explain _how_ things work.

* `install` Installation
    + Bootstrap
    + Bench site initialization
    + First setup
    + Configuration initialization
        - Clean
        - From Git existing repo
* `apps` Selecting and installing apps
* `app-custom` Defining a custom app
* `shell-start` Ways of starting a shell
* `app-start` Ways of starting an app
* `project-new` Creating a new project
* `project-import` Importing an existing project into Bench
* `project-work` Working in the context of a project
    + From an editor or IDE
    + From the shell
* `portable` Using Bench on a USB drive
* `multi-env` Using multiple Bench environments side by side

## Guides `guide`

_How does it work?_

Icon: lightbulb  
Color: blue

Guides should provide in-depth information about different views of the system.
Guides should link primary to the references for _details_.

* `architecture` System architecture
    + Components
    + Folder structure
* `setup` Bench setup and upgrade process
    + Execution of Hooks
* `app-setup` App setup and upgrade process
    + Actions
    + Extraction of resources
    + Custom scripts
* `isolation` Isolation and portability
    + Environment variables
    + Registry isolation by executable adornment
    + Levels of configuration
* `selection` Selecting apps and app groups
    + Dependencies
    + `apps_activated.txt` and `apps_deactivated.txt`
    + Setup dialog of Bench Dashboard
* `shell` Shells
* `launcher` Launchers and execution proxies
* `project-initializer` Project initializers
* `project-action` Project actions

## References `ref`

_Technical details_

Icon: library  
Color: violet

References cover components or processes of Bench in detail.

* `apps` Included apps and app groups
* `bench-ctl` Command-line management tool `bench-ctl`
* `dashboard` Bench Dashboard
    + Main window
    + Setup dialog
    + App info dialog
    + App list windows
* `config-syntax` Markdown syntax for `res\config.md`, `config\config.md`, and `bench-site.md`
* `config` Configuration properties
    + Customizable properties should be marked
* `apps-syntax` Markdown syntax for `res\apps.md` and `config\apps.md`
* `files` Important files
    + `config.md`
    + ...
* `app-types` App types
* `apps-properties` App properties
* `ps-api` PowerShell API for custom scripts (`auto\apps\<app>.<action>.ps1`) and hooks (`config\<action>.ps1`)
* `lib-api` CLR API of `BenchLib.dll`

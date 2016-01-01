# Bench

```
    /==================================================\    
    ||       _____________________________________    ||    
    ||      /   _    __       __                 /|   ||    
    ||     /   /_)  /_  /| / /   /_/            //    ||    
    ||    /   /_)  /_  / |/ |_  / /   o        //|    ||    
    ||   /____________________________|_______//||    ||    
    ||   |____________________________O______|/ ||    ||    
    ||     | || | ||                  |  | || | ||    ||    
    ||     | || |_|/                  °  | || |_|/    ||    
    ||     | ||                          | ||         ||    
    ||     | ||      LET'S GET BUSY!     | ||         ||    
    ||     |_|/                          |_|/         ||    
    ||                                                ||    
    \==================================================/    
```

> Portable Environment for Software Development on Windows

## Prerequisites

* Microsoft Windows 7 or higher (with at least PowerShell 2)
* Internet access for HTTP/HTTPS (possibly via proxy, but without credentials)
* The Windows user account does _not_ need admin priviledges
* A harddrive or a removable drive with at least 2 GB free memory

## Install

* Create a folder on a drive with at least 2 GB space, maybe call it `bench`.
* Save the bootstrap file [bench.bat](https://github.com/mastersign/bench/raw/master/res/bench.bat)
  inside this folder.
* Run the bootstrap file.
  (You probably have to open the properties dialog for the downloaded file, and allow the execution first.)
* Follow the instructions ...
    + Enter your name and email address for your [Git] identity.
    + Configure the HTTP proxy server in the `config.ps1` if you are behind a proxy firewall.
    + Maybe activate or deactivate some apps in the `config.ps1`.
    + Wait for _Bench_ to  download tools and finish setup
* Run the `new-project.cmd` action to scaffold a project with [Yeoman]:
  Choose the _MdProc_ generator to build a [Markdown] documentation project.
* Run the `clonte-git-project.cmd` action to work on an existing project.

## Apps

*Bench* downloads a selection of commandline tools and provides an isolated
path environment with all tools available.

Take a look at the [app registry](res/apps.md).

[Yeoman]: http://yeoman.io "The web's scaffolding tool for modern web apps"
[Markdown]: https://daringfireball.net/projects/markdown/
[Git]: https://git-scm.com
# Bench Bootstrap

The PowerShell scripts in this folder are intended to be used in scenarios,
where Bench must be installed by a single shell command without user interaction.
An example would be an unattended Windows installation.

The scripts install Bench in `%SystemDrive%\Bench` (usually `C:\Bench`).
Bench is installed with _Git_, _Python 3_, and _NodeJS_ activated.

## Isolation Levels

There are three scripts for different isolation levels.

* `isolated.ps1`  
  _Use this if you want to prevent any interference between the Bench apps and the Windows user profile._  
  All isolation features of Bench are activated:
    + separate home (user profile) and temp folder
    + executables on the `PATH` variable of the Windows user profile are **not** available inside the Bench environment
    + and for some apps registry isolation (e. g. Putty)
* `default.ps1`  
  _Use this, if you are not sure._  
  Some isolation features of Bench are activated:
    + separate home and temp folder
    + executables on the `PATH` variable of the Windows user profile are available inside the Bench environment
    + no registry isolation
* `integrated.ps1`  
  _Use this if you want to use Bench as an app manager for your Windows user profile._  
  No isolation features:
    + home (user profile) and temp folders of the Windows user profile are used by Bench apps
    + Bench apps are added to the `PATH` variable of the Windows user profile
    + no registry isolation

## Usage

The shell commands to execute the scripts are

* Isolated:  
  `powershell.exe -NoLogo -ExecutionPolicy ByPass -Command "iex (iwr https://raw.githubusercontent.com/winbench/bench/master/bootstrap/isolated.ps1).Content"`
* Default:  
  `powershell.exe -NoLogo -ExecutionPolicy ByPass -Command "iex (iwr https://raw.githubusercontent.com/winbench/bench/master/bootstrap/default.ps1).Content"`
* Integrated:  
  `powershell.exe -NoLogo -ExecutionPolicy ByPass -Command "iex (iwr https://raw.githubusercontent.com/winbench/bench/master/bootstrap/integrated.ps1).Content"`

You can use them inside of a `*.bat` or `*.cmd` script or in any other place where a shell command is allowed.

## Customization

You can take one of these scripts, change the activated apps and config file templates inside it,
and host it with an HTTP server at another location.

Then adapt the URL in the shell command to run it.

Examples, what you could do by that:

* Select another set of initially activated apps
* Change the Git user (`"User" <user@localhost>`)
* Include your own app library
* Add environment variables to the Bench environment
* Tweak the Bench configuration
* Override app properties

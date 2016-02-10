﻿$launcherDir = Safe-Dir (Get-ConfigPathValue LauncherDir)
$launcherScriptDir = Safe-Dir ([IO.Path]::Combine((Get-ConfigPathValue BenchAuto), "launcher"))

$wshShell = New-Object -ComObject WScript.Shell

function Clean-Launchers() {
    Debug "Cleaning launcher shortcuts: $launcherDir\*"
    $_ = Empty-Dir $launcherDir
    Debug "Cleaning launcher scripts: $launcherScriptDir\*"
    $_ = Empty-Dir $launcherScriptDir
}

function Get-LauncherScriptFile([string]$name) {
    return [IO.Path]::Combine($launcherScriptDir, ($name.ToLowerInvariant() + '.cmd'))
}

function Get-LauncherFile([string]$name) {
    return [IO.Path]::Combine($launcherDir, (App-Launcher $name) + '.lnk')
}

function Get-LauncherTarget([string]$name) {
    $path = App-LauncherExecutable $name
    $adornedExecutables = App-AdornedExecutables $name
    if ($adornedExecutables) {
        $p1 = Resolve-Path $path
        foreach ($exe in $adornedExecutables) {
            $p2 = Resolve-Path $exe
            if ([string]::Equals($p1, $p2, [StringComparison]::OrdinalIgnoreCase)) {
                return Get-ExecutableProxy $name $path
            }
        }
    }
    return $path
}

function Create-LauncherScript([string]$name)
{
    $launcherLabel = App-Launcher $name
    if (!$launcherLabel) {
        return
    }
    Debug "Writing launcher script for '$launcherLabel' ..."
    $executable = Get-LauncherTarget $name
    Debug "Path of launcher target: $executable"
    $arguments = App-LauncherArguments $name | % {
        $arg = $_.Replace('"', '^"')
        if ($_ -match "\s") {
            return "`"$arg`""
        } else {
            return $arg
        }
    }
    $argumentsString = [string]::Join(' ', $arguments)
    $launcherScriptFile = Get-LauncherScriptFile $name
    Debug "Path of launcher script: $launcherScriptFile"
    $nl = [Environment]::NewLine
    $code = "@ECHO OFF$nl"
    $code += "ECHO.Launching $launcherLabel in Bench Context ...$nl"
    $code += "CALL `"%~dp0..\env.cmd`"$nl"
    if ($executable.EndsWith(".cmd", [StringComparison]::OrdinalIgnoreCase)) {
        $code += "START `"$launcherLabel`" CMD /C `"$executable`" $argumentsString$nl"
    } else {
        $code += "START `"$launcherLabel`" `"$executable`" $argumentsString$nl"
    }
    [IO.File]::WriteAllText($launcherScriptFile, $code, [Text.Encoding]::Default)
}

function Create-Shortcut([string]$file, [string]$targetPath, [string]$arguments="",
                         [string]$workingDir=$null, [string]$iconPath=$targetPath,
                         [int]$windowStyle=1) {

    try {
        $shortcut = $wshShell.CreateShortcut($file)
        $shortcut.TargetPath = $targetPath
        if ($arguments) {
            $shortcut.Arguments = $arguments
        }
        if ($workingDir) {
            $shortcut.WorkingDirectory = $workingDir
        }
        if ($iconPath) {
            $shortcut.IconLocation = $iconPath
        }
        $shortcut.WindowStyle = $windowStyle # 1 Default, 3 Maximized, 7 Minimized
        $shortcut.Save()
        Debug "Create shortcut $file"
    } catch {
        Debug "Arguments: $([string]::Join(', ', '`"' + $arguments + '`"'))"
        Debug "Working Dir: '$workingDir'"
        Debug "Icon Path: '$iconPath'"
        Debug "Window Style: $windowStyle"
        Write-Warning $_.Exception.Message
    }
}

function Create-Launcher([string]$name) {
    $launcherLabel = App-Launcher $name
    if (!$launcherLabel) {
        return
    }
    Create-LauncherScript $name
    Debug "Creating launcher for '$launcherLabel' ..."
    $launcherFile = Get-LauncherFile $name
    Debug "Path of launcher: $launcherFile"

    Create-Shortcut $launcherFile (Get-LauncherScriptFile $name) `
      -workingDir (Get-ConfigPathValue BenchRoot) `
      -iconPath (App-LauncherIcon $name) -windowStyle 7
}

function Create-ActionLauncher($label, $action, $icon) {
    $launcherFile = [IO.Path]::Combine($launcherDir, $label + '.lnk')
    $targetPath = [IO.Path]::Combine((Get-ConfigPathValue BenchRoot), "$action.cmd")
    Debug "Creating launcher for '$label' ..."
    Create-Shortcut $launcherFile $targetPath `
      -workingDir (Get-ConfigPathValue BenchRoot) `
      -iconPath $icon
}

function Create-ActionLaunchers() {
    Create-ActionLauncher 'Bench Control' 'bench-ctl' '%SystemRoot%\System32\imageres.dll,109'
    Create-ActionLauncher 'Command Line' 'bench-cmd' '%SystemRoot%\System32\cmd.exe'
    Create-ActionLauncher 'PowerShell' 'bench-ps' '%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe'
    Create-ActionLauncher 'Bourne Again Shell' 'bench-bash' '%SystemRoot%\System32\imageres.dll,89'
}

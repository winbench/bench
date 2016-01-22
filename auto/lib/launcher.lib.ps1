$launcherDir = Empty-Dir (Get-ConfigPathValue LauncherDir)
$launcherScriptDir = Safe-Dir ([IO.Path]::Combine((Get-ConfigPathValue BenchAuto), "launcher"))

$wshShell = New-Object -ComObject WScript.Shell

function Get-LauncherScriptFile([string]$name) {
    return [IO.Path]::Combine($launcherScriptDir, ($name.ToLowerInvariant() + '.cmd'))
}

function Get-LauncherFile([string]$name) {
    return [IO.Path]::Combine($launcherDir, (App-Launcher $name) + '.lnk')
}

function Create-LauncherScript([string]$name)
{
    $launcherLabel = App-Launcher $name
    if (!$launcherLabel) {
        return
    }
    Debug "Writing launcher script for '$launcherLabel' ..."
    $executable = App-LauncherExecutable $name
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
    $code += "START `"$launcherLabel`" `"$executable`" $argumentsString$nl"
    [IO.File]::WriteAllText($launcherScriptFile, $code, [Text.Encoding]::Default)
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

    $shortcut = $wshShell.CreateShortcut($launcherFile)
    $shortcut.TargetPath = Get-LauncherScriptFile $name
    $shortcut.WindowStyle = 7 # Minimized
    $shortcut.WorkingDirectory = Get-ConfigPathValue BenchRoot
    $shortcut.IconLocation = App-LauncherIcon $name
    $shortcut.Save()
}

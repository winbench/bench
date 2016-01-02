﻿param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"

Set-Debugging $debug

$editor = App-Exe (Get-ConfigValue EditorApp)
if (!$editor) {
    throw "Edtor not found"
} else {
    $editor = [IO.Path]::GetFileName($editor)
}

cd $Script:rootDir

Run-Detached $editor @args

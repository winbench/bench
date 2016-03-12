function App-Typ([string]$name) {
    return $Script:cfg.Apps[$name].Typ
}

function App-Version([string]$name) {
    return $Script:cfg.Apps[$name].Version
}

function App-Dependencies([string]$name) {
    return $Script:cfg.Apps[$name].Dependencies
}

function App-Url([string]$name) {
    return $Script:cfg.Apps[$name].Url
}

function App-DownloadHeaders([string]$name) {
    return $Script:cfg.Apps[$name].DownloadHeaders
}

function App-DownloadCookies([string]$name) {
    return $Script:cfg.Apps[$name].DownloadCookies
}

function App-ResourceFile([string]$name) {
    return $Script:cfg.Apps[$name].ResourceFileName
}

function App-ResourceArchive([string]$name) {
    return $Script:cfg.Apps[$name].ResourceArchiveName
}

function App-ResourceArchiveTyp([string]$name) {
    return $Script:cfg.Apps[$name].ResourceArchiveTyp
}

function App-ResourceArchiveSubDir([string]$name) {
    return $Script:cfg.Apps[$name].ResourceArchivePath
}

function App-Force([string]$name) {
    return $Script:cfg.Apps[$name].Force
}

function App-NpmPackage([string]$name) {
    return $Script:cfg.Apps[$name].PackageName
}

function App-PyPiPackage([string]$name) {
    return $Script:cfg.Apps[$name].PackageName
}

function App-Dir([string]$name) {
    return $Script:cfg.Apps[$name].Dir
}

function App-Path([string]$name) {
    $path = $Script:cfg.Apps[$name].Path
	if ($path.Length -gt 0) {
		return $path[0]
	} else {
		return $null
	}
}

function App-Paths([string]$name) {
    return $Script:cfg.Apps[$name].Path
}

function App-Exe([string]$name, [bool]$checkExist = $true) {
    return $Script:cfg.Apps[$name].Exe
}

function App-Register([string]$name) {
    return $Script:cfg.Apps[$name].Register
}

function App-Environment([string]$name) {
    return $Script:cfg.Apps[$name].Environment
}

function App-AdornedExecutables([string]$name) {
    return $Script:cfg.Apps[$name].AdornedExecutables
}

function App-RegistryKeys([string]$name) {
    return $Script:cfg.Apps[$name].RegistryKeys
}

function App-Launcher([string]$name) {
    return $Script:cfg.Apps[$name].Launcher
}

function App-LauncherExecutable([string]$name) {
    return $Script:cfg.Apps[$name].LauncherExecutable
}

function App-LauncherArguments([string]$name) {
    return $Script:cfg.Apps[$name].LauncherArguments
}

function App-LauncherIcon([string]$name) {
    return $Script:cfg.Apps[$name].LauncherIcon
}

function App-SetupTestFile([string]$name) {
    return $Script:cfg.Apps[$name].SetupTestFile
}

function Check-DefaultApp([string]$name) {
    return $Script:cfg.Apps[$name].IsInstalled
}

function Check-NpmPackage([string]$name) {
    return $Script:cfg.Apps[$name].IsInstalled
}

function Check-PyPiPackage ([string]$pythonVersion, [string]$name) {
    return $Script:cfg.Apps[$name].IsInstalled
}

function Check-App([string]$name) {
    return $Script:cfg.Apps[$name].IsInstalled
}

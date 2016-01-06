param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"
. "$scriptsLib\env.lib.ps1"

Set-Debugging $debug

$winShell = New-Object -ComObject Shell.Application

if (!(Test-Path $downloadDir)) { return }
if (!(Test-Path $libDir)) { return }

function Find-Download([string]$pattern) {
    $path = Find-File $Script:downloadDir $pattern
    if (!$path) {
        throw "Download not found: $pattern"
    } else {
        Debug "Found download: $path"
    }
    return $path
}

function ShellUnzip-Archive([string]$zipFile, [string]$targetDir)
{
    Debug "Extracting (Shell) $zipFile to $targetDir"
    $zip = ${Script:winShell}.NameSpace($zipFile)
    if (!$zip) {
        throw "Invalid ZIP file: $zipFile"
    }
    $trg = ${Script:winShell}.NameSpace($targetDir)
    if (!$trg) {
        throw "Invalid target directory: $targetDir"
    }
    foreach($item in $zip.items())
    {
        $trg.copyhere($item)
    }
}

function Extract-Archive([string]$archive, [string]$targetDir) {
    $7z = App-Exe SvZ
    $targetDir = Safe-Dir $targetDir
    if ($7z) {
        Debug "Extracting $archive to $targetDir"
        & $7z "x" "-y" "-o$targetDir" "$archive" | Out-Null
        if (!$?) {
            throw "Extracting $archive failed"
        }
    } else {
        ShellUnzip-Archive $archive $targetDir
    }
}

function Extract-Msi([string]$archive, [string]$targetDir) {
    Debug "Extracting MSI $archive to $targetDir"
    $targetDir = Safe-Dir $targetDir
    $lessmsi = App-Exe LessMsi
    if ($lessmsi) {
        pushd $targetDir
        & $lessmsi "x" $archive ".\" | Out-Null
        if (!$?) {
            throw "Extracting $archive failed"
        }
        popd
    } else {
        throw "Missing LessMsi for MSI extraction"
    }
}

function Execute-Custom-Setup([string]$name) {
    $customSetupFile = "$scriptsLib\..\apps\${name}.setup.ps1"
    if (Test-Path $customSetupFile) {
        Debug "Running custom setup for $name ..."
        $old = Set-StopOnError $false
        . $customSetupFile
        $_ = Set-StopOnError $old
        Debug "Finished custom setup for $name"
    }
}

function Default-Setup([string]$name, [bool]$registerPath = $true) {
    $dir = App-Dir $name
    if ((App-Force $name) -or !(Check-DefaultApp $name)) {
        Write-Host "Setting up $name ..."
        
        $download = App-Download $name
        if ($download) {
            [string]$src = Find-Download $download
            $mode = "copy"
            $subDir = $null
        } else {
            $archive = App-Archive $name
            [string]$src = Find-Download $archive
            if ($src.EndsWith(".msi", [StringComparison]::InvariantCultureIgnoreCase)) {
                $mode = "msi"
            } else {
                $mode = "arch"
            }
            $subDir = App-ArchiveSubDir $name
        }
        
        $dir = Safe-Dir $dir
        if ($subDir) {
            $target = Safe-Dir "$(Get-ConfigPathValue TempDir)\$name"
        } else {
            $target = $dir
        }
        switch ($mode) {
            "copy" { Copy-item $src $target }
            "arch" { Extract-Archive $src $target }
            "msi" { Extract-Msi $src $target }
        }
        if ($subDir) {
            Move-Item "$target\$subDir\*" "$dir\"
            Purge-Dir $target
        }

    } else {
        Write-Host "Skipping allready installed $name in $dir"
    }

    Register-AppPaths $name
    Execute-Custom-Setup $name
}

function Setup-NpmPackage([string]$name) {
    $packageName = App-NpmPackage $name
    $npm = App-Exe Npm
    $version = App-Version $name
    if (!$npm) { throw "Node Package Manager not found" }
    if ((App-Force $name) -or !(Check-NpmPackage $name)) {
        if ($version) {
            Write-Host "Setting up npm package $packageName@$version ..."
            & $npm install "$packageName@$version" --global
        } else {
            Write-Host "Setting up npm package $packageName ..."
            & $npm install $packageName --global
        }
    } else {
        if ($version) {
            Write-Host "Skipping allready installed NPM package $packageName@$version"
        } else {
            Write-Host "Skipping allready installed NPM package $packageName"
        }
    }
    Execute-Custom-Setup $name
}

Load-Environment
Update-EnvironmentPath
$failedApps = @()
$installedApps = @()
foreach ($name in $Script:apps) {
    $typ = App-Typ $name
    switch ($typ) {
        "node-package" {
            try {
                Setup-NpmPackage $name
                $installedApps += $name
            } catch {
                Write-Warning "Installing NPM Package $name failed: $($_.Exception.Message)"
                $failedApps += $name
            }
        }
        default { 
            try {
                Default-Setup $name
                $installedApps += $name
            } catch {
                Write-Warning "Setting up $name failed: $($_.Exception.Message)"
                $failedApps += $name
            }
        }
    }
    Update-EnvironmentPath
}
Write-EnvironmentFile

Purge-Dir $tempDir

Write-Host "$($installedApps.Count) of $($apps.Count) apps successfully installed."
if ($failedApps.Count -gt 0) {
    Write-Warning "Setting up the following $($failedApps.Count) apps failed:"
    foreach ($name in $failedApps) {
        Write-Warning " - $name"
    }
    Write-Warning "Run 'auto/bench-setup.cmd' to try again."
}

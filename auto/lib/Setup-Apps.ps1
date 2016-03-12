param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\env.lib.ps1"
. "$scriptsLib\adornment.lib.ps1"
. "$scriptsLib\reg.lib.ps1"
. "$scriptsLib\launcher.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$winShell = New-Object -ComObject Shell.Application

if (!(Test-Path $downloadDir)) { return }
if (!(Test-Path $libDir)) { return }


function ShellUnzip-Archive([string]$zipFile, [string]$targetDir) {
    Debug "Extracting (Shell) $zipFile to $targetDir"
    $zip = ${Script:winShell}.NameSpace($zipFile)
    if (!$zip) {
        throw "Invalid ZIP file: $zipFile"
    }
    $trg = ${Script:winShell}.NameSpace($targetDir)
    if (!$trg) {
        throw "Invalid target directory: $targetDir"
    }
    foreach($item in $zip.items()) {
        if (!$item) { continue }
        $trg.copyhere($item)
    }
}

function Extract-Archive([string]$archive, [string]$targetDir) {
    $7z = App-Exe SvZ
    $targetDir = Safe-Dir $targetDir
    if ($7z) {
        Debug "Extracting $archive to $targetDir"
        if ($archive -match "\.tar\.\w+$") {
            $tmpDir = "$(Get-ConfigPathValue TempDir)\${name}_tar"
            Empty-Dir $tmpDir | Out-Null
            & $7z "x" "-y" "-o$tmpDir" "$archive" | Out-Null
            $tarFile = Get-ChildItem "$tmpDir\*.tar"
            & $7z "x" "-y" "-o$targetDir" "$tarFile" | Out-Null
            if (!$?) {
                throw "Extracting $archive failed"
            }
            if ($tarFile) {
                Remove-Item $tarFile
            }
        } else {
            & $7z "x" "-y" "-o$targetDir" "$archive" | Out-Null
            if (!$?) {
                throw "Extracting $archive failed"
            }
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
            throw "Extracting MSI $archive failed"
        }
        popd
    } else {
        throw "Missing LessMsi for MSI extraction"
    }
}

function Extract-InnoSetup([string]$archive, [string]$targetDir) {
    Debug "Extracting Inno Setup $archive to $targetDir"
    $targetDir = Safe-Dir $targetDir
    $innounp = App-Exe InnoUnp
    if ($innounp) {
        pushd $targetDir
        & $innounp -q -x $archive | Out-Null
        if (!$?) {
            throw "Extracting Inno Setup $archive failed"
        }
        popd
    } else {
        throw "Missing Inno Setup Unpacker"
    }
}

function Extract-Custom([string]$name, [string]$archive, [string]$targetDir) {
    Debug "Extracing custom archive $archive to $targetDir"
    . "$scriptsLib\..\apps\${name}.extract.ps1" $archive $targetDir
}

function Execute-AppCustomSetup([string]$name) {
    $customSetupFile = "$scriptsLib\..\apps\$($name.ToLowerInvariant()).setup.ps1"
    Debug "Searching for custom setup script 'apps\$($name.ToLowerInvariant()).setup.ps1'"
    if (Test-Path $customSetupFile) {
        Debug "Running custom setup for $name ..."
        $old = Set-StopOnError $false
        . $customSetupFile
        $_ = Set-StopOnError $old
        Debug "Finished custom setup for $name"
    }
}

function Find-DownloadedFile([string]$pattern) {
    if (!$pattern) {
        return $null
    }
    $path = Find-File (Get-ConfigPathValue DownloadDir) $pattern
    if (!$path) {
        throw "Download not found: $pattern"
    } else {
        Debug "Found download: $path"
    }
    return [string]$path
}

function Setup-Common([string]$name) {
    Execute-AppCustomSetup $name
    Execute-AppEnvironmentSetup $name
    Setup-ExecutionProxies $name
    Create-Launcher $name
}

function Setup-MetaApp([string]$name) {
    Write-Host "Setting up meta app $name ..."
    Setup-Common $name
}

function Setup-DefaultApp([string]$name) {
    $dir = App-Dir $name
    if ((App-Force $name) -or !(Check-DefaultApp $name)) {
        Write-Host "Setting up $name ..."

        $download = App-ResourceFile $name
        if ($download) {
            $src = Find-DownloadedFile $download
            $mode = "copy"
            $subDir = $null
        } else {
            $archive = App-ResourceArchive $name
            $src = Find-DownloadedFile $archive
            $mode = App-ResourceArchiveTyp $name
            if ($mode.ToLower() -eq "auto") {
                if (Test-Path "$scriptsLib\..\apps\${name}.extract.ps1") {
                    $mode = "custom"
                } elseif ($src.EndsWith(".msi", [StringComparison]::InvariantCultureIgnoreCase)) {
                    $mode = "msi"
                } elseif ($src.EndsWith(".0")) {
                    $mode = "inno"
                } else {
                    $mode = "generic"
                }
            }
            $subDir = App-ResourceArchiveSubDir $name
        }

        $dir = Safe-Dir $dir
        if ($subDir) {
            $target = Safe-Dir "$(Get-ConfigPathValue TempDir)\$name"
        } else {
            $target = $dir
        }
        switch ($mode.ToLower()) {
            "copy" { Copy-item $src $target }
            "generic" { Extract-Archive $src $target }
            "msi" { Extract-Msi $src $target }
            "inno" { Extract-InnoSetup $src $target }
            "custom" { Extract-Custom $name $src $target }
        }
        if ($subDir) {
            Move-Item "$target\$subDir\*" "$dir\"
            Purge-Dir $target
        }

    } else {
        Debug "Skipping allready installed $name in $dir"
    }

    Setup-Common $name
}

function Setup-NpmPackage([string]$name) {
    $packageName = App-NpmPackage $name
    $npm = App-Exe Npm
    $version = App-Version $name
    $packageNameWithVersion = "${packageName}@${version}"
    if (!$npm) { throw "Node Package Manager not found" }
    if ((App-Force $name) -or !(Check-NpmPackage $name)) {
        if ($version) {
            Write-Host "Setting up NPM package $packageNameWithVersion ..."
            & $npm install "`"$packageNameWithVersion`"" --global
        } else {
            Write-Host "Setting up NPM package $packageName ..."
            & $npm install $packageName --global
        }
    } else {
        Debug "Skipping allready installed NPM package $packageNameWithVersion"
    }
    Setup-Common $name
}

function Setup-PyPiPackage([string]$pythonVersion, [string]$name) {
    $packageName = App-PyPiPackage $name
    $version = App-Version $name
    if ((App-Force $name) -or !(Check-PyPiPackage $pythonVersion $name)) {
        $python = "Python$pythonVersion"
        if ($Script:cfg.Apps[$python].IsActive) {
            $pip = "pip$pythonVersion"
            if ($version) {
                Write-Host "Setting up PyPI package $packageName $version for Python $pythonVersion"
                if (App-Force $name) {
                    & $pip install --upgrade $packageName "`"$version`""
                } else {
                    & $pip install $packageName "`"$version`""
                }
            } else {
                Write-Host "Setting up PyPI package $packageName for Python $pythonVersion"
                if (App-Force $name) {
                    & $pip install --upgrade $packageName
                } else {
                    & $pip install $packageName
                }
            }
        } else {
            Debug "Skipping PyPI package $packageName for inactive Python $pythonVersion"
        }
    } else {
        Debug "Skipping allready installed PyPI package $packageName $version"
    }
    Setup-Common $name
}

Clean-ExecutionProxies
Clean-Launchers
Create-ActionLaunchers
Load-Environment
$failedApps = @()
$installedApps = @()
$apps = $Script:cfg.Apps.ActiveApps
foreach ($app in $apps) {
    $name = $app.ID
    switch ($app.Typ) {
        "meta" {
            try {
                Setup-MetaApp $name
                $installedApps += $name
            } catch {
                Write-Warning "Installing App Group $name failed: $($_.Exception.Message)"
                Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
                $failedApps += $name
            }
        }
        "node-package" {
            try {
                Setup-NpmPackage $name
                $installedApps += $name
            } catch {
                Write-Warning "Installing NPM Package $name failed: $($_.Exception.Message)"
                Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
                $failedApps += $name
            }
        }
        "python2-package" {
            try {
                Setup-PyPiPackage 2 $name
                $installedApps += $name
            } catch {
                Write-Warning "Installing PyPI Package $name failed: $($_.Exception.Message)"
                Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
                $failedApps += $name
            }
        }
        "python3-package" {
            try {
                Setup-PyPiPackage 3 $name
                $installedApps += $name
            } catch {
                Write-Warning "Installing PyPI Package $name failed: $($_.Exception.Message)"
                Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
                $failedApps += $name
            }
        }
        default {
            try {
                Setup-DefaultApp $name
                $installedApps += $name
            } catch {
                Write-Warning "Setting up $name failed: $($_.Exception.Message)"
                Debug "$($_.Exception.Message)$($_.InvocationInfo.PositionMessage)"
                $failedApps += $name
            }
        }
    }
}
Write-EnvironmentFile

Empty-Dir (Get-ConfigPathValue TempDir) | Out-Null

Write-Host ""
Write-Host "$($installedApps.Count) of $($apps.Count) apps successfully installed."
if ($failedApps.Count -gt 0) {
    Write-Warning "Setting up the following $($failedApps.Count) apps failed:"
    foreach ($name in $failedApps) {
        Write-Warning " - $name"
    }
    Write-Warning "Run 'actions/bench-ctl.cmd setup' to try again."
}

Debug "Finished installing apps."

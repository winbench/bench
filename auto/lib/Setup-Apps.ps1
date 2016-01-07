param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"
. "$scriptsLib\env.lib.ps1"

Set-Debugging $debug

$winShell = New-Object -ComObject Shell.Application

if (!(Test-Path $downloadDir)) { return }
if (!(Test-Path $libDir)) { return }


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

function Default-Setup([string]$name, [bool]$registerPath = $true) {
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
        Write-Host "Skipping allready installed $name in $dir"
    }

    Register-AppPaths $name
    Register-AppEnvironment $name
    Load-AppEnvironment $name
    Execute-Custom-Setup $name
    Run-AppEnvironmentSetup $name
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
    Register-AppEnvironment $name
    Load-AppEnvironment $name
    Execute-Custom-Setup $name
    Run-AppEnvironmentSetup $name
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

Empty-Dir $tempDir | Out-Null

Write-Host ""
Write-Host "$($installedApps.Count) of $($apps.Count) apps successfully installed."
if ($failedApps.Count -gt 0) {
    Write-Warning "Setting up the following $($failedApps.Count) apps failed:"
    foreach ($name in $failedApps) {
        Write-Warning " - $name"
    }
    Write-Warning "Run 'auto/bench-setup.cmd' to try again."
}

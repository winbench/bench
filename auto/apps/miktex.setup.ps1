$mpm = "$(App-Path MikTeX)\mpm.exe"
if (!(Test-Path $mpm)) {
    throw "MikTeX Package Manager not found"
}

$packages = @(
    "koma-script",
    "upquote",
    "mathspec",
    "etoolbox",
    "l3kernel",
    "l3packages",
    "tipa",
    "xetex-def",
    "realscripts",
    "metalogo",
    "microtype",
    "url",
    "polyglossia",
    "makecmds",
    "fancyvrb",
    "booktabs"
)

function Extract-InstalledPackageNames() {
    begin {
        [regex]$ex = "\S+$"
    }
    process {
        if ($_.StartsWith("i ")) {
            $m = $ex.Match($_)
            if ($m.Success) {
                return $m.Value
            }
        }
    }
}

Write-Host "Listing installed LaTeX packages"
$installed = & $mpm --list | Extract-InstalledPackageNames

foreach ($package in $packages) {
    if (!($installed -contains $package)) {
        Write-Host "Installing LaTeX package $package"
        & $mpm "--install=$package"
        $installed = & $mpm --list | Extract-InstalledPackageNames
    }
}

param(
    [string[]]$TargetFiles
)

$ErrorActionPreference = 'Stop'
$TimeServer = "http://timestamp.sectigo.com"

$signToolSearchPath = "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\signtool.exe"

[string]$signtool = Get-ChildItem $signToolSearchPath -ErrorAction SilentlyContinue `
    | Sort-Object -Property FullName `
    | Select-Object -Last 1

if (!$signtool) {
    Write-Warning "SignTool.exe not found. You need to install a Windows SDK."
    exit 1
}

$certFile = Get-ChildItem "$PSScriptRoot\..\*.pfx" `
    | Sort-Object -Property Name `
    | Select-Object -First 1
if (!$certFile) {
    Write-Warning "No PFX file found in the project root."
    Write-Host "This script needs a certificate with private key as a PFX file in the project root to work."
    exit 1
}

function ConvertFrom-SecureToPlain {
    param([Parameter(Mandatory=$true)][System.Security.SecureString] $SecurePassword)
    # Create a "password pointer"
    $passwordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
    # Get the plain text version of the password
    $plainTextPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto($passwordPointer)
    # Free the pointer
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($passwordPointer)
    # Return the plain text password
    $plainTextPassword
}

$pfxPassword = Read-Host -AsSecureString "PFX Password"

$unsignedTargets = @()
Write-Output "Searching for files without signature..."
foreach ($f in $TargetFiles) {
    [string]$result = & $signtool Verify /pa /tw $f 2>&1
    if ($LASTEXITCODE) {
        if ($result.Contains("No signature found")) {
            $unsignedTargets += $f
            Write-Output "- $f"
        } else {
            Write-Warning "- $f failed to verify signature"
            & $signtool Verify /pa /tw $f
            Write-Warning "Exit Code: $LASTEXITCODE"
        }
    } else {
        Write-Output "- $f already signed"
    }
}
Write-Output "Signing files..."
foreach ($f in $unsignedTargets) {
    Write-Output "- $f"
    $backup = "$f.bak"
    if (!(Test-Path $backup)) { Copy-Item $f $backup }
    & $signtool sign `
        /f $certFile /p $(ConvertFrom-SecureToPlain $pfxPassword) `
        /fd sha256 /td sha256 /tr $TimeServer `
        $f
    if ($LASTEXITCODE) {
        Write-Warning "Signing failed."
        exit 1
    }
}

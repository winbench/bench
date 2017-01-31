$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$tmpArchive = "$myDir\bench_export.7z"
$7z = "$myDir\lib\bench\7z\7z.exe"
$sfxFile = "$myDir\res\bench.sfx"
$targetFile = "$myDir\test.exe"

function Copy-StreamContent($src, $trg)
{
    $buffer = [Array]::CreateInstance([byte], 32768)
    $read = 0
    while (($read = $src.Read($buffer, 0, $buffer.Length)) -gt 0)
    {
        $trg.Write($buffer, 0, $read)
    }
}

function Copy-FileContent($srcFile, $trg)
{
    $src = [IO.File]::OpenRead($srcFile)
    Copy-StreamContent $src $trg
    $src.Close()
}

function Write-Config($trg)
{
    $enc = New-Object System.Text.UTF8Encoding @($false)
    $w = New-Object System.IO.StreamWriter @($trg, $enc, 1024, $true)

    $w.WriteLine(";!@Install@!UTF-8!")
    $w.WriteLine("Title=`"Bench Transfer Package`"")
    $w.WriteLine("BeginPrompt=`"Are you sure you want to extract and setup this Bench environment?`"")
    $w.WriteLine("ExecuteFile=`"powershell.exe`"")
    $w.WriteLine("ExecuteParameters=`"-NoProfile -NoLogo -ExecutionPolicy Unrestricted -Command ( & \`".\\auto\\lib\\Initialize-ExportedBench.ps1\`" )`"")
    $w.Write(";!@InstallEnd@!")
    $w.Close()
}

if (Test-Path $tmpArchive) { del $tmpArchive }
if (Test-Path $targetFile) { del $targetFile }

& $7z a $tmpArchive .\auto
& $7z a $tmpArchive .\res
& $7z a $tmpArchive .\config

$target = [IO.File]::Create($targetFile)
Copy-FileContent $sfxFile $target
Write-Config $target
Copy-FileContent $tmpArchive $target
$target.Close()

del $tmpArchive
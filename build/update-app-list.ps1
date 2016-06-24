$rootDir = [IO.Path]::GetDirectoryName([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $false, $false)

$apps = $cfg.Apps

$targetFile = "$docsDir\src-content\ref\apps.md"

function GetFrontMatter($file)
{
	$sourceLines = [IO.File]::ReadAllLines($file, [Text.Encoding]::UTF8)
	$hits = 0
	$lastHit = -1
	for ($i = 0; $i -lt $sourceLines.Length; $i++)
	{
		if ($sourceLines[$i] -eq "+++")
		{
			$hits++
			$lastHit = $i
		}
		if ($hits -eq 2) { break; }
	}
	if ($hits -eq 2)
	{
		$sb = New-Object System.Text.StringBuilder
		for ($i = 0; $i -le $lastHit; $i++)
		{
			$_ = $sb.AppendLine($sourceLines[$i])
		}
		return $sb.ToString()
	}
	return ""
}

function WriteAppBlock($sb, $app)
{
	$_ = $sb.AppendLine("### $($app.Label)")
	$_ = $sb.AppendLine()
	$_ = $sb.AppendLine("* ID: ``$($app.ID)``")
	$_ = $sb.AppendLine("* Typ: ``$($app.Typ)``")
	if ($app.Website) { $_ = $sb.AppendLine("* Website: <$($app.Website)>") }
	if ($app.Version) { $_ = $sb.AppendLine("* Version: $($app.Version)") }
	$_ = $sb.AppendLine()
}

function WriteAppCategory($sb, $label, $name)
{
	$_ = $sb.AppendLine("## $label")
	$_ = $sb.AppendLine()
	$apps.ByCategory($name) | Sort-Object -Property Label | % { WriteAppBlock $sb $_ }
}

$sb = New-Object System.Text.StringBuilder
$_ = $sb.Append((GetFrontMatter $targetFile))
$_ = $sb.AppendLine()

WriteAppCategory $sb "Groups" "Groups"
WriteAppCategory $sb "Required Apps" "Required"
WriteAppCategory $sb "Optional Apps" "Optional"

[IO.File]::WriteAllText($targetFile, $sb.ToString(), [Text.Encoding]::UTF8)

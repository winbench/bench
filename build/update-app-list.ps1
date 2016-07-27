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
	  $_ = $sb.AppendLine("### $($app.Label) {#$($app.ID)}")
	  $_ = $sb.AppendLine()
	  $_ = $sb.AppendLine("* ID: ``$($app.ID)``")
	  $_ = $sb.AppendLine("* Typ: ``$($app.Typ)``")
	  if ($app.Website) { $_ = $sb.AppendLine("* Website: <$($app.Website)>") }
    $version = $app.Version
    if (!$version) { $version = "latest" }
    $_ = $sb.AppendLine("* Version: $version")
    if ($app.Dependencies.Length -gt 0)
    {
        [array]$deps = $app.Dependencies | % {
            $depApp = $apps[$_]
            return "[$($depApp.Label)](#$_)"
        }
        $depsList = [string]::Join(", ", $deps)
        $_ = $sb.AppendLine("* Dependencies: $depsList")
    }
	  $_ = $sb.AppendLine()
}

function WriteAppTable($sb, $label, $anchor)
{
    $_ = $sb.AppendLine("[**$label**](#$anchor)")
    $_ = $sb.AppendLine()
    $_ = $sb.AppendLine("<!--")
    $_ = $sb.AppendLine("#data-table /*/$label/*")
    $_ = $sb.AppendLine("#column ID: value(ID)")
    $_ = $sb.AppendLine("#column Name: name(.)")
    if ($label -ne "Groups")
    {
        $_ = $sb.AppendLine("#column Version: value(Version)")
        $_ = $sb.AppendLine("#column Website: value(Website)")
    }
    $_ = $sb.AppendLine("-->")
    $_ = $sb.AppendLine()
}

function WriteAppCategory($sb, $label, $anchor, $name)
{
	  $_ = $sb.AppendLine("## $label {#$anchor}")
	  $_ = $sb.AppendLine()
	  $apps.ByCategory($name) | Sort-Object -Property Label | % { WriteAppBlock $sb $_ }
}

$sb = New-Object System.Text.StringBuilder
$_ = $sb.Append((GetFrontMatter $targetFile))
$_ = $sb.AppendLine()
$_ = $sb.AppendLine("## Overview")
$_ = $sb.AppendLine()
WriteAppTable $sb "Groups" "groups"
WriteAppTable $sb "Required Apps" "apps-required"
WriteAppTable $sb "Optional Apps" "apps-optional"

WriteAppCategory $sb "Groups" "groups" "Groups"
WriteAppCategory $sb "Required Apps" "apps-required" "Required"
WriteAppCategory $sb "Optional Apps" "apps-optional" "Optional"

[IO.File]::WriteAllText($targetFile, $sb.ToString(), [Text.Encoding]::UTF8)

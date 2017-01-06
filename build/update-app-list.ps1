$rootDir = [IO.Path]::GetDirectoryName([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
. "$scriptsDir\bench.lib.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $false, $false)
$apps = $cfg.Apps

$targetFile = "$docsDir\content\ref\apps.md"
$targetDir = Empty-Dir "$docsDir\content\app"

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

function WriteAppFile($app, $no)
{
    $version = $app.Version
    if (!$version) { $version = "latest" }
    $ns = $app.Namespace
    if (!$ns) { $ns = "(default)" }
    $deps = $app.Dependencies
    $resp = $app.Responsibilities

    $f = [IO.Path]::Combine($targetDir, $app.ID + ".md")
    $w = New-Object System.IO.StreamWriter @($f, $false, [Text.Encoding]::UTF8)
    $w.WriteLine("+++")
    $w.WriteLine("title = `"$($app.Label)`"")
    $w.WriteLine("weight = $no")
    $w.WriteLine("app_lib = `"$($app.AppLibrary.ID)`"")
    $w.WriteLine("app_category = `"$($app.Category)`"")
    $w.WriteLine("app_ns = `"$ns`"")
    $w.WriteLine("app_id = `"$($app.ID)`"")
    $w.WriteLine("app_version = `"$version`"")
    $w.WriteLine("+++")
    $w.WriteLine()
    $w.WriteLine("**ID:** ``$($app.ID)``  ")
    $w.WriteLine("**Version:** $version  ")
    $w.WriteLine("<!--more-->")
    if ($app.MarkdownDocumentation)
    {
        $w.WriteLine()
        $w.WriteLine("## Description")
        $w.WriteLine($app.MarkdownDocumentation)
    }
    $w.WriteLine()
    $w.WriteLine("## Source")
    $w.WriteLine()
    $w.WriteLine("* Library: ``$($app.AppLibrary.ID)``")
    $w.WriteLine("* Category: $($app.Category)")
    $w.WriteLine("* Order Index: $no")
    $w.WriteLine()
    $w.WriteLine("## Properties")
    $w.WriteLine()
    $w.WriteLine("* Namespace: $ns")
    $w.WriteLine("* Name: $($app.Name)")
    $w.WriteLine("* Typ: ``$($app.Typ)``")
    if ($app.Website) { $w.WriteLine("* Website: <$($app.Website)>") }
    
    if ($deps.Length -gt 0)
    {
        [array]$deps2 = $deps | % {
            $depApp = $apps[$_]
            return "[$($depApp.Label)](/app/$_)"
        }
        $depsList = [string]::Join(", ", $deps2)
        $w.WriteLine("* Dependencies: $depsList")
    }
    if ($resp.Length -gt 0)
    {
        [array]$resp2 = $resp | % {
            $respApp = $apps[$_]
            return "[$($respApp.Label)](/app/$_)"
        }
        $respList = [string]::Join(", ", $resp2)
        $w.WriteLine("* Responsibilities: $respList")
    }
    $w.WriteLine()
    $w.Close()
}

function WriteAppTableHeader($sb)
{
    $_ = $sb.AppendLine("| Label | Version | Library | Category |")
    $_ = $sb.AppendLine("|-------|---------|---------|----------|")
}

function WriteAppTableRow($sb, $app)
{
    $id = $app.ID
    $label = $app.Label
    $version = $app.Version
    if (!$version) { $version = "latest" }
    $appLib = $app.AppLibrary.ID
    $category = $app.Category
    $_ = $sb.AppendLine("| [${label}](/app/${id}) | $version | ``$appLib`` | $category |")
}

$no = 0
foreach ($app in $apps)
{
    $no++
    Write-Host "$($no.ToString("0000")) $($app.ID)"
    WriteAppFile $app $no
}

$sortedApps = $apps | sort { $_.Label }

$sb = New-Object System.Text.StringBuilder
$_ = $sb.Append((GetFrontMatter $targetFile))
$_ = $sb.AppendLine()
WriteAppTableHeader $sb
foreach ($app in $sortedApps)
{
    WriteAppTableRow $sb $app
}
$_ = $sb.AppendLine()
[IO.File]::WriteAllText($targetFile, $sb.ToString(), [Text.Encoding]::UTF8)

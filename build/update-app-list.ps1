$rootDir = [IO.Path]::GetDirectoryName([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"
. "$scriptsDir\bench.lib.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $false)
$apps = $cfg.Apps

$targetFile = "$docsDir\content\ref\apps.md"
$targetDir = Empty-Dir "$docsDir\content\apps"

function NormalizeTag($value)
{
  return $value.ToLowerInvariant().Replace(" ", "-")
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
    # Custom page params
    $w.WriteLine("app_library = `"$($app.AppLibrary.ID)`"")
    $w.WriteLine("app_category = `"$($app.Category)`"")
    $w.WriteLine("app_typ = `"$($app.Typ)`"")
    $w.WriteLine("app_ns = `"$ns`"")
    $w.WriteLine("app_id = `"$($app.ID)`"")
    $w.WriteLine("app_version = `"$version`"")
    # Taxonomies
    $w.WriteLine("app_categories = [`"$($app.Category)`"]")
    $w.WriteLine("app_libraries = [`"$($app.AppLibrary.ID)`"]")
    $w.WriteLine("app_types = [`"$($app.Typ)`"]")
    $w.WriteLine("+++")
    $w.WriteLine()
    $w.WriteLine("**ID:** ``$($app.ID)``  ")
    $w.WriteLine("**Version:** $version  ")
    $w.WriteLine("<!--more-->")
    $w.WriteLine()
    $w.WriteLine("[Back to all apps](/apps/)")
    if ($app.MarkdownDocumentation)
    {
        $w.WriteLine()
        $w.WriteLine("## Description")
        $w.WriteLine($app.MarkdownDocumentation)
    }
    $w.WriteLine()
    $w.WriteLine("## Source")
    $w.WriteLine()
    $w.WriteLine("* Library: [``$($app.AppLibrary.ID)``](/app_libraries/$(NormalizeTag $app.AppLibrary.ID))")
    $w.WriteLine("* Category: [$($app.Category)](/app_categories/$(NormalizeTag $app.Category))")
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
            return "[$($depApp.Label)](/apps/$_)"
        }
        $depsList = [string]::Join(", ", $deps2)
        $w.WriteLine("* Dependencies: $depsList")
    }
    if ($resp.Length -gt 0)
    {
        [array]$resp2 = $resp | % {
            $respApp = $apps[$_]
            return "[$($respApp.Label)](/apps/$_)"
        }
        $respList = [string]::Join(", ", $resp2)
        $w.WriteLine("* Responsibilities: $respList")
    }
    $w.WriteLine()
    $w.Close()
}

$no = 0
foreach ($app in $apps)
{
    $no++
    if (!$app.AppLibrary) { continue }
    Write-Host "$($no.ToString("0000")) $($app.ID)"
    WriteAppFile $app $no
}

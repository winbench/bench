param (
  [switch]$Remove,
  $CompilerPackageVersion = "1.3.2"
)

$packageId = "Microsoft.Net.Compilers"
$packageVersion = $CompilerPackageVersion

$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = Resolve-Path "$myDir\..\BenchManager"

$projects = @(
    @{
        "name" = "BenchLib"
        "targetFramework" = "net45"
    }
    @{
        "name" = "BenchLib.Test"
        "targetFramework" = "net45"
    }
    @{
        "name" = "BenchCLI"
        "targetFramework" = "net45"
    }
    @{
        "name" = "BenchDashboard"
        "targetFramework" = "net45"
    }
)

function removeCompiler($name)
{
    $packagesConfigFile = "$rootDir\$name\packages.config"
    if (Test-Path $packagesConfigFile -PathType Leaf)
    {
        [xml]$packagesConfig = Get-Content $packagesConfigFile
        $killNodes = @()
        foreach ($p in $packagesConfig.SelectNodes("/packages/package"))
        {
            if ($p.id -eq $packageId)
            {
                $killNodes += $p
            }
        }
        foreach ($n in $killNodes)
        {
            echo "Removing package $($n.id) v$($n.version) from project $name"
            $_ = $n.ParentNode.RemoveChild($n)
        }
        $pp = $packagesConfig.SelectNodes("/packages/package")
        if ($pp.Count -gt 0)
        {
            $packagesConfig.Save($packagesConfigFile)
        }
        else
        {
            del $packagesConfigFile
        }
    }
}

function addCompiler($name, $framework)
{
    $packagesConfigFile = "$rootDir\$name\packages.config"
    if (!(Test-Path $packagesConfigFile -PathType Leaf))
    {
        [xml]$newDoc = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`n<packages></packages>"
        $newDoc.Save($packagesConfigFile)
    }
    echo "Adding package $packageId v$packageVersion to project $name"
    [xml]$packagesConfig = Get-Content $packagesConfigFile
    [Xml.XmlElement]$p = $packagesConfig.CreateElement("package")
    $p.SetAttribute("id", $packageId)
    $p.SetAttribute("version", $packageVersion)
    $p.SetAttribute("targetFramework", $framework)
    $p.SetAttribute("developmentDependency", "true")
    $pp = $packagesConfig.SelectSingleNode("/packages")
    $_ = $pp.AppendChild($p)
    $packagesConfig.Save($packagesConfigFile)
}

foreach ($project in $projects)
{
    removeCompiler $project.name
    if (!$Remove)
    {
        addCompiler $project.name $project.framework
    }
}

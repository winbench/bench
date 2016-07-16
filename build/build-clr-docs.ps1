param (
    $sourceFile = $(Resolve-Path "$([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))\..\BenchManager\BenchLib\bin\Debug\BenchLib.xml"),
    $typesFile = $(Resolve-Path "$([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))\xml-doc-types.txt"),
    $targetDir = $(Resolve-Path "$([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition))\..\tmp\clr-docs")
)

if (!$sourceFile -or !(Test-Path $sourceFile -PathType Leaf))
{
	Write-Error "Could not find the source file."
	return
}
if (!$typesFile -or !(Test-Path $typesFile -PathType Leaf))
{
	Write-Error "Could not find the list of types to consider."
	return
}
if (!$targetDir -or !(Test-Path $targetDir -PathType Container))
{
	Write-Error "Could not find the target directory."
	return
}

Set-Alias new New-Object
$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

$typeStyleFile = Resolve-Path "$myDir\xml-doc-to-md-type.xslt"
$memberStyleFile = Resolve-Path "$myDir\xml-doc-to-md-member.xslt"

$typeStyle = new System.Xml.Xsl.XslCompiledTransform
$typeStyle.Load([string]$typeStyleFile)
$memberStyle = new System.Xml.Xsl.XslCompiledTransform
$memberStyle.Load([string]$memberStyleFile)

$source = new System.Xml.XmlDocument
$source.Load([string]$sourceFile)

function member()
{
    $source.SelectNodes("/doc/members/member")
}

function type-member($typeName)
{
    member | ? { $_.name -like "T:${typeName}" }
}

function field-member($typeName)
{
    member | ? { $_.name -like "F:${typeName}.*" }
}

function event-member($typeName)
{
    member | ? { $_.name -like "E:${typeName}.*" }
}

function property-member($typeName)
{
    member | ? { $_.name -like "P:${typeName}.*" }
}

function method-member($typeName)
{
    member | ? { $_.name -like "M:${typeName}.*" }
}

function transform($style, [System.Xml.XmlElement]$m)
{
    $w = new System.IO.StringWriter
    $sr = new System.IO.StringReader ("<partial>" + [string]$m.OuterXml + "</partial>")
    $xr = [System.Xml.XmlReader]::Create($sr)
    $xmlArgs = new System.Xml.Xsl.XsltArgumentList
    try 
    {
        $style.Transform($xr, $xmlArgs, $w)
    }
    catch
    {
        Write-Warning $_.Exception.Message
    }
    $xr.Close()
    $sr.Close()
    return $w.ToString()
}

$types = Get-Content $typesFile

foreach ($t in $types)
{
    $tm = type-member $t
    
    $out = [IO.File]::OpenWrite("$targetDir\${t}.md")
    $writer = new System.IO.StreamWriter($out, (new System.Text.UTF8Encoding ($false)))

    $writer.WriteLine((transform $typeStyle $tm))
    [array]$methods = property-member $t
    if ($methods)
    {
        $writer.WriteLine("## Methods {#methods}")
        foreach ($m in $methods)
        {
            $writer.WriteLine("### " + $m.name)
            $writer.WriteLine((transform $memberStyle $m))
        }
    }

    $writer.Close()
    $out.Close()
}

<#
.SYNOPSIS
Convert XML documentation files from .NET assemblies into Markdown files.

.PARAMETER TargetPath
A path to the directory where the Markdown files will be stored.
If the specified directory does not exist, it is created.

.PARAMETER Assemblies
A list of absolute ar relative paths, pointing to the .NET assemblies
(*.dll, *.exe) which will be included in the documentation.
A path can contain wildcards, which will be expanded before processing
the assemblies.

.PARAMETER FileNameExtension
This file name extension is used for the generated Markdown files.

.PARAMETER UrlBase
The URL base is used as a prefix for all links in the generated Markdown files.

.PARAMETER UrlFileNameExtension
This file name extension is used in the URLs of cross file links,
(e.g. if one type references another type via a see or seealso XML doc tag).

.PARAMETER Title
The title for the whole API which is documented by the XML doc files.

.PARAMETER Author
The author of the documentation.

.PARAMETER Publisher
The publisher of the documentation.

.PARAMETER Copyright
A copyright notice to include in the optional footer and the meta-data header.

.PARAMETER NoTitleHeadline
Use this switch to suppress the title written as the main headline.

.PARAMETER HeadlineOffset
Provide -1 or 1 to adjust the headline levels.

.PARAMETER Footer
Use this switch to activate the generation of a footer with some metadata.

.PARAMETER MetaDataStyle
Provide one of three styles for the metadata header.
- None: no header is written
- Pandoc: An EPUB compatible YAML header is written
- Hugo: A YAML header for static website generation is written

.DESCRIPTION

xmldoc2md aims to be a light-weight converter from XML doc files (and their 
assemblies) to Markdown files.

If you use the /doc switch with a .NET compiler, e.g. by activating the XML
documentation generation in the project properties inside of Visual Studio,
the compiler will generate an XML file along with your assembly.
This XML doc file contains the content of your XML comments on various
code blocks (classes, properties, methods, ...).

But the XML doc file itself is not very helpful and needs further processing
to provide an easy-to-browse documentation.
Actually, to build meaningfulf documentation, the compiled assembly is needed
as well, because a lot of information about the types and type members is not
included in the XML doc file.

To generate a documentation for a number of assemblies, you can call
xmldoc2md, specifying the output directory for the Markdown files and the 
paths to the assemblies. xmldoc2md expects to find the XML doc files beside
their corresponding assembly.

xmldoc2md generates one Markdown file for every namespace and every public type
in the given assemblies.

.EXAMPLE

To generate the Markdown formatted documentation for one assembly use:

> xmldoc2md "path\to\docs\md" "path\to\my.assembly.dll"

... or with named parameters and explicit array of assembly paths:

> xmldoc2md -TargetPath "path\to\target-directory" `
            -Assemblies @("path\to\my.assembly.dll")

To pass multiple assemblies you can put them into an array:

> xmldoc2md "path\to\docs\md" @("bin\assembly1.dll", "bin\assembly2.exe")

... or pipe the assembly paths into xmldoc2md:

> gci *.dll | xmldoc2md "path\to\docs\md"

You can use wildcards in the assembly paths:

> xmldoc2md "path\to\docs\md" @("project\bin\*.dll", "project\bin\*.exe")

If you need the generated files to have another filename extension than *.md,
you can use the FileNameExtension parameter:

> xmldoc2md "path\to\docs\md" "bin\*.dll" -FileNameExtension "*.markdown"

To specify the base path for the URLs in generated links, use the UrlBase
parameter:

> xmldoc2md "path\to\docs\md" "bin\*.dll" -UrlBase "http://my-domain.com/docs/"

If you will transform the Markdown files into another format, changing the
filename extension of the doc files in that process, you can specify the final
filename extension to be used in the URLs of generated links:

> xmldoc2md "path\to\docs\md" "bin\*.dll" -UrlFileNameExtension ".htm"

.NOTES

You can use tools like the Sandacstle Help File Builder or Doxygen to generate
a static HTML website or a compiled HTML (*.chm) file, but these tools are kind
of heavy and do not support output in Markdown format.

https://github.com/EWSoftware/SHFB
http://www.stack.nl/~dimitri/doxygen/

The Markdown format is a light-weight text markup syntax and is widely used
and supported in a lot of modern software development and collaboration tools.

https://daringfireball.net/projects/markdown/

xmldoc2md works with a combination of code in different languages:

* The overall transformation process is controlled by this PowerShell script.
* A C# code file defining a couple of classes with parsing algorithms
  for cref-style member references, and formatting functions.
* A number of XSLT files for rendering the XML doc files as Markdown.

The PowerShell script compiles the C# file at runtime by calling the Add-Type
cmdlet. The XSLT files are used through the .NET class 
System.Xml.Xsl.XsltCompiledTransformation. The types of the runtime compiled
C# files are passed to the XsltCompiledTransformation via an XsltArgumentList
as extension objects. This way, the XSLT files have access to the C# defined
functions.
By using more than one call to the XSL transformations, to generate one output
file, the PowerShell script has fine control over the content of the generated
Markdown files.

.LINK
https://github.com/mastersign/xmldoc2md

.LINK
https://daringfireball.net/projects/markdown/

.LINK
http://gohugo.io

.LINK
http://pandoc.org
#>

[CmdletBinding()]
Param (
    [Parameter(Position=0, Mandatory=$True)]
	[ValidateNotNullOrEmpty()]
	[string]$TargetPath,

    [Parameter(Position=1, Mandatory=$True, ValueFromPipeline=$True)]
	[ValidateNotNullOrEmpty()]
	[string[]]$Assemblies,

	[string]$FileNameExtension = ".md",

	[string]$UrlBase = "",

	[string]$UrlFileNameExtension = ".md",

	[string]$Title = "Untitled API",

	[Parameter(Mandatory=$False)]
	[string]$Author,

	[Parameter(Mandatory=$False)]
	[string]$Publisher,

	[string]$Copyright = "Copyright &copy; $([DateTime]::Now.Year). All rights reserved.",

	[ValidateRange(-1,1)]
	[int]$HeadlineOffset = 0,

	[switch]$NoTitleHeadline,

	[switch]$Footer,

	[ValidateSet("None", "Pandoc", "Hugo")]
	[string]$MetaDataStyle = "None"
)

if (!(Test-Path $TargetPath)) { mkdir $TargetPath | Out-Null }
$TargetPath = Resolve-Path $TargetPath

$assemblyPaths = @()
foreach ($p in $Assemblies)
{
	$assemblyPaths += Resolve-Path $p
}
[array]$assemblyPaths = $assemblyPaths | sort

Write-Host "Target Path: $TargetPath"
Write-Host "Assemblies:"
foreach ($p in $assemblyPaths)
{
	$docFile = [IO.Path]::ChangeExtension($p, ".xml")
	if (Test-Path $docFile)
	{
		Write-Host "  - $p"
	}
	else
	{
		Write-Host "  - $p (NO XMLDOC FILE)"
	}
}

Set-Alias new New-Object
$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)

try
{
	Add-Type -Path "$myDir\xmldoc2md.cs" -ReferencedAssemblies "System.Xml.dll"
}
catch
{
	Write-Warning $_.Exception.Message
	return
}
$crefParsing = new Mastersign.XmlDoc.CRefParsing
$crefFormatting = new Mastersign.XmlDoc.CRefFormatting
$crefFormatting.FileNameExtension = $FileNameExtension
$crefFormatting.UrlBase = $UrlBase
$crefFormatting.UrlFileNameExtension = $UrlFileNameExtension

$typeStyleFile = Resolve-Path "$myDir\xmldoc2md_type.xslt"
$memberStyleFile = Resolve-Path "$myDir\xmldoc2md_member.xslt"

$typeStyle = new System.Xml.Xsl.XslCompiledTransform
$typeStyle.Load([string]$typeStyleFile)
$memberStyle = new System.Xml.Xsl.XslCompiledTransform
$memberStyle.Load([string]$memberStyleFile)

$assemblyRefs = @()
$xmlDocs = @()

foreach ($p in $assemblyPaths)
{
	try
    {
		$assembly = [Reflection.Assembly]::LoadFrom($p)
		$xmlDocFile = [IO.Path]::ChangeExtension($p, ".xml")
		$xmlDoc = new System.Xml.XmlDocument
		if (Test-Path $xmlDocFile)
		{
			$xmlDoc.Load([string]$xmlDocFile)
		}
		$assemblyRefs += $assembly
		$xmlDocs += $xmlDoc
	}
	catch
	{
		Write-Warning "Loading XMLDOC for $p failed."
		Write-Warning $_.Exception.Message
	}
}
$crefFormatting.Assemblies = $assemblyRefs;
$crefFormatting.XmlDocs = $xmlDocs;

$headlinePrefix = "#" * ($HeadlineOffset + 1)
$printTitleHeadline = $headlinePrefix -and !$NoTitleHeadline

function parse-cref($cref)
{
    return [Mastersign.XmlDoc.CRefParsing]::Parse($cref)
}

function public-types()
{
	foreach ($a in $assemblyRefs)
	{
		[System.Reflection.Assembly]$a = $a
        try
        {
	    	$a.GetTypes() | ? { $_.IsPublic -or $_.IsNestedPublic }
        }
        catch
        {
            $ex = $_.Exception
            Write-Warning $ex.Message
            foreach ($lex in $ex.LoaderExceptions)
            {
                Write-Warning "- $($lex.Message)"
            }
        }
	}
}

function all-member()
{
	foreach ($doc in $xmlDocs)
	{
		$doc.SelectNodes("/doc/members/member")
	}
}

function type-member($typeName) { all-member | ? { $_.name -eq "T:${typeName}" } }
function field-member($typeName) { all-member | ? { $_.name -like "F:${typeName}.*" } }
function event-member($typeName) { all-member | ? { $_.name -like "E:${typeName}.*" } }
function property-member($typeName) { all-member | ? { $_.name -like "P:${typeName}.*" } }
function ctor-member($typeName)
{
	all-member | ? {
		$_.name.StartsWith("M:${typeName}.#ctor") -or
		$_.name.Equals("M:${typeName}.#cctor")
	}
}
function method-member($typeName)
{
	all-member | ? {
		($_.name -match "M:${typeName}\.[^\.]+(\(.+\))?$") -and
		!($_.name.Contains("#ctor") -or $_.name.EndsWith("#cctor"))
	}
}

function transform($writer, $style, [System.Xml.XmlElement]$e)
{
	if (!$e) { return }
    $sr = new System.IO.StringReader ("<partial>" + [string]$e.OuterXml + "</partial>")
    $xr = [System.Xml.XmlReader]::Create($sr)
    $xmlArgs = new System.Xml.Xsl.XsltArgumentList
	$xmlArgs.AddParam("headlinePrefix", "", $headlinePrefix)
	$xmlArgs.AddExtensionObject("urn:CRefParsing", $crefParsing)
	$xmlArgs.AddExtensionObject("urn:CRefFormatting", $crefFormatting)
    try
    {
        $style.Transform($xr, $xmlArgs, $writer)
    }
    catch
    {
        Write-Warning $_.Exception
    }
    $xr.Close()
    $sr.Close()
}

function write-indexblock($writer, $nodes, $title)
{
	if (!$nodes) { return }
	$writer.WriteLine("**$title**")
	$writer.WriteLine()
	foreach ($n in $nodes)
	{
		$cref = $n.name
		$label = $crefFormatting.EscapeMarkdown($crefFormatting.Label($cref))
		$anchor = $crefFormatting.Anchor($cref)
		$writer.WriteLine("* [$label](#$anchor)")
	}
	$writer.WriteLine()
}

function write-memberblock($writer, $nodes, $title, $ref)
{
	if (!$nodes) { return }
	$writer.WriteLine("${headlinePrefix}## $title {#$ref}")
	$writer.WriteLine()
    foreach ($n in $nodes)
    {
        transform $writer $memberStyle $n
    }
}

function write-metadata()
{
	switch ($MetaDataStyle)
	{
		"Pandoc" { write-pandoc-epub-header @args }
		"Hugo" { write-hugo-front-matter @args }
		default { }
	}
}

function write-pandoc-epub-header($writer, $label, $memberKind)
{
	$writer.WriteLine("---")
	$writer.WriteLine("title:")
	$writer.WriteLine("- type: main")
	$writer.WriteLine("  text: `"$Title`"")
	$writer.WriteLine("- type: subtitle")
	$writer.WriteLine("  text: `"$label`"")
	$writer.WriteLine("subject: $memberKind")
	$writer.WriteLine("date: $([DateTime]::Now.ToString("yyyy-MM-dd"))")
	if ($Author) { $writer.WriteLine("creator: `"$Author`"") }
	if ($Publisher) { $writer.WriteLine("publisher: `"$Publisher`"") }
	if ($Copyright) { $writer.WriteLine("rights: `"$Copyright`"") }
	$writer.WriteLine("...")
}

function write-hugo-front-matter($writer, $label, $memberKind)
{
	$writer.WriteLine("---")
	$writer.WriteLine("title: `"$Title - $label`"")
	$writer.WriteLine("categeories:")
	$writer.WriteLine("  - `".NET API`"")
	$writer.WriteLine("  - `"$memberKind`"")
	$writer.WriteLine("date: $([DateTime]::Now.ToString("yyyy-MM-dd"))")
	if ($Author) { $writer.WriteLine("author: `"$Author`"") }
	if ($Publisher) { $writer.WriteLine("publisher: `"$Publisher`"") }
	if ($Copyright) { $writer.WriteLine("copyright: `"$Copyright`"") }
	$writer.WriteLine("---")
}

function type-variation([Type]$type)
{
	if ($type.IsInterface) { return "Interface"	}
	elseif ($type.IsEnum) { return "Enumeration" }
	elseif ([MulticastDelegate].IsAssignableFrom($type)) { return "Delegate" }
	elseif ($type.IsValueType) { return "Struct" }
	else { return "Class" }
}

[array]$types = public-types | sort { $_.FullName }
[string[]]$namespaces = $types | % { $_.Namespace } | select -Unique | sort

Write-Host "Files:"
foreach ($ns in $namespaces)
{
    $nsCRef = "N:$ns"
    $nsFile = $crefFormatting.FileName($nsCRef)
    $nsLabel = $crefFormatting.Label($nsCRef)
    Write-Host "  -> $nsFile"

    $out = [IO.File]::Open("$TargetPath\$nsFile", [IO.FileMode]::Create, [IO.FileAccess]::Write)
    $writer = new System.IO.StreamWriter($out, (new System.Text.UTF8Encoding ($false)))

	write-metadata $writer $nsLabel "Namespace"

    $writer.WriteLine()
	if ($printTitleHeadline) { $writer.WriteLine("${headlinePrefix} $($crefFormatting.EscapeMarkdown($Title))") }
    $writer.WriteLine("${headlinePrefix}# $($crefFormatting.EscapeMarkdown($nsLabel)) Namespace")
    $writer.WriteLine()

    foreach ($t in $types)
    {
        if ($t.Namespace -eq $ns)
        {
            $tCRef = $crefFormatting.CRef($t)
            $tLabel = $crefFormatting.Label($tCRef)
            $tUrl = $crefFormatting.Url($tCRef)
            $writer.WriteLine("* [$($crefFormatting.EscapeMarkdown($tLabel))]($tUrl)")
        }
    }

	if ($Footer)
	{
	    $writer.WriteLine()
		$writer.WriteLine("----")
		if ($Author) { $writer.WriteLine("Author: $Author  ") }
		if ($Publisher) { $writer.WriteLine("Publisher: $Publisher  ")}
		$writer.WriteLine("Generated: $([DateTime]::Now.ToString("yyyy-MM-dd"))  ")
		if ($Copyright) { $writer.WriteLine("License: $Copyright  ") }
	}

    $writer.Close()
    $out.Close()
}

foreach ($t in $types)
{
	$tFile = $crefFormatting.FileName($t)
	$tCRefName = $crefFormatting.CRefTypeName($t)
	$tCRef = $crefFormatting.CRef($t)
	$tLabel = $crefFormatting.Label($tCref)
	$tParseResult = parse-cref $tCRef
	$typeVariation = type-variation $t

	Write-Host "  -> $tFile"
    $typeNode = type-member $tCRefName

    $out = [IO.File]::Open("$TargetPath\$tFile", [IO.FileMode]::Create, [IO.FileAccess]::Write)
    $writer = new System.IO.StreamWriter($out, (new System.Text.UTF8Encoding ($false)))

	write-metadata $writer $tLabel $typeVariation

	#$writer.WriteLine("<!--")
	#$writer.WriteLine("Type: $($t.FullName)")
	#$writer.WriteLine("FileName: $tFile")
	#$writer.WriteLine("CRef: $tCRef")
	#$writer.WriteLine("-->")
	$writer.WriteLine()
	if ($printTitleHeadline) { $writer.WriteLine("${headlinePrefix} $($crefFormatting.EscapeMarkdown($Title))") }
	$writer.WriteLine("${headlinePrefix}# $($crefFormatting.EscapeMarkdown($tLabel)) $typeVariation")

	if ($typeNode)
	{
		transform $writer $typeStyle $($typeNode.SelectSingleNode("summary"))
	}

	[Reflection.AssemblyName]$aName = $t.Assembly.GetName()
	$writer.WriteLine("**Absolute Name:** ``$($crefFormatting.FullLabel($tCRef))``  ")
	$writer.WriteLine("**Namespace:** $($t.Namespace)  ")
	$writer.WriteLine("**Assembly:** $($aName.Name), Version $($aName.Version)")
	$writer.WriteLine()

    [array]$ctorNodes = ctor-member $tCRefName
    [array]$fieldNodes = field-member $tCRefName
    [array]$eventNodes = event-member $tCRefName
    [array]$propertyNodes = property-member $tCRefName
    [array]$methodNodes = method-member $tCRefName

	if ($typeNode)
	{
		transform $writer $typeStyle $typeNode
		$writer.WriteLine()
	}

	if ($ctorNodes -or $fieldNodes -or $eventNodes -or $propertyNodes -or $methodNodes)
	{
		$writer.WriteLine("${headlinePrefix}## Overview")
		write-indexblock $writer $ctorNodes "Constructors"
		write-indexblock $writer $fieldNodes "Fields"
		write-indexblock $writer $eventNodes "Events"
		write-indexblock $writer $propertyNodes "Properties"
		write-indexblock $writer $methodNodes "Methods"
	}

	write-memberblock $writer $ctorNodes "Constructors" "ctors"
	write-memberblock $writer $fieldNodes "Fields" "fields"
	write-memberblock $writer $eventNodes "Events" "events"
	write-memberblock $writer $propertyNodes "Properties" "properties"
	write-memberblock $writer $methodNodes "Methods" "methods"

	if ($Footer)
	{
		$writer.WriteLine()
		$writer.WriteLine("----")
		if ($Author) { $writer.WriteLine("Author: $Author  ") }
		if ($Publisher) { $writer.WriteLine("Publisher: $Publisher  ")}
		$writer.WriteLine("Generated: $([DateTime]::Now.ToString("yyyy-MM-dd"))  ")
		if ($Copyright) { $writer.WriteLine("License: $Copyright  ") }
	}

    $writer.Close()
    $out.Close()
}

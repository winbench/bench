$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)

$docsDir = "$rootDir\docs"
$targetFile = "$docsDir\src-content\ref\bench-cli.md"

@"
+++
date = "$([DateTime]::Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"))"
description = "The command-line interface: bench.exe"
title = "Bench CLI"
weight = 2
+++

"@ | Out-File $targetFile -Encoding utf8

& $rootDir\auto\bin\bench.exe --help-format Markdown help --no-title --target-file "$targetFile" --append

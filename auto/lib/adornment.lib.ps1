function Clean-ExecutionProxies() {
    $baseDir = Get-ConfigPathValue AppAdornmentBaseDir
    Debug "Cleaning adornment proxy base path: $baseDir"
    $_ = Empty-Dir $baseDir
}

function Get-ExecutableProxy([string]$name, [string]$path) {
    $proxyBaseDir = Get-ConfigValue AppAdornmentBaseDir
    $proxyDir = Safe-Dir ([IO.Path]::Combine($proxyBaseDir, $name.ToLowerInvariant()))
    $proxyName = [IO.Path]::GetFileNameWithoutExtension($path) + ".cmd"
    return [IO.Path]::Combine($proxyDir, $proxyName)
}

function Setup-ExecutionProxies([string]$name) {
    $adornedExePaths = App-AdornedExecutables $name
    if ($adornedExePaths) {
        $libDir = Get-ConfigPathValue LibDir
        $nl = [Environment]::NewLine
        foreach ($exePath in $adornedExePaths) {
            Debug "Creating adornment proxy for '$exePath' of $name"
            $proxyPath = Get-ExecutableProxy $name $exePath
            if ($exePath.StartsWith($libDir, [StringComparison]::InvariantCultureIgnoreCase)) {
                $exePath = $exePath.Substring($libDir.Length).Trim('\')
                $proxyTarget = "%AUTO_DIR%\..\" + [IO.Path]::Combine(
                    (Get-ConfigValue LibDir), $exePath)
            } else {
                $proxyTarget = $exePath
            }
            $proxyCode = "@ECHO OFF$nl"
            $proxyCode += "SET AUTO_DIR=%~dp0..\..$nl"
            $proxyCode += "CALL runps Run-PreAdornment $name `"$exePath`"$nl"
            $proxyCode += "IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%$nl"
            $proxyCode += "CALL `"$proxyTarget`" %*$nl"
            $proxyCode += "CALL runps Run-PostAdornment $name `"$exePath`"$nl"
            $proxyCode += "IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%$nl"
            [IO.File]::WriteAllText($proxyPath, $proxyCode, [Text.Encoding]::Default)
        }
    }
}

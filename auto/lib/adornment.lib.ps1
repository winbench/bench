function Setup-ExecutionProxies([string]$name) {
    $adornedExePaths = App-AdornedExecutables $name
    if ($adornedExePaths) {
        $proxyBaseDir = Get-ConfigPathValue AppAdornmentBaseDir
        $proxyDir = Safe-Dir ([IO.Path]::Combine($proxyBaseDir, $name.ToLowerInvariant()))
        $libDir = Get-ConfigPathValue LibDir
        $nl = [Environment]::NewLine
        foreach ($exePath in $adornedExePaths) {
            Debug "Creating adornment proxy for '$exePath' of $name"
            $proxyName = [IO.Path]::GetFileNameWithoutExtension($exePath) + ".cmd"
            $proxyPath = [IO.Path]::Combine($proxyDir, $proxyName)
            $appDir = App-Dir $name
            if ($appDir.StartsWith($libDir, [StringComparison]::InvariantCultureIgnoreCase)) {
                $appDir = $appDir.Substring($libDir.Length).Trim('\')
                $proxyTarget = "%AUTO_DIR%\..\" + [IO.Path]::Combine(
                    (Get-ConfigValue LibDir), $appDir, $exePath)
            } else {
                $proxyTarget = [IO.Path]::Combine($appDir, $exePath)
            }
            $proxyCode = "@ECHO OFF$nl"
            $proxyCode += "SET AUTO_DIR=%~dp0..\..$nl"
            $proxyCode += "CALL runps Run-PreAdornment $name `"$exePath`"$nl"
            $proxyCode += "CALL `"$proxyTarget`" %*$nl"
            $proxyCode += "CALL runps Run-PostAdornment $name `"$exePath`"$nl"
            [IO.File]::WriteAllText($proxyPath, $proxyCode, [Text.Encoding]::Default)
        }
    }
}

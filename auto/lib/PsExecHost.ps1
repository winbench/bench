param ($Token = "Bench.Default")

$scriptsDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = Resolve-Path "$scriptsDir\..\.."
. "$scriptsDir\bench.lib.ps1"
. "$scriptsLib\reg.lib.ps1"
$Script:BenchEnv = New-Object Mastersign.Bench.BenchEnvironment ($global:BenchConfig)

function _WaitForClient([IO.Pipes.NamedPipeServerStream]$pipe)
{
	Write-Host ">>>>"
	while ($true)
	{
		try
		{
			$pipe.WaitForConnection()
			break
		}
		catch
		{
			Write-Warning $_.Exception.Message
			$pipe.Disconnect()
		}
	}
}

function _ParseShellArguments([string]$argStr)
{
	$sb = New-Object System.Text.StringBuilder
	$escaped = $false
	foreach ($c in $argStr.ToCharArray())
	{
		if ($escaped)
		{
			if ($c -eq "`"")
			{
				$escaped = $false
			}
			else
			{
				$sb.Append($c) | Out-Null
			}
		}
		else
		{
			if ($c -eq "`"")
			{
				$escaped = $true
			}
			elseif ($c -eq " ")
			{
				if ($sb.Length -gt 0)
				{
					$sb.ToString() # -> Output
					$sb.Clear() | Out-Null
				}
			}
			else
			{
				$sb.Append($c) | Out-Null
			}
		}
	}
	if ($sb.Length -gt 0)
	{
		$sb.ToString() # -> Output
	}
}

function _ParsePsArguments([string]$argStr)
{
	return $ExecutionContext.InvokeCommand.InvokeScript($argStr)
}

function _HandleExecutionRequest([IO.TextReader]$reader, [IO.TextWriter]$writer)
{
	try
	{
		$cwd = $reader.ReadLine()
		$cmd = $reader.ReadLine()
		$cmdArgs = $reader.ReadLine()
	}
	catch
	{
		Write-Warning "Bench: Could not read execution arguments from named pipe."
		return
	}
	
	$Script:benchEnv.Load()
	pushd $cwd
	$exitCode = 0
	$transcriptPath = [IO.Path]::Combine((Get-ConfigValue TempDir), $token)
	Start-Transcript -Path $transcriptPath | Out-Null
	try
	{
		if ([IO.Path]::GetExtension($cmd) -ieq ".ps1")
		{
			[array]$cmdArgList = _ParsePsArguments $cmdArgs
			. $cmd @cmdArgList
			if (!$?) { $exitCode = -1 }
		}
		else
		{
			[array]$cmdArgList = _ParseShellArguments $cmdArgs
			& $cmd @cmdArgList
			$exitCode = $LastExitCode
		}
	}
	catch
	{
		if ($_.Exception.Message)
		{
			Write-Warning $_.Exception.Message
			$writer.WriteLine($_.Exception.Message)
		}
		if ($exitCode -eq 0) { $exitCode = -1 }
	}
	Stop-Transcript | Out-Null
	popd
	$writer.WriteLine("EXITCODE $Token $exitCode")
	$writer.WriteLine("TRANSCRIPT $Token $transcriptPath")
}

$server = New-Object System.IO.Pipes.NamedPipeServerStream($Token, [IO.Pipes.PipeDirection]::InOut)

$closed = $false
while (!$closed)
{
	$reader = New-Object System.IO.StreamReader ($server, [Text.Encoding]::UTF8, $true, 4, $true)
	_WaitForClient $server
	$writer = New-Object System.IO.StreamWriter ($server, [Text.Encoding]::UTF8, 4, $true)
	$cmd = $reader.ReadLine()
	switch ($cmd)
	{
		"exec" { _HandleExecutionRequest $reader $writer | Out-Default }
		"close" { $closed = $true }
	}
	$writer.Flush()
	$server.WaitForPipeDrain()
	$writer.Dispose()
	$reader.Dispose()
	$server.Disconnect()
}
$server.Dispose()
Write-Host "<<<<"

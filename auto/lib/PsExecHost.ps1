param ($Token = "Bench.Default")

$scriptsDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = Resolve-Path "$scriptsDir\..\.."
. "$scriptsDir\bench.lib.ps1"

function _WaitForClient([IO.Pipes.NamedPipeServerStream]$pipe)
{
	Write-Host "Bench: Waiting for Request..."
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

function _ParseArguments([string]$argStr)
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

function _TeeOutput([IO.TextWriter]$w)
{
	process
	{
		$w.WriteLine($_)
		$_
	}
}

function _HandleExecutionRequest([IO.TextReader]$reader, [IO.TextWriter]$writer)
{
	try
	{
		$cwd = $reader.ReadLine()
		$cmd = $reader.ReadLine()
		$args = $reader.ReadLine()
	}
	catch
	{
		Write-Warning "Bench: Could not read execution arguments from named pipe."
		return
	}
	[array]$argList = _ParseArguments $args
	$exitCode = 0
	$env = New-Object Mastersign.Bench.BenchEnvironment ($Script:cfg)
	$env.Load()
	pushd $cwd
	try
	{
		if ([IO.Path]::GetExtension($cmd) -ieq ".ps1")
		{
			# Write-Host "Running script: $([IO.Path]::GetFileName($cmd)) ..."
			. $cmd @argList | _TeeOutput $writer | Out-Default
			if (!$?) { $exitCode = -1 }
			Write-Host "----"
		}
		else
		{
			# Write-Host "Running executable: $([IO.Path]::GetFileName($cmd)) ..."
			& $cmd @argList | _TeeOutput $writer | Out-Default
			$exitCode = $LastExitCode
			Write-Host "----"
		}
	}
	catch
	{
		Write-Warning $_.Exception.Message
		if ($exitCode -eq 0) { $exitCode = -1 }
	}
	popd
	$writer.WriteLine("EXITCODE $Token $exitCode")
}

$server = New-Object System.IO.Pipes.NamedPipeServerStream($Token, [IO.Pipes.PipeDirection]::InOut)
# Write-Host "Bench: Started Execution Host ..."

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
Write-Host "Bench: Stopped Execution Host."

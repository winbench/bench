param ($token = "Bench.Default")

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
		$cmd = $reader.ReadLine()
		$args = $reader.ReadLine()
	}
	catch
	{
		Write-Warning "Bench: Could not read execution arguments from named pipe."
		return
	}
	[array]$argList = _ParseArguments $args
	try
	{
		if ([IO.Path]::GetExtension($cmd) -ieq ".ps1")
		{
			Write-Host "Bench: Running script: $cmd ..."
			. $cmd @argList | _TeeOutput $writer | Out-Default
		}
		else
		{
			Write-Host "Bench: Running executable: $cmd ..."
			& $cmd @argList | _TeeOutput $writer | Out-Default
		}
	}
	catch
	{
		Write-Warning $_.Exception.Message
	}
}

$server = New-Object System.IO.Pipes.NamedPipeServerStream($token, [IO.Pipes.PipeDirection]::InOut)
Write-Host "Bench: Started Execution Host ..."

$closed = $false
while (!$closed)
{
	$reader = New-Object System.IO.StreamReader ($server, [Text.Encoding]::UTF8, $true, 4, $true)
	_WaitForClient $server
	Write-Host "Bench: Creating Writer"
	$writer = New-Object System.IO.StreamWriter ($server, [Text.Encoding]::UTF8, 4, $true)
	Write-Host "Bench: Reading Command ..."
	$cmd = $reader.ReadLine()
	Write-Host "Bench: Command: $cmd"
	switch ($cmd)
	{
		"exec" { _HandleExecutionRequest $reader $writer | Out-Default }
		"close" { $closed = $true }
	}
	Write-Host "Bench: Finished Request."
	$writer.Flush()
	$server.WaitForPipeDrain()
	$writer.Dispose()
	$reader.Dispose()
	Write-Host "Bench: Disconnecting"
	$server.Disconnect()
}
$server.Dispose()
Write-Host "Bench: Stopped Execution Host."

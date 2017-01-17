param ($Token = "Bench.Default", $WaitMessage = ">>>>")

$scriptsDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = Resolve-Path "$scriptsDir\..\.."
. "$scriptsDir\bench.lib.ps1"
. "$scriptsDir\reg.lib.ps1"

$Script:BenchEnv = New-Object Mastersign.Bench.BenchEnvironment ($global:BenchConfig)

function _PrintWaitingMessage()
{
	if ($WaitMessage) 
	{
		Write-Host $WaitMessage
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

$Script:executionResult = $null
function _ExecutionHandler([string]$cwd, [string]$cmd, [string]$cmdArgs)
{
	$Script:BenchEnv.Load()
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
		}
		if ($exitCode -eq 0) { $exitCode = -1 }
	}
	Stop-Transcript | Out-Null
	popd
	$Script:executionResult = New-Object Mastersign.Bench.RemoteExecHost.ExecutionResult @($exitCode, $transcriptPath)
}

function _ReloadHandler()
{
	Write-Host "Reloading Bench configuration..."
	$rootDir = $global:BenchConfig.BenchRootDir
	$global:BenchConfig = New-Object Mastersign.Bench.BenchConfiguration ($rootDir)
	$Script:BenchEnv = New-Object Mastersign.Bench.BenchEnvironment ($global:BenchConfig)
}

$server = New-Object Mastersign.Bench.RemoteExecHost.RemoteExecHostServer @($token)
Write-Host "PowerShell execution host started."

while($server) 
{
	_PrintWaitingMessage
	$rcmd = [Mastersign.Bench.RemoteExecHost.RemoteExecutionFacade]::WaitForCommand()
	switch ($rcmd.Type)
	{
		"Ping"
		{
			Write-Host "Remoting interface available."
			$rcmd.NotifyResult("OK")
		}
	    "Execution"
		{
			$cwd = $rcmd.Parameter.WorkingDirectory
			$exe = $rcmd.Parameter.Executable
			$exeArgs = $rcmd.Parameter.Arguments
			_ExecutionHandler $cwd $exe $exeArgs
			$rcmd.NotifyResult($Script:executionResult)
		}
		"Reload" 
		{ 
			_ReloadHandler 
		}
		"Shutdown"
		{
			$server.Dispose()
			$server = $null
			Write-Host "PowerShell execution host shut down."
			exit
		}
	}
}

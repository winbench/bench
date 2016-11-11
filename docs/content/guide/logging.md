+++
date = "2016-11-11T12:10:00+02:00"
description = "The transcript of the setup process"
title = "Logging"
weight = 8
+++

Every setup process in Bench is logged into a text file in
[`log`](/ref/file-structure/#log-dir).
The log file contains messages about every setup step,
occuring errors, including stack traces from the Bench core binary,
and the output of called command line tools.
During the development of custom setup scripts or incase a setup process
failes for some reason, analyzing the last log file can be very informative.
<!--more-->

The output of the command line tools is captured with the transcript
feature of the PowerShell (`Start-Transcript`, `Stop-Transcript`),
incase the command line tool is run by `PsExecHost.ps1`.
If a command line tool is run directly without the help of `PsExecHost.ps1`,
its output is simply piped in a temporary file, which is copied into the log.

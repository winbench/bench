param ( $appId )

#
# Is supposed to be copied to <Bench Root>\auto\bin to work properly
#

if (!$appId.Contains(".")) { $appId = "Bench." + $appId }

$ErrorActionPreference = "Stop"

$thisDir = Split-Path $MyInvocation.MyCommand.Path -Parent

bench manage load-app-libs
if (!$?) { exit }
bench --verbose app download $appId
if (!$?) { exit }
bench --verbose app uninstall $appId
if (!$?) { exit }
bench --verbose app install $appId
if (!$?) { exit }

explorer "-e,`"$(bench app property $appId Dir)`""

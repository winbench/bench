$scribusDir = App-Dir Scribus

$nsisDirs = "$scribusDir\`$PLUGINSDIR", "$scribusDir\`$TEMP"
foreach ($d in $nsisDirs)
{
    if (Test-Path $d)
    {
        Purge-Dir $d
    }
}

$uninst = "$scribusDir\uninst.exe"
if (Test-Path $uninst)
{
    del $uninst -Force
}

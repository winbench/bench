SFX Info
========

The file `7zSD.sfx` was taken out of the _LZMA SDK_ from 
<http://7-zip.org/download.html>.

To prevent UAC from elevating the SFX executable to administrator privileges, the `sfx.manifest` was added with `mt.exe` from the Windows SDK.
The result file is called `7zSD.sfx.exe`.

    copy /Y .\7zSD.sfx .\7zSD.sfx.exe
    mt.exe -manifest .\sfx.manifest -outputresource:".\7zSD.sfx.exe;#1"

Additionally the icon resource is replaced with
[Resource Hacker](http://angusj.com/resourcehacker/)
to achieve a nicer fit to the Bench ecosystem.

    ResourceHacker.exe -script .\reshack.txt

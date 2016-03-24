$settingsFile = "$(App-Dir Maven)\conf\settings.xml"

if (Test-Path $settingsFile) {
    Debug "Configuring Maven settings ..."
    $dom = [xml](Get-Content $settingsFile -Encoding UTF8)
    $nameTable = New-Object System.Xml.NameTable
    $nsMgr = New-Object System.Xml.XmlNamespaceManager($nameTable)
    $doc = $dom.DocumentElement
    $ns = $doc.NamespaceURI
    $nsMgr.AddNamespace("m", $ns)

    
    $repoE = $doc.SelectSingleNode("m:localRepository", $nsMgr)
    if (!$repoE) {
        $repoE = $dom.CreateElement("localRepository", $ns);
        $_ = $doc.AppendChild($repoE)
    }
    $repoE.InnerText = "`${env.HOME}\m2_repo"

    # TODO set proxies

    $dom.Save($settingsFile)
}

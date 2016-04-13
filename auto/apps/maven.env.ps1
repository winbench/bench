$settingsFile = "$(App-Dir Maven)\conf\settings.xml"

function AppendValueElement([Xml.XmlElement]$e, [string]$name, [string]$value) {
    $dom = $e.OwnerDocument
    $ns = $dom.DocumentElement.NamespaceURI
    $vE = $dom.CreateElement($name, $ns)
    $vE.InnerText = $value
    $_ = $e.AppendChild($vE)
}

function AddProxy([Xml.XmlElement]$proxiesE, [string]$protocol, [Uri]$uri) {
    $dom = $proxiesE.OwnerDocument
    $ns = $dom.DocumentElement.NamespaceURI
    $proxyE = $dom.CreateElement("proxy", $ns)
    AppendValueElement $proxyE "id" "bench_$protocol"
    AppendValueElement $proxyE "active" "true"
    AppendValueElement $proxyE "protocol" $protocol
    AppendValueElement $proxyE "host" $uri.Host
    AppendValueElement $proxyE "port" $uri.Port
    $_ = $proxiesE.AppendChild($proxyE)
}

if (Test-Path $settingsFile) {
    Debug "Configuring Maven settings ..."
    $dom = [xml](Get-Content $settingsFile -Encoding UTF8)
    $nameTable = New-Object System.Xml.NameTable
    $nsMgr = New-Object System.Xml.XmlNamespaceManager($nameTable)
    $doc = $dom.DocumentElement
    $ns = $doc.NamespaceURI
    $nsMgr.AddNamespace("m", $ns)

    # Update location of local repository
    
    $repoE = $doc.SelectSingleNode("m:localRepository", $nsMgr)
    if (!$repoE) {
        $repoE = $dom.CreateElement("localRepository", $ns)
        $_ = $doc.AppendChild($repoE)
    }
    $repoE.InnerText = "`${env.HOME}\m2_repo"

    # Update proxy configuration

    $proxiesE = $doc.SelectSingleNode("m:proxies", $nsMgr)
    if (!$proxiesE) {
        $proxiesE = $dom.CreateElement("proxies", $ns)
        $_ = $doc.AppendChild($proxiesE)
    }
    $proxiesE.RemoveAll()

    if (Get-ConfigValue UseProxy) {
        [Uri]$httpProxyUri = Get-ConfigValue HttpProxy
        if ($httpProxyUri) {
            AddProxy $proxiesE "http" $httpProxyUri
        }
        [Uri]$httpsProxyUri = Get-ConfigValue HttpsProxy
        if ($httpsProxyUri) {
            AddProxy $proxiesE "https" $httpsProxyUri
        }
    }

    $dom.Save($settingsFile)
}

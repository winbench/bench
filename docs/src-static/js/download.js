function GetLatestReleaseInfo() {
  $.getJSON("https://api.github.com/repos/winbench/bench/releases/latest")
    .done(function(release) {
        var setupAsset = null;
        var archiveAsset = null;
        for (var i = 0; i < release.assets.length; i++) {
            if (release.assets[i].name == "BenchSetup.exe") {
                setupAsset = release.assets[i];
            }
            if (release.assets[i].name == "Bench.zip") {
                archiveAsset = release.assets[i];
            }
        }
        var name = release.name.replace(/^v/, '');
        $(".version-info").text(name);
        if (setupAsset) {
            $(".setup-download-button").attr("href", setupAsset.browser_download_url);
            $(".setup-download-link").attr("href", setupAsset.browser_download_url);
        }
        if (archiveAsset) {
            $(".archive-download-button").attr("href", archiveAsset.browser_download_url);
            $(".archive-download-link").attr("href", archiveAsset.browser_download_url);
        }
      });
}

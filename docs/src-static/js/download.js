function GetLatestReleaseInfo() {
  $.getJSON("https://api.github.com/repos/mastersign/bench/releases/latest")
    .done(function(release) {
        var asset = null;
        for (var i = 0; i < release.assets.length; i++) {
            if (release.assets[i].name == "BenchSetup.exe") {
                asset = release.assets[i];
                break;
            }
        }
        if (!asset) return;
        // var downloadCount = 0;
        // for (var i = 0; i < release.assets.length; i++) {
        //   downloadCount += release.assets[i].download_count;
        // }
        // var oneHour = 60 * 60 * 1000;
        // var oneDay = 24 * oneHour;
        // var dateDiff = new Date() - new Date(asset.updated_at);
        // var timeAgo;
        // if (dateDiff < oneDay) {
        //   timeAgo = (dateDiff / oneHour).toFixed(1) + " hours ago";
        // } else {
        //   timeAgo = (dateDiff / oneDay).toFixed(1) + " days ago";
        // }
        // var releaseInfo = release.name + " was updated " + timeAgo + " and downloaded " + downloadCount.toLocaleString() + " times.";
        // console.log(releaseInfo);
        var name = release.name.replace(/^v/, '');
        $(".setup-download-button").attr("href", asset.browser_download_url);
        $(".setup-download-link").attr("href", asset.browser_download_url);
        $(".version-info").text(name);
      });
}

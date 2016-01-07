$lein = App-Exe Leiningen

Debug "LEIN_JAR: $env:LEIN_JAR"

& $lein self-install

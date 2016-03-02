$gitDir = App-Dir Git
pushd $gitDir
Debug "Running post-install script for Git ..."
.\git-bash.exe --no-needs-console --hide --no-cd --command=git-post-install.bat
popd
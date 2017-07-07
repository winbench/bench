$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$ghpDir = "$rootDir.gh-pages"
$branchName = "gh-pages"
$remoteName = "origin"

$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"

& "$myDir\build-docs.ps1"

pushd

& "$scriptsDir\Load-ClrLibs.ps1"
$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$benchEnv.Load()

$branches = git branch --list | ? { $_ -match $branchName }
$branchMissing = !$branches

if ($branchMissing)
{
    git fetch $remoteName "${branchName}:${branchName}"
    git branch "--set-upstream-to=${remoteName}/${branchName}" $branchName
}
$branches = git branch --list | ? { $_ -match $branchName }
$branchMissing = !$branches

if ((Test-Path $ghpDir -PathType Container) -and (Test-Path "$ghpDir\.git" -PathType Leaf))
{
    Write-Host "GitHub pages found at $ghpDir"
    cd $ghpDir
    git reset --hard
    git pull
}
else
{
    Write-Host "Creating GitHub pages at $ghpDir"
    if (!(Test-Path $ghpDir -PathType Container))
    {
        $_ = mkdir $ghpDir
    }
    if ($branchMissing)
    {
        cd $rootDir
        git worktree add -f "$ghpDir" master
        cd $ghpDir
        git checkout --orphan $branchName
        git rm -rf .
    }
    else
    {
        cd $rootDir
        git worktree add "$ghpDir" $branchName
        cd $ghpDir
        git pull
    }
}

Remove-Item "$ghpDir\*" -Exclude ".git", ".gitignore" -Recurse -Force

cp "$docsDir\public\*" "$ghpDir\" -Recurse

cd $ghpDir
git add -A :/
git commit -m "Automatic Update"

popd

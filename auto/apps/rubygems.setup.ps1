$ruby = App-Exe Ruby
$gemsTmpDir = App-Dir RubyGems

$packageDir = gci "$gemsTmpDir\rubygems-*" | Sort-Object -Descending
cd $packageDir
ruby setup.rb

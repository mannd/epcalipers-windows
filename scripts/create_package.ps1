# Make sure powershell is running as admin
If (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
    [Security.Principal.WindowsBuiltInRole] "Administrator"))
{
    Write-Warning "You must run this script as adminstrator."
    Break
}
[string]$destinationDirectory = "C:\Users\mannd\Documents\epcalipers-app\epcalipers"
[string]$scriptsDirectory = "C:\Users\mannd\git\epcalipers-windows\scripts"
[string]$assetsFiles = "C:\Users\mannd\git\epcalipers-windows\appx-manifest-assets\*"
[string]$packageFilesPath = Join-Path -Path $destinationDirectory  -ChildPath "\PackageFiles\"
[string]$appxPath = Join-Path -Path $destinationDirectory -ChildPath "\epcalipers.appx"
# Remove old destination file if it exists
Remove-Item $destinationDirectory -Recurse -ErrorAction Ignore -Verbose
cd $scriptsDirectory
# Convert app
.\convert_app.ps1
# Copy assets files to PackageFiles directory
Copy-Item $assetsFiles -Destination $packageFilesPath -Recurse -Force -Verbose
# Delete original epcalipers.appx
Remove-Item $appxPath -Verbose
# remake and sign appx
.\make_appx.ps1

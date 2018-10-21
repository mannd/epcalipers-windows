param (
    [string]$platform = "x86",
    [string]$version = "1.10.0.0"
)
[string]$installer = "C:\Users\mannd\git\epcalipers-windows\epcalipers\epcalipers-install\bin\$platform\Release\epcalipers_install.msi"
If ($platform -eq "x64")
{
    $installer = "C:\Users\mannd\git\epcalipers-windows\epcalipers\epcalipers-x64-install\bin\$platform\Release\epcalipers_install.msi"
}

DesktopAppConverter.exe -Installer $installer -Destination C:\Users\mannd\Documents\epcalipers-app\ -PackageName "epcalipers" -Publisher "CN=EP Studios" -Version $version -MakeAppx -Verbose

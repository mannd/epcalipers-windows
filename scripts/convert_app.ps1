param (
    [string]$platform = "x86",
    [string]$version = "1.10.0.0"
)

DesktopAppConverter.exe -Installer C:\Users\mannd\git\epcalipers-windows\epcalipers\epcalipers-install\bin\$platform\Release\epcalipers_install.msi  -Destination C:\Users\mannd\Documents\epcalipers-app\ -PackageName "epcalipers" -Publisher "CN=EP Studios" -Version $version -MakeAppx -Verbose

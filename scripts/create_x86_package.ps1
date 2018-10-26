param (
    [Parameter(Mandatory=$true)][string]$version = ""
)
if ([string]::isNullOrEmpty($version))
{
    Write-Warning "Must supply -version parameter on command line."
    Break
}
.\create_package -platform x86 -version $version

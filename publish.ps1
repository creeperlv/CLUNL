param([System.Version]$Version=$(throw "Need Parameter 'Version': -Verison Version"),[String]$Channel="InDev")
Write-Host ""
Write-Host "Building solution in configuration of: Release"
Write-Host ""
dotnet build -c Release
dotnet build CLUNL.Pipeline -c Release
.\package.ps1 -Version $Version -Channel $Channel
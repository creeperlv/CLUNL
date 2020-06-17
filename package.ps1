param([System.Version]$Version=$(throw "Need Parameter 'Version': -Verison Version"),[String]$Channel)
Write-Host ""
Write-Host "Package Build Artifacts"
Write-Host ""
Write-Host "==================================="
Write-Host "Target Version:		Target Channel:"
Write-Host "$Version" -NoNewLine -BackgroundColor DarkGreen
Write-Host "			" -NoNewLine
Write-Host "$Channel" -BackgroundColor Blue
Write-Host "==================================="
if(Test-Path ".\Artifacts"){
	
}else{
	mkdir ".\Artifacts"
}
$ArtifactName="CLUNL-AnyCPU-$Version-$Channel"
Write-Host "Generate Manifest file to: .\Artifacts\LibManifest"
$ManifestContent="CLUNL.dll"
$ManifestContent+="`nCLUNL.DirectedIO.dll"
$ManifestContent|Out-File ".\Artifacts\LibManifest" 
Write-Host "Compressing..."
$Compress=@{
	Path=".\Creeper Lv's Universal dotNet Library\bin\Release\netstandard2.0\CLUNL.dll",
		".\CLUNL.DirectedIO\bin\Release\netstandard2.0\CLUNL.DirectedIO.dll",
		".\CLUNL.Pipeline\bin\Release\netstandard2.0\CLUNL.Pipeline.dll",
		".\Artifacts\LibManifest"
	Destination=".\Artifacts\$ArtifactName.zip"
}
Compress-Archive @Compress -Force
Write-Host "Completed."
Write-Host "Generated Artifact to: .\Artifacts\$ArtifactName.zip"
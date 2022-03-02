param([System.Version]$Version=$(throw "Need Parameter 'Version': -Verison Version"),[String]$Channel)
Write-Output ""
Write-Output "Package Build Artifacts"
Write-Output ""
Write-Output "==================================="
Write-Output "Target Version:		Target Channel:"
Write-Output "$Version" -NoNewLine -BackgroundColor DarkGreen
Write-Output "			" -NoNewLine
Write-Output "$Channel" -BackgroundColor Blue
Write-Output "==================================="
if(Test-Path ".\Artifacts"){
	
}else{
	mkdir ".\Artifacts"
}
$ArtifactName="CLUNL-AnyCPU-$Version-$Channel"
Write-Output "Generate Manifest file to: .\Artifacts\LibManifest"
$ManifestContent="CLUNL.dll"
$ManifestContent+="`nCLUNL.DirectedIO.dll"
$ManifestContent+="`nCLUNL.Pipeline.dll"
$ManifestContent+="`nCLUNL.Diagnosis.dll"
$ManifestContent+="`nCLUNL.Packaging.dll"
$ManifestContent|Out-File ".\Artifacts\LibManifest" 
Write-Output "Compressing..."
$Compress=@{
	Path=".\Creeper Lv's Universal dotNet Library\bin\Release\netstandard2.0\CLUNL.dll",
		".\CLUNL.DirectedIO\bin\Release\netstandard2.0\CLUNL.DirectedIO.dll",
		".\CLUNL.Pipeline\bin\Release\netstandard2.0\CLUNL.Pipeline.dll",
		".\CLUNL.Diagnosis\bin\Release\netstandard2.0\CLUNL.Diagnosis.dll",
		".\CLUNL.Packaging\bin\Release\netstandard2.0\CLUNL.Packaging.dll",
		".\Artifacts\LibManifest"
	Destination=".\Artifacts\$ArtifactName.zip"
}
Compress-Archive @Compress -Force
Write-Output "Completed."
Write-Output "Generated Artifact to: .\Artifacts\$ArtifactName.zip"
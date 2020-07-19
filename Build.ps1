#!bin/pwsh

##############################
#Build with channel parameter#
##############################
Write-Output "Copyright (C) 2020 Creeper Lv"
Write-Output "Licensed under the MIT License"
Write-Output ""
Write-Output "Cloning..."
Write-Output ""
git clone https://github.com/creeperlv/Creeper-Lv-s-Universal-dotNet-Library.git CLUNL
#git clone https://gitea.com/CreeperLv/CLUNL.git CLUNL
#¡ü use this address in China to boost clone process.
cd CLUNL
$Ori = "./Creeper Lv's Universal dotNet Library/BuildInfo.cs"
$Dest =  "./Creeper Lv's Universal dotNet Library/BuildInfo.cs"
$ChannelName=""
if($args.Count -eq 0){
	$ChannelName=(git log -n 1)[0]
	$ChannelName="DailyBuild-"+($ChannelName -Split " ")[1]
}else{
	$ChannelName="CustomBuild-"+$args[0];
}
Write-Output "Build Channel is set to `"$ChannelName`""
Write-Output "Build Channel is defined in `"BuildInfo.cs`""
Write-Output ""
Write-Output "Starting .Net Core SDK"
Write-Output ""
(Get-Content $Ori) | Foreach-Object {$_ -replace 'Undefined', $ChannelName} | Set-Content $Dest
dotnet build -c Release
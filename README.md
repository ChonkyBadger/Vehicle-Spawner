# Badger Vehicle Spawner
Badger Vehicle Spawner is a simple Vehicle Spawner Menu using MenuAPI (https://github.com/TomGrobbe/MenuAPI)

Make sure you download from **RELEASES** on the right side and NOT the source code.  

## Configuration
Configuration is done within the config.json file, found in the folder named "Config".

`menuKey:` This determintes what key is used to open the menu. Numbers for each key can be found here: https://docs.fivem.net/docs/game-references/controls/  
`rightAlligned:` When "true", the menu will be alligned to the right hand side of the screen. This is enabled by default. To disable, set value to "false".  
`enabledInEmergencyVehicles:` This determines whether or not the menu can be opened while in a vehicle apart of the Emergency category, such as a police car. Value must be "true" or "false".  

`menus:` This is where you set up each category. In the default config, there are two cateogires so you can see how to set up a category and have multiple.  
&nbsp;&nbsp;&nbsp;&nbsp;`name:` This decides the display name for each category.  
&nbsp;&nbsp;&nbsp;&nbsp;`aceRequired:` This is the ace perm required to see the category. You can change this value.  
&nbsp;&nbsp;&nbsp;&nbsp;`vehicles:` This is an array which holds all the vehicles.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`name:` This is the display name for the vehicle.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;`spawncode:` This is the spawncode/model name for the vehicle.  

#### Ace Perms Example
In this example, let's say that the first category should be only for admins, or the ace group "group.admin".  
To do this, I would first decide what ace perm I wanted to restrict the category to. Lets say I do "VehicleSpawner.Admin".  
The aceRequired value in the menu in the config should therefore be "VehicleSpawner.Admin". To give admins this ace perm, I will then, in my server.cfg, do `add_ace group.admin VehicleSpawner.Admin allow`  
By doing this, I have restricted the first category to just admins.  

## License
Full license is viewable in the LICENSE file in this repository.  
This license is subject to be changed at any given time.

## Links
- [My Discord Server](https://discord.gg/TFCQE8d)

<a href="https://discord.com/invite/TFCQE8d"><img src="https://github.com/ChonkyBadger/ChonkyBadger/blob/main/Badger%20Icon.jpg" allign="left" width="250" >

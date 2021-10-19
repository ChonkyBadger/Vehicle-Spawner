# Badger Vehicle Spawner
Badger Vehicle Spawner is a simple Vehicle Spawner Menu using MenuAPI (https://github.com/TomGrobbe/MenuAPI)

## Features
**Ragdoll:** Players can toggle ragdoll by pressing "U" by default, editable in config.  
And uh, well the stuff on the hud, and the commands.  

## Commands
`/togglehud` Toggles the hud.

`/die` Kills the player who executes this command.

`/revive [Target Player ID]` Revives the player who uses this command or another player if specified by Id.

`/respawn` Respawns the player who executes this command.

`/pt` Toggles peacetime on and off.

`/pc <duration>` Turns on priority cooldown for a set time in minutes.  
`/pc-active` Sets priority status to Active.
`/pc-onhold` Sets priority status to On hold.  
`/pc-reset` Resets priority status to None.

`/setAOP <aop>` Sets the aop to whatever arguments are given. Requires permission node BadgerEssentials.Command.SetAOP

`/postal <postal>` Sets a waypoint to the specified postal.

`/announce <Announcement Message>` Displays a message to all players on the server. Requires permission node BadgerEssentials.Command.Announce  

## Configuration
To be done, in mean time, just figure it out. It is fairly self explanatory. Just make sure to follow json syntax!

## Permissions
`BadgerEssentials.Commands` Gives access to all commands.

`BadgerEssentials.Command.Announce` Gives access to the /Announce command.  

`BadgerEssentials.Command.PriorityCooldown` Gives access to all priority cooldown commands.  
`BadgerEssentials.Command.PC` Gives access to /pc  
`BadgerEssentials.Command.PCActive` Gives access to /pc-active   
`BadgerEssentials.Command.PCOnHold` Gives access to /pc-onhold  
`BadgerEssentials.Command.PCReset` Gives access to /pc-reset  


`BadgerEssentials.Command.Peacetime` Gives access to /pt  
`BadgerEssentials.Command.SetAOP` Gives access to /aop.  

`BadgerEssentials.Bypass.ReviveTimer` Bypasses the timer before you can revive.    

## License
Full license is viewable in the LICENSE file in this repository.  
This license is subject to be changed at any given time.

## Links
- [My Discord Server](https://discord.gg/TFCQE8d)
- [Postal Map used by this script](https://github.com/ocrp/postal_map/)

<a href="https://discord.com/invite/TFCQE8d"><img src="https://github.com/ChonkyBadger/ChonkyBadger/blob/main/Badger%20Icon.jpg" allign="left" width="250" >

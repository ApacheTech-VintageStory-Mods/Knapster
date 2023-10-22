# Knapster

Easier knapping, clayforming, and smithing, for those with low manual dexterity.

## Suport the Mod Author

If you find this mod useful, and you would like to show appreciation for the work I produce; please consider supporting me, and my work, using one of the methods below. Every single expression of support is most appreciated, and makes it easier to produce updates, and new features for my mods, moving fowards. Thank you.

 - [Join my Patreon!](https://www.patreon.com/ApacheTechSolutions?fan_landing=true)
 - [Donate via PayPal](http://bitly.com/APGDonate)
 - [Buy Me a Coffee](https://www.buymeacoffee.com/Apache)
 - [Subscribe on Twitch.TV](https://twitch.tv/ApacheGamingUK)
 - [Subscribe on YouTube](https://youtube.com/c/ApacheGamingUK)
 - [Purchase from my Amazon Wishlist](http://amzn.eu/7qvKTFu)
 - [Visit my website!](https://apachegaming.net)

## Features:
  
There are four different modes the server admin can set for each feature:
 - **Disabled:** The feature is disabled for all players on the server.
 - **Enabled:** The feature is enabled for all players on the server.
 - **Whitelist:** The feature is only enabled for players added to the whitelist.
 - **Blacklist:** The feature is enabled for all players on the server, except for those added to the blacklist.

 ### **Easy Knapping**
 
Provides an easier way for players to knap items within the game.

 - Click and hold anywhere on the knapping grid to place/remove voxels as needed, to complete the recipe.

 ### **Easy Clayforming** 
 
Provides an easier way for players to form clay items within the game.
    
 - Adds a new mode to clay, when clayforming.
 - Click and hold anywhere on the clayforming grid to place/remove voxels as needed, to complete the recipe.
 - Server Admins can change the number of voxels handled per click (1-8) (Default: 1).

 ### **Easy Smithing**
 
Provides an easier way for players to smith items on an anvil within the game.
    
 - Adds a new mode to the hammer, when smithing.
 - Click and hold anywhere on the smithing grid to place/remove voxels as needed, to complete the recipe.
 - Server Admins can change the hammer durability loss per click (1-10) (Default: 5).

## Server-Side Commands:

| Command                                   | Description |
| ---                                       | --- |
| **/knapster knapping mode [disabled\|enabled\|whitelist\|blacklist]**             | Change the mode for Easy Knapping. |
| **/knapster knapping whitelist**            | Show players currently on the knapping whitelist. |
| **/knapster knapping whitelist [playerName]**            | Add/Remove a player from the knapping whitelist. |
| **/knapster knapping blacklist**            | Show players currently on the knapping blacklist. |
| **/knapster knapping blacklist [playerName]**            | Add/Remove a player from the knapping blacklist. |
| ---                                       | --- |
| **/knapster clayforming mode [disabled\|enabled\|whitelist\|blacklist]**             | Change the mode for Easy Clay Forming. |
| **/knapster clayforming whitelist**            | Show players currently on the clayforming whitelist. |
| **/knapster clayforming whitelist [playerName]**            | Add/Remove a player from the clayforming whitelist. |
| **/knapster clayforming blacklist**            | Show players currently on the clayforming blacklist. |
| **/knapster clayforming blacklist [playerName]**            | Add/Remove a player from the clayforming blacklist. |
| **/knapster clayforming voxels [1-8]**    | Change the number of voxels handled per click. |
| ---                                       | --- |
| **/knapster smithing mode [disabled\|enabled\|whitelist\|blacklist]**             | Change the mode for Easy Smithing. |
| **/knapster smithing whitelist**            | Show players currently on the smithing whitelist. |
| **/knapster smithing whitelist [playerName]**            | Add/Remove a player from the smithing whitelist. |
| **/knapster smithing blacklist**            | Show players currently on the smithing blacklist. |
| **/knapster smithing blacklist [playerName]**            | Add/Remove a player from the smithing blacklist. |
| **/knapster smithing cost [1-10]**        | Change the hammer durability loss per click. |
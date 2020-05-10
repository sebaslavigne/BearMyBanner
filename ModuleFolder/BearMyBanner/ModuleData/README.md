For better reading, see this file at
https://github.com/SebLavK/BearMyBanner/tree/master/ModuleFolder/BearMyBanner/ModuleData

There are two configuration files for Bear my Banner:
* `BMBSettings.xml` with all of the settings described below.
* `BMBFormationBanners.xml` with the codes for the custom formation banners. Use the [Online Banner Editor](https://bannerlord.party/banner/) to create your designs and copy the code into the file.

# Main Settings

## Battle Types

### Sieges

**AllowSieges** - Banners will appear during sieges.  
`true` or `false`

**SiegeAttackersUseBanners** - Attackers in a siege use banners.  
`true` or `false`

**SiegeDefendersUseBanners** - Defenders in a siege use banners.  
`true` or `false`

### Hideout Attacks

**AllowHideouts** - Banners will appear during hideout attacks.  
`true` or `false`

**HideoutAttackersUseBanners** - Attackers in a hideout mission use banners.  
`true` or `false`

**HideoutBanditsUseBanners** - Bandits in a hideout mission use banners. **NOTE:** you also have to allow bandits in the filters below.  
`true` or `false`

### Tournaments

**TournamentBanners** - Allow banners during tournaments. Gives only one banner per team and only to cavalry.  
`true` or `false`

**TournamentThemes** - Use banner themes. Each culture has different patterns for team banners, selects a random theme for every team.  
`true` or `false`

**TournamentBannersInShields** - Each team's banner theme is also applied to the shields. If disabled, shields always use plain colors.  
`true` or `false`

### Towns and villages

**TownCastleVisitBanner** - When visiting a town or a castle, the companion that follows you carries a banner.  
`true` or `false`

**VillageVisitBanner** - When visiting a village, the companion that follows you carries a banner.  
`true` or `false`

## Banner Ratios and Filters

### Ratios

**BearerToTroopRatio** - Give a banner to every 1 in X units in a group. Smaller values mean more banners.  
From `1` to `50`

**UnitCountMode** - How the units will be grouped to give them banners.  
* `Spec` - Group units by their general type (infantry, ranged, cavalry and horse archers).
* `Formation` - Group units by formation.
* `Troop` - Group by their specific kind (Imperial Veteran Infantryman, Battanian Skirmisher, Vlandian Knight, etc.).

### Filter by Occupation

**AllowSoldiers** - Allow soldiers to bear banners. Most units are of this type.  
`true` or `false`

**AllowMercenaries** - Allow mercenaries to bear banners (i.e.Watchmen, Hired Blades, etc.).  
`true` or `false`

**AllowCaravanGuards** - Allow caravan guards to bear banners. "Caravan masters only" gives a banner only to the master of a caravan, caravan guards in the service of a lord also get banners.  
* `NotAllowed` - Caravan guards don't get banners.
* `OnlyMasters` - Only caravan masters get a banner.
* `Allowed` - Caravan guards can bear banners.

**AllowBandits** - Allow bandits to bear banners (i.e.Looters, Bandits, Raiders, etc.). "Recruited bandits only" gives banners to bandits that are in the service of a lord.  
* `NotAllowed` - Bandits don't get banners.
* `RecruitedOnly` - Bandits can only bear banners when their in the service of a lord.
* `Allowed` - Bandits can bear banners.

### Filter by General Type

**AllowInfantry** - Allow infantry type units to bear banners.  
`true` or `false`

**AllowRanged** - Allow archers and crossbowmen to bear banners. **NOTE:** bearers will lose their bow or crossbow, but still use their melee weapons. If ordered to "Charge" they will rush the enemy, order them to "Advance" and they will stay in formation.  
`true` or `false`

**AllowMounted** - Allow cavalry type units to bear banners.  
`true` or `false`

**AllowMountedRanged** - Allow horse archer type units to bear banners. **NOTE:** same as with the archers.  
`true` or `false`

### Filter by Tier

**FilterTiers** - Use tier filters. If disabled, all tiers get banners.  
`true` or `false`

**AllowedTiers** - The tiers that will get banners, separated by commas. Some troop tree mods include tiers higher than 6, just add them to the list to enable them(e.g. `8,9,10,11`).  
* By default `3,4,5,6`.

### Give banners to Formations

**AllowFormationInfantry** - Give banners among units assigned to formation I Infantry  
`true` or `false`

**AllowFormationRanged** - Give banners among units assigned to formation II Ranged  
`true` or `false`

**AllowFormationCavalry** - Give banners among units assigned to formation III Cavalry  
`true` or `false`

**AllowFormationHorseArcher** - Give banners among units assigned to formation IV Horse Archer  
`true` or `false`

**AllowFormationSkirmisher** - Give banners among units assigned to formation V Skirmisher  
`true` or `false`

**AllowFormationHeavyInfantry** - Give banners among units assigned to formation VI Heavy Infantry  
`true` or `false`

**AllowFormationLightCavalry** - Give banners among units assigned to formation VII Light Cavalry  
`true` or `false`

**AllowFormationHeavyCavalry** - Give banners among units assigned to formation VIII Heavy Cavalry  
`true` or `false`

### Give to Heroes

**AllowPlayer** - Give a banner to the player. **NOTE:** you won't be able to equip your shield with the scroll wheel. Use the numpad to change to a shield or hold G to drop the banner.  
`true` or `false`

**AllowCompanions** - Give a banner to every companion.  
`true` or `false`

**AllowNobles** - Give a banner to every noble. Might confuse nobles with ranged weapons.  
`true` or `false`

## Banner Dropping

**DropOnLowHealth** - Units will drop their banner when their health is below the threshold. If they have a shield they will equip it.  
`true` or `false`

**DropHealthThreshold** - Units will drop their banner when their health is below the threshold.  
From `0` to `100`

**DropRetreatModeSetting** - Units have a chance of dropping their banner when retreating. If "Weighted by experience", more experienced units have less chance of dropping the banner.  
* `Disabled` - Units never drop their banners when retreating.
* `Weighted` - The chance of dropping the banner is tied to the unit's experience.
* `Fixed` - Units always have the same chance of dropping a banner when retreating.

**DropRetreatChance** - Base chance of units dropping their banner when retreating.  
* From `0.0` to `1.0`. By default `0.6`.

# Formation Banners Settings

**EnableFormationBanners** - Enable the use of custom banner designs for troop formations. Only applies to player's party. Codes are defined in the ModuleData folder of this mod.  
`true` or `false`

**CompanionsUseFormationBanners** -
Companions use the banner of the formation they're assigned to. If disabled they use the clan banner.  
`true` or `false`

**FormationBannersUseInShields** -
The design also applies to the shields of all the troops in each formation (all of them, not only the banner bearers).  
`true` or `false`

**Banner codes** - Change the banner codes in file `BMBFormationBanners.xml` to have customized banners for each formation.

# Miscellaneous

**ShowMessages** - Show messages in the lower left corner. For example, how many banners are given to each party in a battle.  
`true` or `false`

 **DebugMessages** - Shows details of banner assignments in each battle. Useful for understanding how different settings behave.  
 `true` or `false`

**WhiteMessages** - Messages are always displayed in white. Useful if some party colors are hard to read.  
`true` or `false`

**ReloadFiles** - Reloads config files before every battle, without needing to restart the game. Useful to quickly tweak the settings, disable after you're happy with the results.  
`true` or `false`

**KonamiCode** - Leftover code from an old game.  
`true` or `false`

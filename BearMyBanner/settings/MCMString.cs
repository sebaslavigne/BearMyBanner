using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Settings
{
    public class MCMHint
    {
        public const string AllowSieges = "Banners will appear during sieges";
        public const string SiegeAttackersUseBanners = "Attackers in a siege use banners.";
        public const string SiegeDefendersUseBanners = "Defenders in a siege use banners.";

        public const string AllowHideouts = "Banners will appear during hideout attacks.";
        public const string HideoutAttackersUseBanners = "Attackers in a hideout mission use banners.";
        public const string HideoutBanditsUseBanners = "Bandits in a hideout mission use banners.NOTE: you also have to allow bandits in the filters below.";

        public const string TournamentBanners = "Allow banners during tournaments. Gives only one banner per team and only to cavalry.";
        public const string TournamentThemes = "Use banner themes. Each culture has different patterns for team banners, selects a random theme for every team.";
        public const string TournamentBannersInShields = "Each team's banner theme is also applied to the shields. If disabled, shields always use plain colors.";

        public const string TownCastleVisitBanner = "When visiting a town or a castle, the companion that follows you carries a banner.";
        public const string VillageVisitBanner = "When visiting a village, the companion that follows you carries a banner.";

        public const string BearerToTroopRatio = "Give a banner to every 1 in X units of the same type. Smaller values mean more banners.";
        public const string UnitCountMode = "How the units will be counted to give them banners using the filters and the banner to troop ratio.";

        public const string AllowSoldiers = "Allow soldiers to bear banners. Most units are of this type.";
        public const string AllowCaravanGuards = "Allow caravan guards to bear banners.";
        public const string AllowMercenaries = "Allow mercenaries to bear banners (i.e.Watchmen, Hired Blades, etc.).";
        public const string AllowBandits = "Allow bandits to bear banners (i.e.Looters, Bandits, Raiders, etc.).";

        public const string AllowInfantry = "Allow infantry units to bear banners.";
        public const string AllowRanged = "Allow archers and crossbowmen to bear banners. NOTE: bearers will lose their bow or crossbow, but still use their melee weapons. If ordered to \"Charge\" they will rush the enemy, order them to \"Advance\" and they will stay in formation.";
        public const string AllowMounted = "Allow cavalry units to bear banners.";
        public const string AllowMountedRanged = "Allow horse archers to bear banners. NOTE: same as with the archers.";

        public const string FilterTiers = "Use tier filters. If disabled, all tiers get banners.";
        public const string AllowedTiers = "The tiers that will get banners, separated by commas. Some troop tree mods include tiers higher than 6, just add them to the list to enable them(e.g. 8,9,10,11).";

        public const string AllowPlayer = "Give a banner to the player. NOTE: you won't be able to equip your shield with the scroll wheel. Use the numpad to change to a shield or hold G to drop the banner.";
        public const string AllowCompanions = "Give a banner to every companion.";
        public const string AllowNobles = "Give a banner to every noble. Might confuse nobles with ranged weapons.";

        public const string ShowMessages = "Show messages in the lower left corner. For example, how many banners are given to each party in a battle.";
        public const string WhiteMessages = "Messages are always displayed in white. Useful if some party colors are hard to read.";
    }

    public class MCMDisplayName
    {
        public const string AllowSieges = "Use banners in sieges";
        public const string SiegeAttackersUseBanners = "Siege attackers use banners";
        public const string SiegeDefendersUseBanners = "Siege defenders use banners";
        public const string AllowHideouts = "Use banners in hideout attacks";
        public const string HideoutAttackersUseBanners = "Hideout attackers use banners";
        public const string HideoutBanditsUseBanners = "Hideout bandits use banners";
        public const string TournamentBanners = "Use banners in tournaments";
        public const string TournamentThemes = "Random tournament themes";
        public const string TournamentBannersInShields = "Apply theme to shields";
        public const string TownCastleVisitBanner = "Use banners in towns and castles";
        public const string VillageVisitBanner = "Use banners in villages";
        public const string BearerToTroopRatio = "Bearer to troop ratio";
        public const string UnitCountMode = "Bearer units count mode";
        public const string AllowSoldiers = "Allow soldiers";
        public const string AllowCaravanGuards = "Allow caravan guards";
        public const string AllowMercenaries = "Allow mercenaries";
        public const string AllowBandits = "Allow bandits";
        public const string AllowInfantry = "Allow infantry";
        public const string AllowRanged = "Allow ranged";
        public const string AllowMounted = "Allow cavalry";
        public const string AllowMountedRanged = "Allow horse archers";
        public const string FilterTiers = "Filter by tier";
        public const string AllowedTiers = "Allowed tiers";
        public const string AllowPlayer = "Give banner to player";
        public const string AllowCompanions = "Give banners to companions";
        public const string AllowNobles = "Give banners to nobles";
        public const string ShowMessages = "Show messages";
        public const string WhiteMessages = "White messages";
    }
}

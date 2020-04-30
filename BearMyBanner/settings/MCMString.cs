using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Settings
{
    public class MCMHint
    {
        public const string AllowSieges = "Allow banners during sieges";
        public const string SiegeAttackersUseBanners = "Attackers in a siege use banners";
        public const string SiegeDefendersUseBanners = "Defenders in a siege use banners";
        public const string AllowHideouts = "Allow banners during hideout attacks";
        public const string HideoutAttackersUseBanners = "Attackers in a hideout mission use banners";
        public const string HideoutBanditsUseBanners = "Bandits in a hideout mission use banners.NOTE: you also have to allow bandits in the filters below";
        public const string TournamentBanners = "Allow banners during tournaments.Gives only one banner per team and only to cavalry";
        public const string TournamentThemes = "Use banner themes.Each culture has different patterns for team banners, selects a random theme for every team.";
        public const string TournamentBannersInShields = "Each team's banner theme is also applied to the shields. If disabled, shields always use plain colors";
        public const string TownCastleVisitBanner = "When visiting a town or a caslte, the companion that follows you carries a banner";
        public const string VillageVisitBanner = "When visiting a village, the companion that follows you carries a banner";
        public const string BearerToTroopRatio = "Give a banner to every 1 in X units of the same type.Smaller values mean more banners";
        public const string UnitCountMode = "How the units will be counted to give them banners using the filters and the banner to troop ratio";
        public const string AllowSoldiers = "Allow soldiers.These are most troops";
        public const string AllowCaravanGuards = "Allow caravan guards";
        public const string AllowMercenaries = "Allow mercenaries(i.e.Watchmen, Hired Blades, etc.)";
        public const string AllowBandits = "Allow bandits(i.e.Looters, Bandits, Raiders, etc.)";
        public const string AllowInfantry = "Allow infantry units";
        public const string AllowRanged = "Allow archers and crossbowmen.NOTE: the troops that get banners will lose their bow or crossbow, but still use their melee weapons. Also note that if ordered to \"Charge\" they will rush the enemy, order them to \"Advance\" and they will stay with the rest of the archers";
        public const string AllowMounted = "Allow cavalry units";
        public const string AllowMountedRanged = "Allow horse archers.NOTE: same as with the archers";
        public const string FilterTiers = "Use tier filters.If disabled, all tiers get banners";
        public const string AllowedTiers = "The tiers that will get banners, separated by commas. Some troop tree mods include tiers higher than 6, just add them to the list to enable them(e.g. 8,9,10,11)";
        public const string AllowPlayer =     "Give banners to your character, your companions and other nobles. IMPORTANT NOTE: same rules as with the archers apply(for now), bows and crossbows are removed at the start of the battle";
        public const string AllowCompanions = "Give a banner to every companion";
        public const string AllowNobles = "Give a banner to every noble";
        public const string ShowMessages = "Show messages in the lower left corner.For example, how many banners are given to each party in a battle";
        public const string WhiteMessages = "Messages are always displayed in white.Useful if some party colors are hard to read";
        //public const string ReloadFiles
    }

    public class MCMDisplayName
    {
        public const string AllowSieges = "AllowSieges";
        public const string SiegeAttackersUseBanners = "SiegeAttackersUseBanners";
        public const string SiegeDefendersUseBanners = "SiegeDefendersUseBanners";
        public const string AllowHideouts = "AllowHideouts";
        public const string HideoutAttackersUseBanners = "HideoutAttackersUseBanners";
        public const string HideoutBanditsUseBanners = "HideoutBanditsUseBanners";
        public const string TournamentBanners = "TournamentBanners";
        public const string TournamentThemes = "TournamentThemes";
        public const string TournamentBannersInShields = "TournamentBannersInShields";
        public const string TownCastleVisitBanner = "TownCastleVisitBanner";
        public const string VillageVisitBanner = "VillageVisitBanner";
        public const string BearerToTroopRatio = "BearerToTroopRatio";
        public const string UnitCountMode = "UnitCountMode";
        public const string AllowSoldiers = "AllowSoldiers";
        public const string AllowCaravanGuards = "AllowCaravanGuards";
        public const string AllowMercenaries = "AllowMercenaries";
        public const string AllowBandits = "AllowBandits";
        public const string AllowInfantry = "AllowInfantry";
        public const string AllowRanged = "AllowRanged";
        public const string AllowMounted = "AllowMounted";
        public const string AllowMountedRanged = "AllowMountedRanged";
        public const string FilterTiers = "FilterTiers";
        public const string AllowedTiers = "AllowedTiers";
        public const string AllowPlayer = "AllowPlayer";
        public const string AllowCompanions = "AllowCompanions";
        public const string AllowNobles = "AllowNobles";
        public const string ShowMessages = "ShowMessages";
        public const string WhiteMessages = "WhiteMessages";
    }
}

namespace BearMyBanner.Settings
{
    public static class IBMBSettingsExtension
    {
        public static IBMBSettings SetDefaultSettings(this IBMBSettings settings)
        {
            settings.AllowSoldiers = true;
            settings.AllowCaravanGuards = CaravanAssignMode.OnlyMasters;
            settings.AllowMercenaries = false;
            settings.AllowBandits = BanditAssignMode.RecruitedOnly;

            settings.AllowInfantry = true;
            settings.AllowMounted = true;
            settings.AllowRanged = false;
            settings.AllowMountedRanged = false;

            settings.FilterTiers = true;
            settings.AllowedTiers = "4,5,6";

            settings.AllowPlayer = false;
            settings.AllowCompanions = false;
            settings.AllowNobles = false;

            settings.BearerToTroopRatio = 7;
            settings.UnitCountMode = UnitCountMode.Type;

            settings.AllowSieges = true;
            settings.SiegeAttackersUseBanners = true;
            settings.SiegeDefendersUseBanners = false;

            settings.AllowHideouts = false;
            settings.HideoutAttackersUseBanners = false;
            settings.HideoutBanditsUseBanners = false;

            settings.TournamentBanners = true;
            settings.TournamentThemes = true;
            settings.TournamentBannersInShields = false;

            settings.TownCastleVisitBanner = false;
            settings.VillageVisitBanner = false;

            settings.DropOnLowHealth = true;
            settings.DropHealthThreshold = 20;
            settings.DropRetreatMode = DropRetreatMode.Weighted;
            settings.DropRetreatChance = 0.6f;

            settings.ShowMessages = true;
            settings.WhiteMessages = false;
            settings.ReloadFiles = false;

            return settings;
        }
    }
}
namespace BearMyBanner.Settings
{
    public static class IBMBSettingsExtension
    {
        public static IBMBSettings SetDefaultSettings(this IBMBSettings settings)
        {
            settings.AllowSoldiers = true;
            settings.AllowCaravanGuards = CaravanAssignMode.OnlyMasters;
            settings.AllowMercenaries = true;
            settings.AllowBandits = BanditAssignMode.RecruitedOnly;

            settings.AllowTypeInfantry = true;
            settings.AllowTypeRanged = true;
            settings.AllowTypeMounted = true;
            settings.AllowTypeMountedRanged = true;

            settings.FilterTiers = false;
            settings.AllowedTiers = "4,5,6";

            settings.AllowPlayer = false;
            settings.AllowCompanions = false;
            settings.AllowNobles = false;

            settings.BearerToTroopRatio = 1;
            settings.UnitCountMode = UnitCountMode.Spec;

            settings.AllowSieges = true;
            settings.SiegeAttackersUseBanners = true;
            settings.SiegeDefendersUseBanners = false;

            settings.AllowHideouts = false;
            settings.HideoutAttackersUseBanners = false;
            settings.HideoutBanditsUseBanners = false;

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
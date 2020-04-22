namespace BearMyBanner.Settings
{
    public static class IBMBSettingsExtension
    {
        public static IBMBSettings SetDefaults(this IBMBSettings settings)
        {
            settings.AllowSoldiers = true;
            settings.AllowCaravanGuards = false;
            settings.AllowMercenaries = false;
            settings.AllowBandits = false;
            settings.AllowInfantry = true;
            settings.AllowMounted = true;
            settings.AllowRanged = false;
            settings.AllowMountedRanged = false;

            settings.FilterTiers = true;
            settings.AllowTier1 = false;
            settings.AllowTier2 = false;
            settings.AllowTier3 = false;
            settings.AllowTier4 = true;
            settings.AllowTier5 = true;
            settings.AllowTier6 = true;
            settings.AllowTier7Plus = false;

            settings.AllowPlayer = false;
            settings.AllowCompanions = false;
            settings.AllowNobles = false;

            settings.BearerToTroopRatio = 7;
            settings.UseTroopSpecs = false;

            settings.AllowSieges = true;
            settings.SiegeAttackersUseBanners = true;
            settings.SiegeDefendersUseBanners = false;

            settings.AllowHideouts = false;
            settings.HideoutAttackersUseBanners = false;
            settings.HideoutBanditsUseBanners = false;

            settings.ShowMessages = true;
            settings.WhiteMessages = false;

            settings.EnableArmyBanners = true;
            settings.GiveArmyBannerToPlayer = false;

            return settings;
        }
    }
}
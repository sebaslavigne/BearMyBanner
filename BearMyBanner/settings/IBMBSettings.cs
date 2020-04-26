namespace BearMyBanner.Settings
{
    public interface IBMBSettings
    {
        bool AllowSieges { get; set; }
        bool SiegeAttackersUseBanners { get; set; }
        bool SiegeDefendersUseBanners { get; set; }
        bool AllowHideouts { get; set; }
        bool HideoutAttackersUseBanners { get; set; }
        bool HideoutBanditsUseBanners { get; set; }
        bool TournamentBanners { get; set; }
        bool TournamentThemes { get; set; }
        bool TournamentBannersInShields { get; set; }
        int BearerToTroopRatio { get; set; }
        bool UseTroopSpecs { get; set; }
        bool AllowSoldiers { get; set; }
        bool AllowCaravanGuards { get; set; }
        bool AllowMercenaries { get; set; }
        bool AllowBandits { get; set; }
        bool AllowInfantry { get; set; }
        bool AllowRanged { get; set; }
        bool AllowMounted { get; set; }
        bool AllowMountedRanged { get; set; }
        bool FilterTiers { get; set; }
        bool AllowTier1 { get; set; }
        bool AllowTier2 { get; set; }
        bool AllowTier3 { get; set; }
        bool AllowTier4 { get; set; }
        bool AllowTier5 { get; set; }
        bool AllowTier6 { get; set; }
        bool AllowTier7Plus { get; set; }
        bool AllowPlayer { get; set; }
        bool AllowCompanions { get; set; }
        bool AllowNobles { get; set; }
        bool ShowMessages { get; set; }
        bool WhiteMessages { get; set; }
        bool ReloadFiles { get; set; }
    }
}
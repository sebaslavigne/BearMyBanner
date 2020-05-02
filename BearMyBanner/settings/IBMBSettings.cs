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
        bool TownCastleVisitBanner { get; set; }
        bool VillageVisitBanner { get; set; }
        int BearerToTroopRatio { get; set; }
        UnitCountMode UnitCountMode { get; set; }
        bool AllowSoldiers { get; set; }
        bool AllowCaravanGuards { get; set; }
        bool AllowMercenaries { get; set; }
        bool AllowBandits { get; set; }
        bool AllowInfantry { get; set; }
        bool AllowRanged { get; set; }
        bool AllowMounted { get; set; }
        bool AllowMountedRanged { get; set; }
        bool FilterTiers { get; set; }
        string AllowedTiers { get; set; }
        bool AllowPlayer { get; set; }
        bool AllowCompanions { get; set; }
        bool AllowNobles { get; set; }
        bool DropOnLowHealth { get; set; }
        int DropHealthThreshold { get; set; }
        bool DropOnRetreat { get; set; }
        int DropRetreatChance { get; set; }
        bool DropWeightedRetreat { get; set; }
        bool ShowMessages { get; set; }
        bool WhiteMessages { get; set; }
        bool ReloadFiles { get; set; }
    }
}
using System;

namespace BearMyBanner.Settings
{
    [Serializable]
    public class BMBSettings : IBMBSettings
    {
        public static IBMBSettings Instance => GetInstance();

        private static IBMBSettings _settings;

        private static IBMBSettings GetInstance()
        {
            if (_settings == null)
            {
                _settings = SettingsLoader.LoadBMBSettings();
            }
            return _settings;
        }

        public static IBMBSettings Reload()
        {
            _settings = null;
            return Instance;
        }

        internal BMBSettings()
        {
        }

        public bool AllowSieges { get; set; }
        public bool SiegeAttackersUseBanners { get; set; }
        public bool SiegeDefendersUseBanners { get; set; }
        public bool AllowHideouts { get; set; }
        public bool HideoutAttackersUseBanners { get; set; }
        public bool HideoutBanditsUseBanners { get; set; }
        public bool TournamentBanners { get; set; }
        public bool TournamentThemes { get; set; }
        public bool TournamentBannersInShields { get; set; }
        public bool TownCastleVisitBanner { get; set; }
        public bool VillageVisitBanner { get; set; }
        public int BearerToTroopRatio { get; set; }
        public UnitCountMode UnitCountMode { get; set; }
        public bool AllowSoldiers { get; set; }
        public bool AllowCaravanGuards { get; set; }
        public bool AllowMercenaries { get; set; }
        public bool AllowBandits { get; set; }
        public bool AllowInfantry { get; set; }
        public bool AllowRanged { get; set; }
        public bool AllowMounted { get; set; }
        public bool AllowMountedRanged { get; set; }
        public bool FilterTiers { get; set; }
        public string AllowedTiers { get; set; }
        public bool AllowPlayer { get; set; }
        public bool AllowCompanions { get; set; }
        public bool AllowNobles { get; set; }
        public bool ShowMessages { get; set; }
        public bool DropOnLowHealth { get; set; }
        public int DropHealthThreshold { get; set; }
        public bool DropOnRetreat { get; set; }
        public int DropRetreatChance { get; set; }
        public bool DropWeightedRetreat { get; set; }
        public bool WhiteMessages { get; set; }
        public bool ReloadFiles { get; set; }
    }
}

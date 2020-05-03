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
        public CaravanAssignMode AllowCaravanGuards { get; set; }
        public bool AllowMercenaries { get; set; }
        public BanditAssignMode AllowBandits { get; set; }
        public bool FilterTiers { get; set; }
        public string AllowedTiers { get; set; }
        public bool AllowTypeInfantry { get; set; }
        public bool AllowTypeRanged { get; set; }
        public bool AllowTypeMounted { get; set; }
        public bool AllowTypeMountedRanged { get; set; }
        public bool AllowFormationInfantry { get; set; }
        public bool AllowFormationRanged { get; set; }
        public bool AllowFormationCavalry { get; set; }
        public bool AllowFormationHorseArcher { get; set; }
        public bool AllowFormationSkirmisher { get; set; }
        public bool AllowFormationHeavyInfantry { get; set; }
        public bool AllowFormationLightCavalry { get; set; }
        public bool AllowFormationHeavyCavalry { get; set; }
        public bool AllowPlayer { get; set; }
        public bool AllowCompanions { get; set; }
        public bool AllowNobles { get; set; }
        public bool ShowMessages { get; set; }
        public bool DropOnLowHealth { get; set; }
        public int DropHealthThreshold { get; set; }
        public DropRetreatMode DropRetreatMode { get; set; }
        public float DropRetreatChance { get; set; }
        public bool WhiteMessages { get; set; }
        public bool ReloadFiles { get; set; }
        public bool KonamiCode { get; set; }
    }
}

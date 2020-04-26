using System;

namespace BearMyBanner.Settings
{
    [Serializable]
    public class BMBFormationBanners : IBMBFormationBanners
    {
        public static IBMBFormationBanners Instance => GetInstance();

        private static IBMBFormationBanners _formationBanners;

        private static IBMBFormationBanners GetInstance()
        {
            if (_formationBanners == null)
            {
                _formationBanners = SettingsLoader.LoadBMBFormationBanners();
            }
            return _formationBanners;
        }

        public static IBMBFormationBanners Reload()
        {
            _formationBanners = null;
            return Instance;
        }

        internal BMBFormationBanners()
        {
        }

        public bool EnableFormationBanners { get; set; }
        public bool CompanionsUseFormationBanners { get; set; }
        public bool UseInShields { get; set; }

        public string Infantry { get; set; }
        public string Ranged { get; set; }
        public string Cavalry { get; set; }
        public string HorseArcher { get; set; }
        public string Skirmisher { get; set; }
        public string HeavyInfantry { get; set; }
        public string LightCavalry { get; set; }
        public string HeavyCavalry { get; set; }
    }
}

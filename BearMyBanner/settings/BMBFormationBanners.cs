using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Settings
{
    class BMBFormationBanners : IBMBFormationBanners
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

        internal BMBFormationBanners()
        {
        }

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

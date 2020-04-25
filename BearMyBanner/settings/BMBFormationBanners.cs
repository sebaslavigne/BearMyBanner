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

        public string KeyFormation1 { get; set; }
        public string KeyFormation2 { get; set; }
        public string KeyFormation3 { get; set; }
        public string KeyFormation4 { get; set; }
        public string KeyFormation5 { get; set; }
        public string KeyFormation6 { get; set; }
        public string KeyFormation7 { get; set; }
        public string KeyFormation8 { get; set; }
    }
}

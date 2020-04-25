using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Settings
{
    public static class IBMBFormationBannersExtension
    {

        public static IBMBFormationBanners SetDefaults(this IBMBFormationBanners formationBanners)
        {
            formationBanners.KeyFormation1 = "";
            formationBanners.KeyFormation2 = "";
            formationBanners.KeyFormation3 = "";
            formationBanners.KeyFormation4 = "";
            formationBanners.KeyFormation5 = "";
            formationBanners.KeyFormation6 = "";
            formationBanners.KeyFormation7 = "";
            formationBanners.KeyFormation8 = "";

            return formationBanners;
        }
    }
}

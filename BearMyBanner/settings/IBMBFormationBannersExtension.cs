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

        public static IBMBFormationBanners SetDefaultFormationSettings(this IBMBFormationBanners formationBanners)
        {
            formationBanners.Infantry = "1.111.111.1536.1536.756.764.1.0.0.301.116.116.700.700.764.764.0.0.225";
            formationBanners.Ranged = "1.111.111.1536.1536.756.764.1.0.0.311.116.116.700.700.764.764.0.0.315";
            formationBanners.Cavalry = "1.111.111.1536.1536.756.764.0.0.0.124.116.116.500.500.784.864.0.0.0.301.116.116.300.300.764.564.0.0.225";
            formationBanners.HorseArcher = "1.111.111.1536.1536.756.764.0.0.0.124.116.116.500.500.784.864.0.0.0.311.116.116.300.300.764.564.0.0.315";
            formationBanners.Skirmisher = "1.111.111.1536.1536.756.764.1.0.0.304.116.116.600.600.764.864.0.0.45.304.116.116.600.600.764.664.0.0.45.304.116.116.600.600.764.764.0.0.45";
            formationBanners.HeavyInfantry = "1.111.111.1536.1536.756.764.1.0.0.318.111.116.700.700.764.764.1.1.0.301.116.116.700.700.764.764.0.0.225";
            formationBanners.LightCavalry = "1.111.111.1536.1536.756.764.0.0.0.124.116.116.500.500.784.864.0.0.0.304.116.116.300.300.764.614.0.0.315.304.116.116.300.300.764.514.0.0.315.304.116.116.300.300.764.564.0.0.315";
            formationBanners.HeavyCavalry = "1.111.111.1536.1536.756.764.0.0.0.124.116.116.500.500.784.864.0.0.0.318.111.116.300.300.764.564.1.0.0.301.116.116.300.300.764.564.0.0.225";

            return formationBanners;
        }

        public static void CopyCodesTo(this IBMBFormationBanners from, IBMBFormationBanners to)
        {
            to.Infantry = from.Infantry;
            to.Ranged = from.Ranged;
            to.Cavalry = from.Cavalry;
            to.HorseArcher = from.HorseArcher;
            to.Skirmisher = from.Skirmisher;
            to.HeavyInfantry = from.HeavyInfantry;
            to.LightCavalry = from.LightCavalry;
            to.HeavyCavalry = from.HeavyCavalry;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace BearMyBanner.Wrapper
{
    class TournamentBanner : IBMBBanner
    {

        public TournamentBanner()
        {
            PrimaryColor = 1;
            SecondaryColor = 1;
            Mesh = 1;
        }

        public void GenerateKey()
        {
            Banner banner = new Banner(BaseKey);
            if (!HasIcon)
            {
                banner.ClearAllIcons();
                banner.SetBackgroundMeshId(Mesh);
                banner.ChangeBaseColors(PrimaryColor, SecondaryColor);
                banner.BannerDataList[0].Mirror = Mirrored;
            }
            else
            {
                banner.ChangeIconMesh(Mesh);
                banner.ChangeBaseColors(SecondaryColor, SecondaryColor);
                banner.ChangeIconColor(PrimaryColor);
                banner.BannerDataList[1].Mirror = Mirrored;
            }
            Key = banner.Serialize();
        }

        private const string BaseKey = "1.35.19.1836.1836.764.764.1.0.0.505.116.116.500.500.764.764.0.0.0";

        public string Key { get; set; }

        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public int Mesh { get; set; }
        public bool HasIcon { get; set; }
        public bool Mirrored { get; set; }
    }
}

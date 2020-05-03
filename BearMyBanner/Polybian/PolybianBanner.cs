using System;
using System.Collections.Generic;

namespace BearMyBanner.Settings
{
    [Serializable]
    public class PolybianBanner
    {

        public PolybianBanner() { }

        public PolybianBanner(string troopId, List<string> bannerCodes)
        {
            TroopId = troopId;
            BannerCodes = bannerCodes;
        }

        public string TroopId { get; set; }
        public List<string> BannerCodes { get; set; } = new List<string>();
    }
}

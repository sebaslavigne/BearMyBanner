using System;
using System.Collections.Generic;

namespace BearMyBanner.Settings
{
    [Serializable]
    public class PolybianUnit
    {

        public PolybianUnit() { }

        public PolybianUnit(string troopId, List<string> bannerCodes)
        {
            TroopId = troopId;
            BannerCodes = bannerCodes;
        }

        public string TroopId { get; set; }
        public List<string> BannerCodes { get; set; } = new List<string>();
    }
}

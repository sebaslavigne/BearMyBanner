using BearMyBanner.Settings;

namespace BearMyBannerTests
{
    public class TestFormations : IBMBFormationBanners
    {
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

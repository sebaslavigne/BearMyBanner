namespace BearMyBanner.Settings
{
    public interface IBMBFormationBanners
    {
        bool EnableFormationBanners { get; set; }
        bool CompanionsUseFormationBanners { get; set; }
        bool UseInShields { get; set; }

        string Infantry { get; set; }
        string Ranged { get; set; }
        string Cavalry { get; set; }
        string HorseArcher { get; set; }
        string Skirmisher { get; set; }
        string HeavyInfantry { get; set; }
        string LightCavalry { get; set; }
        string HeavyCavalry { get; set; }
    }
}

using System.Collections.Generic;

namespace BearMyBanner.Settings
{
    public interface IPolybianConfig
    {
        List<PolybianBanner> PolybianBanners { get; set; }
        Dictionary<string, PolybianBanner> PolybianDict { get; }
    }
}

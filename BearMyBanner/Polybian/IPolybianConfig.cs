using System.Collections.Generic;

namespace BearMyBanner.Settings
{
    public interface IPolybianConfig
    {
        List<PolybianUnit> PolybianUnits { get; set; }
        Dictionary<string, PolybianUnit> PolybianDict { get; }
    }
}

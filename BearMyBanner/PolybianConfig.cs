using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BearMyBanner.Settings
{
    [Serializable]
    public class PolybianConfig : IPolybianConfig
    {
        public static IPolybianConfig Instance => GetInstance();

        private static IPolybianConfig _polybianConfig;

        private static IPolybianConfig GetInstance()
        {
            if (_polybianConfig == null)
            {
                _polybianConfig = SettingsLoader.LoadPolybianConfig();
            }
            return _polybianConfig;
        }

        public static IPolybianConfig Reload()
        {
            _polybianConfig = null;
            return Instance;
        }

        internal PolybianConfig() { }

        public List<PolybianBanner> PolybianBanners { get; set; }

        [XmlIgnore]
        public Dictionary<string, PolybianBanner> PolybianDict => CreateDictionary();

        private Dictionary<string, PolybianBanner> CreateDictionary()
        {
            Dictionary<string, PolybianBanner> dict = new Dictionary<string, PolybianBanner>();
            foreach (PolybianBanner polybianBanner in PolybianBanners)
            {
                dict.Add(polybianBanner.TroopId, polybianBanner);
            }
            return dict;
        }
    }
}

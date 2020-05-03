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

        public List<PolybianUnit> PolybianUnits { get; set; }

        [XmlIgnore]
        public Dictionary<string, PolybianUnit> PolybianDict => CreateDictionary();

        private Dictionary<string, PolybianUnit> CreateDictionary()
        {
            Dictionary<string, PolybianUnit> dict = new Dictionary<string, PolybianUnit>();
            foreach (PolybianUnit polybianUnit in PolybianUnits)
            {
                dict.Add(polybianUnit.TroopId, polybianUnit);
            }
            return dict;
        }
    }
}

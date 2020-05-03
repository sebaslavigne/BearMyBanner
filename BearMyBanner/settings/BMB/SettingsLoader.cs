using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace BearMyBanner.Settings
{
    internal class SettingsLoader
    {
        internal static IBMBSettings LoadBMBSettings()
        {
            string modulePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
            string settingsPath = Path.Combine(
                modulePath,
                "ModuleData",
                "BMBSettings.xml"
            );

            IBMBSettings settings;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(BMBSettings));
                using (StreamReader streamReader = new StreamReader(settingsPath))
                {
                    settings = (BMBSettings)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                Main.LoadingMessages.Add(("BMB Error loading settings: settings file not found or is corrupt", true));
                IBMBSettings BMBSettings = new BMBSettings();
                settings = BMBSettings.SetDefaultSettings();
                Main.LoadingMessages.Add(("Bear my Banner will use default settings", true));

                //Use when adding new settings to easily create new file
                //SerializeSettings<BMBSettings>(settingsPath, (BMBSettings)settings);

            }
            return settings;
        }

        internal static IBMBFormationBanners LoadBMBFormationBanners()
        {
            string modulePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
            string settingsPath = Path.Combine(
                modulePath,
                "ModuleData",
                "BMBFormationBanners.xml"
            );

            IBMBFormationBanners formationBanners;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(BMBFormationBanners));
                using (StreamReader streamReader = new StreamReader(settingsPath))
                {
                    formationBanners = (BMBFormationBanners)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                Main.LoadingMessages.Add(("BMB Error loading settings: formation banners file not found or is corrupt", true));
                IBMBFormationBanners BMBFormationBanners = new BMBFormationBanners();
                formationBanners = BMBFormationBanners.SetDefaultFormationSettings();
                Main.LoadingMessages.Add(("Bear my Banner will use default formation banners", true));

                //Use when adding new settings to easily create new file
                //SerializeSettings<BMBFormationBanners>(settingsPath, (BMBFormationBanners)formationBanners);

            }
            return formationBanners;
        }

        internal static IPolybianConfig LoadPolybianConfig()
        {
            string modulePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
            string settingsPath = Path.Combine(
                modulePath,
                "ModuleData",
                "PolybianConfig.xml"
            );

            IPolybianConfig polybianConfig;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PolybianConfig));
                using (StreamReader streamReader = new StreamReader(settingsPath))
                {
                    polybianConfig = (PolybianConfig)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                Main.LoadingMessages.Add(("Polybian Error loading settings: config file not found or is corrupt", true));
                IPolybianConfig config = new PolybianConfig();
                polybianConfig = config.InitializeTemplateList();
                Main.LoadingMessages.Add(("Polybian will use default banners", true));

                //Use when adding new settings to easily create new file
                //SerializeSettings<PolybianConfig>(settingsPath, (PolybianConfig)polybianConfig);
            }
            return polybianConfig;
        }

        private static void SerializeSettings<T>(string settingsPath, T settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (TextWriter streamWriter = new StreamWriter(settingsPath))
            {
                xmlSerializer.Serialize(streamWriter, settings);
            }
        }
    }
}

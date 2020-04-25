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
            catch (Exception ex)
            {
                Main.LoadingMessages.Add(("BMB Error loading settings: settings file not found or is corrupt", true));
                IBMBSettings BMBSettings = new BMBSettings();
                settings = BMBSettings.SetDefaults();
                Main.LoadingMessages.Add(("Bear my Banner will use default settings", true));

                //Use when adding new settings to easily create new file
                //SerializeDefaults(settingsPath, settings);

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
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(BMBSettings));
                using (StreamReader streamReader = new StreamReader(settingsPath))
                {
                    formationBanners = (BMBFormationBanners)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception ex)
            {
                Main.LoadingMessages.Add(("BMB Error loading settings: formation banners file not found or is corrupt", true));
                IBMBFormationBanners BMBFormationBanners = new BMBFormationBanners();
                formationBanners = BMBFormationBanners.SetDefaults();
                Main.LoadingMessages.Add(("Bear my Banner will use default formation banners", true));

                //Use when adding new settings to easily create new file
                //SerializeDefaults(settingsPath, settings);

            }
            return formationBanners;
        }

        private static void SerializeDefaults(string settingsPath, IBMBSettings settings)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BMBSettings));
            using (TextWriter streamWriter = new StreamWriter(settingsPath))
            {
                xmlSerializer.Serialize(streamWriter, settings);
            }
        }
        private static void SerializeFormationBanners(string settingsPath, IBMBFormationBanners formationBanners)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BMBSettings));
            using (TextWriter streamWriter = new StreamWriter(settingsPath))
            {
                xmlSerializer.Serialize(streamWriter, formationBanners);
            }
        }
    }
}

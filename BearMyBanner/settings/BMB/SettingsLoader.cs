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
            catch (InvalidOperationException invalidOperationEx)
            {
                string message = "BMB Error loading settings\n" + invalidOperationEx.Message;
                if (invalidOperationEx.InnerException != null) message += "\n" + invalidOperationEx.InnerException.Message;
                Main.LoadingMessages.Add((message, true));
                IBMBSettings BMBSettings = new BMBSettings();
                settings = BMBSettings.SetDefaultSettings();
                settings.ReloadFiles = true;
            }
            catch (FileNotFoundException)
            {
                Main.LoadingMessages.Add(("BMB Error: settings file not found", true));
                Main.LoadingMessages.Add(("Bear my Banner will use default settings", true));
                IBMBSettings BMBSettings = new BMBSettings();
                settings = BMBSettings.SetDefaultSettings();

                //Use when adding new settings to easily create new file
                //SerializeSettings<BMBSettings>(settingsPath, (BMBSettings)settings);
            }
            catch (Exception)
            {
                Main.LoadingMessages.Add(("BMB Unexpected Error while loading settings file", true));
                IBMBSettings BMBSettings = new BMBSettings();
                settings = BMBSettings.SetDefaultSettings();
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
            catch (InvalidOperationException invalidOperationEx)
            {
                string message = "BMB Error loading formation banners file\n" + invalidOperationEx.Message;
                if (invalidOperationEx.InnerException != null) message += "\n" + invalidOperationEx.InnerException.Message;
                Main.LoadingMessages.Add((message, true));
                IBMBFormationBanners BMBFormationBanners = new BMBFormationBanners();
                formationBanners = BMBFormationBanners.SetDefaultFormationSettings();
            }
            catch (FileNotFoundException)
            {
                Main.LoadingMessages.Add(("BMB Error: formation banners file not found", true));
                Main.LoadingMessages.Add(("Bear my Banner will use default formation banners", true));
                IBMBFormationBanners BMBFormationBanners = new BMBFormationBanners();
                formationBanners = BMBFormationBanners.SetDefaultFormationSettings();

                //Use when adding new settings to easily create new file
                //SerializeSettings<BMBSettings>(settingsPath, (BMBSettings)settings);
            }
            catch (Exception)
            {
                Main.LoadingMessages.Add(("BMB Unexpected Error while loading formation banners file", true));
                IBMBFormationBanners BMBFormationBanners = new BMBFormationBanners();
                formationBanners = BMBFormationBanners.SetDefaultFormationSettings();
            }
            return formationBanners;
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

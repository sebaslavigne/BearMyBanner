using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System.Collections.Generic;

namespace BearMyBanner
{
    public class Main : MBSubModuleBase
    {

        public const string ModuleFolderName = "BearMyBanner";
        public const string ModName = "Bear my Banner";

        public static List<(string content, bool isError)> LoadingMessages = new List<(string, bool)>();

        private IBMBSettings _settings;
        private IBMBFormationBanners _formationBanners;
        private IBMBFormationBanners _configFileBanners;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                _configFileBanners = BMBFormationBanners.Instance;

                string loadedMessage = "Loaded Bear my Banner";

#if MCMCONFIG
                loadedMessage += ", using MCM";
#elif BMBCONFIG
                _settings = BMBSettings.Instance;
                _formationBanners = BMBFormationBanners.Instance;
                loadedMessage += ", using config files";
#endif

                LoadingMessages.Add((loadedMessage, false));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            try
            {
#if MCMSETTINGS
                try
                {
                    _settings = MCMSettings.Instance;
                    _formationBanners = MCMSettings.Instance;
                    _configFileBanners.CopyCodesTo(_formationBanners);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("There was an error with MCM");
                }
#endif

                PrintLoadingMessages();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);
            try
            {
                if (_settings == null)
                {
                    throw new InvalidOperationException("Settings were not initialized");
                }

                if (_settings.ReloadFiles)
                {
#if BMBCONFIG
                    _settings = BMBSettings.Reload();
#endif
                    _configFileBanners = BMBFormationBanners.Reload();
                    _configFileBanners.CopyCodesTo(_formationBanners);
                    PrintInMessageLog("BMB Configuration files reloaded", 4282569842U);
                    PrintLoadingMessages();
                }

                if (Mission.Current.CombatType == Mission.MissionCombatType.Combat)
                {
                    MissionType missionType = mission.GetMissionType();
                    switch (missionType)
                    {
                        case MissionType.FieldBattle:
                            mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings, _formationBanners, missionType));
                            if (_settings.KonamiCode) mission.AddMissionBehaviour(new KCBehaviour());
                            break;
                        case MissionType.Siege:
                            if (_settings.AllowSieges) mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings, _formationBanners, missionType));
                            break;
                        case MissionType.Hideout:
                            if (_settings.AllowHideouts) mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings, _formationBanners, missionType));
                            break;
                        case MissionType.Tournament:
                            if(_settings.TournamentBanners) mission.AddMissionBehaviour(new TournamentBannerAssignBehaviour(_settings));
                            break;
                        case MissionType.TownVisit:
                            if(_settings.TownCastleVisitBanner) mission.AddMissionBehaviour(new VisitBannerBehaviour());
                            break;
                        case MissionType.VillageVisit:
                            if (_settings.VillageVisitBanner) mission.AddMissionBehaviour(new VisitBannerBehaviour());
                            break;
                        case MissionType.CustomBattle:
                            mission.AddMissionBehaviour(new CustomBattleBannerBehaviour(_settings));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public static void PrintInMessageLog(string message)
        {
            PrintInMessageLog(message, Color.White);
        }

        public static void PrintInMessageLog(string message, uint color)
        {
            PrintInMessageLog(message, Color.FromUint(color));
        }

        public static void PrintInMessageLog(string message, Color color)
        {
            if (BMBSettings.Instance.ShowMessages)
            {
#if MCMCONFIG
                if (MCMSettings.Instance.WhiteMessages) color = Color.White;
#elif BMBCONFIG
                if (BMBSettings.Instance.WhiteMessages) color = Color.White;
#endif

                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
        }

        public static void LogError(Exception ex)
        {
            try
            {
                PrintInMessageLog("BMB Error: " + ex.Message);
            }
            catch (Exception)
            {
                InformationManager.DisplayMessage(new InformationMessage("BMB unexpected error"));
            }
        }

        private static void PrintLoadingMessages()
        {
            foreach ((string content, bool isError) message in LoadingMessages)
            {
                PrintWhileLoading(message.content, message.isError);
            }
            LoadingMessages.Clear();
        }

        public static void PrintWhileLoading(string message, bool isError)
        {
            Color color = isError ? new Color(1f, 0f, 0f, 1f) : Color.FromUint(4282569842U);
            InformationManager.DisplayMessage(new InformationMessage(message, color));
        }
    }
}

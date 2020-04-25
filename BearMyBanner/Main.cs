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

        public static List<(string content, bool isError)> LoadingMessages = new List<(string, bool)>();

        private IBMBSettings _settings;
        private IBMBFormationBanners _formationBanners;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                _settings = BMBSettings.Instance;
                _formationBanners = BMBFormationBanners.Instance;
                LoadingMessages.Add(("Loaded Bear my Banner", false));
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            foreach ((string content, bool isError) message in LoadingMessages)
            {
                PrintWhileLoading(message.content, message.isError);
            }
            LoadingMessages.Clear();
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

                if (Mission.Current.CombatType == Mission.MissionCombatType.Combat)
                {
                    switch (mission.GetMissionType())
                    {
                        case MissionType.FieldBattle:
                        case MissionType.Siege:
                        case MissionType.Hideout:
                            mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings, _formationBanners));
                            break;
                        case MissionType.Tournament:
                            if(_settings.TournamentBanners) mission.AddMissionBehaviour(new TournamentBannerAssignBehaviour(_settings));
                            break;
                        default:
                            break;
                    }
                }
                else if (Mission.Current.CombatType == Mission.MissionCombatType.NoCombat)
                {
                    //TODO add behaviour for town and village visits
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public static void PrintInMessageLog(string message)
        {
            PrintInMessageLog(message, TaleWorlds.Library.Color.White);
        }

        public static void PrintInMessageLog(string message, uint color)
        {
            PrintInMessageLog(message, TaleWorlds.Library.Color.FromUint(color));
        }

        public static void PrintInMessageLog(string message, TaleWorlds.Library.Color color)
        {
            if (BMBSettings.Instance.ShowMessages)
            {
                if (BMBSettings.Instance.WhiteMessages) color = TaleWorlds.Library.Color.White;
                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
        }

        public static void LogError(Exception ex)
        {
            try
            {
                PrintInMessageLog("BMB Error: " + ex.Message);
            }
            catch (Exception) //If there's an exception because settings were not loaded
            {
                InformationManager.DisplayMessage(new InformationMessage("BIG ERROR"));
            }
        }

        public static void PrintWhileLoading(string message, bool isError)
        {
            Color color = isError ? new Color(1f, 0f, 0f, 1f) : Color.FromUint(4282569842U);
            InformationManager.DisplayMessage(new InformationMessage(message, color));
        }
    }
}

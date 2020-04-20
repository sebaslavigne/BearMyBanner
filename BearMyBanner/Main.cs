using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using ModLib;
using BearMyBanner.Settings;
using BearMyBanner.Wrapper;

namespace BearMyBanner
{
    public class Main : MBSubModuleBase
    {

        public const string ModuleFolderName = "BearMyBanner";

        private IBMBSettings _settings;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                FileDatabase.Initialise(ModuleFolderName);
                BMBSettings settings = FileDatabase.Get<BMBSettings>(BMBSettings.InstanceID) ??
                                       (BMBSettings)new BMBSettings().SetDefaults();

                _settings = settings;
                SettingsDatabase.RegisterSettings(settings);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            PrintInMessageLog("Loaded Bear my Banner", TaleWorlds.Library.Color.FromUint(4282569842U));
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
                            mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings));
                            break;
                        case MissionType.Tournament:
                            mission.AddMissionBehaviour(new TournamentBannerAssignBehaviour(_settings));
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
            PrintInMessageLog("BMB Error: " + ex.Message);
        }
    }
}

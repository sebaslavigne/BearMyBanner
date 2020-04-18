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
                LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("Loaded Bear my Banner", Color.FromUint(4282569842U)));
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

                TypedMission typedMission = new TypedMission(mission);

                if (Mission.Current.CombatType == Mission.MissionCombatType.Combat)
                {
                    switch (typedMission.MissionType)
                    {
                        case MissionType.FieldBattle:
                        case MissionType.Siege:
                        case MissionType.Hideout:
                            mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public static void LogInMessageLog(string message)
        {
            if (BMBSettings.Instance.ShowMessages)
            {
                InformationManager.DisplayMessage(new InformationMessage(message));
            }
        }
    }
}

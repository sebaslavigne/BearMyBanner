using System;
using System.Linq;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Engine;
using ModLib;

namespace BearMyBanner
{
    public class Main : MBSubModuleBase
    {

        public const string ModuleFolderName = "BearMyBanner";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                FileDatabase.Initialise(ModuleFolderName);
                BMBSettings settings = FileDatabase.Get<BMBSettings>(BMBSettings.InstanceID);
                if (settings == null) settings = new BMBSettings();
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
                LogInMessageLog(mission.SceneName);
                if (Mission.Current.CombatType == Mission.MissionCombatType.Combat)
                {
                    if (mission.IsFieldBattle
                        || (MissionUtils.IsSiege(mission))
                        || (MissionUtils.IsHideout(mission)))
                    {
                        mission.AddMissionBehaviour(new BattleBannerAssignBehaviour());
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

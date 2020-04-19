using System;
using System.Collections.Generic;
using BearMyBanner.Effects;
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
            LogInMessageLog("Loaded Bear my Banner", Color.FromUint(4282569842U));
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

                    // For battles, we don't want ranged units dropping banners because they had a bow
                    var forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
                    {
                        ItemObject.ItemTypeEnum.Arrows,
                        ItemObject.ItemTypeEnum.Bolts,
                        ItemObject.ItemTypeEnum.Bow,
                        ItemObject.ItemTypeEnum.Crossbow
                    };

                    var gameObjectEditor = new GameObjectEditor(_settings, forbiddenWeapons);

                    switch (mission.GetMissionType())
                    {
                        case MissionType.FieldBattle:
                        case MissionType.Siege:
                        case MissionType.Hideout:
                            mission.AddMissionBehaviour(new BattleBannerAssignBehaviour(_settings, gameObjectEditor));
                            mission.AddMissionBehaviour(new BannerStatusEffectsLogic(_settings, gameObjectEditor));
                            break;
                        case MissionType.Tournament:
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
                LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public static void LogInMessageLog(string message)
        {
                LogInMessageLog(message, Color.White);
        }

        public static void LogInMessageLog(string message, uint color)
        {
            LogInMessageLog(message, Color.FromUint(color));
        }

        public static void LogInMessageLog(string message, Color color)
        {
            if (BMBSettings.Instance.ShowMessages)
            {
                if (BMBSettings.Instance.WhiteMessages) color = Color.White;
                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
        }
    }
}

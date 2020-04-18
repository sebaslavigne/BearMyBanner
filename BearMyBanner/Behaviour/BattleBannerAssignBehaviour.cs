using System;
using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BattleBannerAssignBehaviour : MissionLogic
    {
        private readonly BannerAssignmentController _bannerAssignmentController;
        private readonly GameObjectEditor _gameObjectEditor;

        public BattleBannerAssignBehaviour(IBMBSettings settings)
        {
            _bannerAssignmentController = new BannerAssignmentController(settings);
            _gameObjectEditor = new GameObjectEditor(settings);
        }

        public override void OnCreated()
        {
            base.OnCreated();

            try
            {
                TypedMission mission = new TypedMission(Mission);
                _bannerAssignmentController.FilterAllowedBearerTypes(mission.IsHideout);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                bool agentGetsBanner = _bannerAssignmentController.ProcessBuiltAgent(new CampaignAgent(agent), new TypedMission(Mission));
                if (agentGetsBanner)
                {
                    _gameObjectEditor.AddBannerToAgentSpawnEquipment(agent);
                }
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);//Use LINQ for team parties
            try
            {
                _bannerAssignmentController.ShowBannersEquippedByPartiesInTeam(team);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }
    }
}

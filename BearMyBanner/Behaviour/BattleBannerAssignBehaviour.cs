using System;
using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Effects;
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

        public BattleBannerAssignBehaviour(IBMBSettings settings, GameObjectEditor gameObjectEditor)
        {
            _gameObjectEditor = gameObjectEditor;
            _bannerAssignmentController = new BannerAssignmentController(settings, _gameObjectEditor);
        }

        public override void OnCreated()
        {
            base.OnCreated();

            try
            {
                var nativeCharacterTypes = new List<CharacterObject>();
                MBObjectManager.Instance.GetAllInstancesOfObjectType(ref nativeCharacterTypes);
                var characterTypes = nativeCharacterTypes.Select(t => new CampaignCharacter(t)).ToList();
                _bannerAssignmentController.FilterAllowedBearerTypes(characterTypes, this.Mission.IsHideout());
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
                _bannerAssignmentController.ProcessBuiltAgent(new CampaignAgent(agent), Mission.GetMissionType());
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);
            try
            {
                List<CampaignAgent> teamAgents = team.TeamAgents.Select(ta => new CampaignAgent(ta)).ToList();
                _bannerAssignmentController.ShowBannersEquippedByPartiesInTeam(teamAgents);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }
    }
}

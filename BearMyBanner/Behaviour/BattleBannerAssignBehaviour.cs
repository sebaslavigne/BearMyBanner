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

        public BattleBannerAssignBehaviour(IBMBSettings settings)
        {
            // For battles, we don't want ranged units dropping banners because they had a bow
            var forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };

            var gameObjectEditor = new GameObjectEditor(settings, forbiddenWeapons);
            _bannerAssignmentController = new BannerAssignmentController(settings, gameObjectEditor);
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

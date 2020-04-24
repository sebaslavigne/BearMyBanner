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
        private readonly BattleBannerController _bannerAssignmentController;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;
        private readonly IBMBSettings _settings;

        private Agent bigBannerAgent;

        public BattleBannerAssignBehaviour(IBMBSettings settings)
        {
            _bannerAssignmentController = new BattleBannerController(settings);
            _settings = settings;

            // For battles, we don't want ranged units dropping banners because they had a bow
            _forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };
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

                List<ItemObject> testBanners = new List<ItemObject>();
                MBObjectManager.Instance.GetAllInstancesOfObjectType(ref testBanners);
                ;
                testBanners = testBanners.Where(item => item.Type == ItemObject.ItemTypeEnum.Banner).Select(item => item).ToList();
                ;
                ItemObject bigBanner = MBObjectManager.Instance.GetObject<ItemObject>("banner_big");
                ItemObject campaignBanner = MBObjectManager.Instance.GetObject<ItemObject>("campaign_banner_small");
                //can't be done
                //bigBanner.MultiMeshName = campaignBanner.MultiMeshName;

            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                var campaignAgent = new CampaignAgent(agent);
                var missionType = this.Mission.GetMissionType();

                if (_bannerAssignmentController.AgentIsEligible(campaignAgent, missionType)
                    && _bannerAssignmentController.AgentGetsBanner(campaignAgent)) 
                {
                    agent.AddBannerToSpawnEquipment(_forbiddenWeapons);
                    if (bigBannerAgent == null && agent.Team.IsPlayerTeam) bigBannerAgent = agent;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);
            try
            {
                List<CampaignAgent> teamAgents = team.TeamAgents.Select(ta => new CampaignAgent(ta)).ToList();

                Dictionary<string, uint> partiesInTeam = teamAgents
                .DistinctBy(ta => ta.PartyName)
                .ToDictionary(ta => ta.PartyName, ta => ta.PartyColor);

                _bannerAssignmentController.PrintBannersEquippedByPartiesInTeam(partiesInTeam);

                if (bigBannerAgent != null)
                {
                    bigBannerAgent.ModifyBannerMeshAndSize();
                    Main.PrintInMessageLog("Big banner given to " + bigBannerAgent.Character.Name);
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}

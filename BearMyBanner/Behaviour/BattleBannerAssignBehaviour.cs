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
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;
        private readonly IBMBSettings _settings;

        public BattleBannerAssignBehaviour(IBMBSettings settings)
        {
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
            }
            catch (Exception ex)
            {
                Main.PrintInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                var campaignAgent = new CampaignAgent(agent);
                var missionType = this.Mission.GetMissionType();
                bool agentGetsBanner = false;

                if (_bannerAssignmentController.AllowedBearerTypes.Contains(campaignAgent.Character))
                {
                    if (missionType == MissionType.FieldBattle)
                    {
                        agentGetsBanner = _bannerAssignmentController.ProcessAgent(campaignAgent);
                    }
                    else if (_settings.AllowSieges && missionType == MissionType.Siege)
                    {
                        if ((_settings.SiegeAttackersUseBanners && campaignAgent.IsAttacker)
                            || (_settings.SiegeDefendersUseBanners && campaignAgent.IsDefender))
                        {
                            agentGetsBanner = _bannerAssignmentController.ProcessAgent(campaignAgent);
                        }
                    }
                    else if (_settings.AllowHideouts && missionType == MissionType.Hideout)
                    {
                        if ((_settings.HideoutAttackersUseBanners && campaignAgent.IsAttacker)
                            || (_settings.HideoutBanditsUseBanners && campaignAgent.IsDefender))
                        {
                           agentGetsBanner = _bannerAssignmentController.ProcessAgent(campaignAgent);
                        }
                    }
                }

                if (agentGetsBanner)
                {
                    agent.AddBannerToSpawnEquipment(_forbiddenWeapons);
                }
            }
            catch (Exception ex)
            {
                Main.PrintInMessageLog("BMB Error: " + ex.Message);
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
            }
            catch (Exception ex)
            {
                Main.PrintInMessageLog("BMB Error: " + ex.Message);
            }
        }
    }
}

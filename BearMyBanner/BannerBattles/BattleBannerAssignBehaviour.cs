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
        private readonly Dictionary<FormationClass, string> _formationKeys;

        private readonly IBMBSettings _settings;
        private readonly IBMBFormationBanners _formationBanners;

        public BattleBannerAssignBehaviour(IBMBSettings settings, IBMBFormationBanners formationBanners)
        {
            _bannerAssignmentController = new BattleBannerController(settings);
            _settings = settings;
            _formationBanners = formationBanners;

            // For battles, we don't want ranged units dropping banners because they had a bow
            _forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };

            _formationKeys = new Dictionary<FormationClass, string>()
            {
                { FormationClass.Infantry, _formationBanners.Infantry },
                { FormationClass.Ranged, _formationBanners.Ranged },
                { FormationClass.Cavalry, _formationBanners.Cavalry },
                { FormationClass.HorseArcher, _formationBanners.HorseArcher },
                { FormationClass.Skirmisher, _formationBanners.Skirmisher },
                { FormationClass.HeavyInfantry, _formationBanners.HeavyInfantry },
                { FormationClass.LightCavalry, _formationBanners.LightCavalry },
                { FormationClass.HeavyCavalry, _formationBanners.HeavyCavalry }
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
                    agent.RemoveFromEquipment(_forbiddenWeapons);
                    agent.RemoveFromSpawnEquipment(_forbiddenWeapons);

                    FormationClass formationIndex = agent.Formation != null ? agent.Formation.FormationIndex : FormationClass.Unset;
                    if (agent.Formation != null && _formationKeys.ContainsKey(formationIndex))
                    {
                        agent.EquipBanner(_formationKeys[formationIndex]);
                    }
                    else
                    {
                        agent.EquipBanner();
                    }
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
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}

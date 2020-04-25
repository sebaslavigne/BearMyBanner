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
        private readonly Dictionary<int, string> _formationKeys;

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

            _formationKeys = new Dictionary<int, string>()
            {
                { (int)FormationClass.Infantry, _formationBanners.Infantry },
                { (int)FormationClass.Ranged, _formationBanners.Ranged },
                { (int)FormationClass.Cavalry, _formationBanners.Cavalry },
                { (int)FormationClass.HorseArcher, _formationBanners.HorseArcher },
                { (int)FormationClass.Skirmisher, _formationBanners.Skirmisher },
                { (int)FormationClass.HeavyInfantry, _formationBanners.HeavyInfantry },
                { (int)FormationClass.LightCavalry, _formationBanners.LightCavalry },
                { (int)FormationClass.HeavyCavalry, _formationBanners.HeavyCavalry }
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
                    agent.RemoveForbiddenItems(_forbiddenWeapons);
                    agent.EquipBanner(_formationKeys[(int)agent.Formation.FormationIndex]);
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

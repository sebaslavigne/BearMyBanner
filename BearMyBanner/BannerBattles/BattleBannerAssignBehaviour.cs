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
        private readonly Dictionary<FormationClass, Banner> _formationBanners;

        private readonly IBMBSettings _settings;
        private readonly IBMBFormationBanners _formationBannerSettings;

        private List<Agent> _spawnedAgents = new List<Agent>();

        public BattleBannerAssignBehaviour(IBMBSettings settings, IBMBFormationBanners formationBannerSettings)
        {
            _bannerAssignmentController = new BattleBannerController(settings);
            _settings = settings;
            _formationBannerSettings = formationBannerSettings;

            // For battles, we don't want ranged units dropping banners because they had a bow
            _forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };

            _formationBanners = new Dictionary<FormationClass, Banner>()
            {
                { FormationClass.Infantry, new Banner(_formationBannerSettings.Infantry) },
                { FormationClass.Ranged, new Banner(_formationBannerSettings.Ranged) },
                { FormationClass.Cavalry, new Banner(_formationBannerSettings.Cavalry) },
                { FormationClass.HorseArcher, new Banner(_formationBannerSettings.HorseArcher) },
                { FormationClass.Skirmisher, new Banner(_formationBannerSettings.Skirmisher) },
                { FormationClass.HeavyInfantry, new Banner(_formationBannerSettings.HeavyInfantry) },
                { FormationClass.LightCavalry, new Banner(_formationBannerSettings.LightCavalry) },
                { FormationClass.HeavyCavalry, new Banner(_formationBannerSettings.HeavyCavalry) }
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
                _spawnedAgents.Add(agent);
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        public override void OnPreMissionTick(float dt)
        {
            base.OnPreMissionTick(dt);
            try
            {
                if (_spawnedAgents.IsEmpty()) return;

                foreach (Agent agent in _spawnedAgents)
                {
                    AfterAgentSpawned(agent);
                }
                _spawnedAgents.Clear();
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        /// <summary>
        /// Should take place after agent is built AND WieldInitialWeapons is invoked
        /// </summary>
        /// <param name="agent"></param>
        private void AfterAgentSpawned(Agent agent)
        {
            var campaignAgent = new CampaignAgent(agent);
            var missionType = this.Mission.GetMissionType();

            if (_bannerAssignmentController.AgentIsEligible(campaignAgent, missionType)
                && _bannerAssignmentController.AgentGetsBanner(campaignAgent))
            {

                agent.RemoveFromEquipment(_forbiddenWeapons);

                FormationClass formationIndex = agent.Formation != null ? agent.Formation.FormationIndex : FormationClass.Unset;
                if (agent.Formation != null && _formationBanners.ContainsKey(formationIndex))
                {
                    agent.EquipBanner(_formationBanners[formationIndex]);
                }
                else
                {
                    agent.EquipBanner();
                }
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

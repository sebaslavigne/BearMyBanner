using System;
using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BearMyBanner
{
    public class BattleBannerAssignBehaviour : MissionLogic
    {
        private readonly BattleBannerController _controller;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;
        private readonly Dictionary<FormationGroup, Banner> _formationBanners;

        private readonly IBMBSettings _settings;
        private readonly IBMBFormationBanners _formationBannerSettings;

        private List<Agent> _spawnedAgents = new List<Agent>();
        private bool _initialUnitsSpawned = false;

        public BattleBannerAssignBehaviour(IBMBSettings settings, IBMBFormationBanners formationBannerSettings, MissionType missionType)
        {
            _controller = new BattleBannerController(settings, formationBannerSettings, missionType);
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

            _formationBanners = new Dictionary<FormationGroup, Banner>()
            {
                { FormationGroup.Infantry, new Banner(_formationBannerSettings.Infantry) },
                { FormationGroup.Ranged, new Banner(_formationBannerSettings.Ranged) },
                { FormationGroup.Cavalry, new Banner(_formationBannerSettings.Cavalry) },
                { FormationGroup.HorseArcher, new Banner(_formationBannerSettings.HorseArcher) },
                { FormationGroup.Skirmisher, new Banner(_formationBannerSettings.Skirmisher) },
                { FormationGroup.HeavyInfantry, new Banner(_formationBannerSettings.HeavyInfantry) },
                { FormationGroup.LightCavalry, new Banner(_formationBannerSettings.LightCavalry) },
                { FormationGroup.HeavyCavalry, new Banner(_formationBannerSettings.HeavyCavalry) }
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
                _controller.FilterAllowedBearerTypes(characterTypes);
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
                OnInitialUnitsSpawned();
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
                _spawnedAgents.Clear();
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

            if (_formationBanners.ContainsKey(campaignAgent.Formation) && _controller.AgentGetsFancyShield(campaignAgent))
            {
                agent.SwitchShieldBanner(_formationBanners[campaignAgent.Formation]);
            }

            if (_controller.AgentIsEligible(campaignAgent)
                && _controller.AgentGetsBanner(campaignAgent))
            {
                agent.RemoveFromEquipment(_forbiddenWeapons);
                agent.AddComponent(new DropBannerComponent(agent));

                if (_formationBanners.ContainsKey(campaignAgent.Formation) && _controller.AgentGetsFancyBanner(campaignAgent))
                {
                    agent.EquipBanner(_formationBanners[campaignAgent.Formation]);
                }
                else
                {
                    agent.EquipBanner();
                }
            }
        }

        private void OnInitialUnitsSpawned()
        {
            try
            {
                if (_initialUnitsSpawned) return;

                foreach (Team team in this.Mission.Teams)
                {
                    List<CampaignAgent> teamAgents = team.TeamAgents.Select(ta => new CampaignAgent(ta)).ToList();

                    Dictionary<string, uint> partiesInTeam = teamAgents
                    .DistinctBy(ta => ta.PartyName)
                    .ToDictionary(ta => ta.PartyName, ta => ta.PartyColor);

                    _controller.PrintBannersEquippedByPartiesInTeam(partiesInTeam);
                }

                _initialUnitsSpawned = true;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}

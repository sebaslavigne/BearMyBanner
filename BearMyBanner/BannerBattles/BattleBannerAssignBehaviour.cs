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
        private readonly DropBannerController _dropBannerController;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        private readonly IBMBSettings _settings;
        private readonly IPolybianConfig _polybianConfig;

        private List<Agent> _spawnedAgents = new List<Agent>();
        private bool _initialUnitsSpawned = false;
        private bool _unprocessedUnits = false;

        public BattleBannerAssignBehaviour(IBMBSettings settings, IPolybianConfig polybianConfig, MissionType missionType)
        {
            _controller = new BattleBannerController(settings, polybianConfig, missionType);
            _dropBannerController = new DropBannerController(settings);
            _settings = settings;
            _polybianConfig = polybianConfig;

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
                _unprocessedUnits = true;
                if (agent.IsHuman) _spawnedAgents.Add(agent);
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
                if (!_unprocessedUnits) return;

                foreach (Agent agent in _spawnedAgents)
                {
                    AfterAgentSpawned(agent);
                }
                _spawnedAgents.Clear();
                _unprocessedUnits = false;
                OnInitialUnitsSpawned();
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
                _spawnedAgents.Clear();
                _unprocessedUnits = false;
            }
        }

        /// <summary>
        /// Should take place after agent is built AND WieldInitialWeapons is invoked
        /// </summary>
        /// <param name="agent"></param>
        private void AfterAgentSpawned(Agent agent)
        {
            var campaignAgent = new CampaignAgent(agent);

            if (_controller.PolybianUnitExists(campaignAgent))
            {
                _controller.CountAgentForPolybian(campaignAgent);
                agent.SwitchShieldBanner(_controller.GetPolybianBannerForAgent(campaignAgent));
            }

            if (_controller.AgentIsEligible(campaignAgent)
                && _controller.AgentGetsBanner(campaignAgent))
            {
                agent.RemoveFromEquipment(_forbiddenWeapons);
                agent.AddComponent(new DropBannerComponent(agent, _settings, _dropBannerController));

                if (_controller.PolybianUnitExists(campaignAgent))
                {
                    agent.EquipBanner(_controller.GetPolybianBannerForAgent(campaignAgent));
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

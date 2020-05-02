using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BearMyBanner
{
    public class CustomBattleBannerBehaviour : MissionLogic
    {
        private readonly BattleBannerController _controller;
        private readonly DropBannerController _dropBannerController;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        private readonly IBMBSettings _settings;

        private List<Agent> _spawnedAgents = new List<Agent>();
        private bool _initialUnitsSpawned = false;

        public CustomBattleBannerBehaviour(IBMBSettings settings)
        {
            _controller = new BattleBannerController(settings, null, MissionType.CustomBattle);
            _dropBannerController = new DropBannerController(settings);
            _settings = settings;

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
                var nativeCharacterTypes = new List<BasicCharacterObject>();
                MBObjectManager.Instance.GetAllInstancesOfObjectType(ref nativeCharacterTypes);

                var characterTypes = nativeCharacterTypes.Select(t => new CustomBattleCharacter(t)).ToList();
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

                _spawnedAgents.Shuffle<Agent>();
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
            var battleAgent = new CustomBattleAgent(agent);

            if (_controller.AgentIsEligible(battleAgent)
                && _controller.AgentGetsBanner(battleAgent))
            {
                agent.RemoveFromEquipment(_forbiddenWeapons);
                agent.AddComponent(new DropBannerComponent(agent, _settings, _dropBannerController));
                agent.EquipBanner();
            }

        }

        private void OnInitialUnitsSpawned()
        {
            try
            {
                if (_initialUnitsSpawned) return;

                foreach (Team team in this.Mission.Teams)
                {
                    List<CustomBattleAgent> teamAgents = team.TeamAgents.Select(ta => new CustomBattleAgent(ta)).ToList();

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

using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class CustomBattleBannerBehaviour : MissionLogic
    {
        private readonly BattleBannerController _controller;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        private readonly IBMBSettings _settings;

        private List<Agent> _spawnedAgents = new List<Agent>();
        private bool _initialUnitsSpawned = false;

        public CustomBattleBannerBehaviour(IBMBSettings settings)
        {
            _controller = new BattleBannerController(settings, null);
            _settings = settings;

            _forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };
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
            var battleAgent = new CustomBattleAgent(agent);

        }

        private void OnInitialUnitsSpawned()
        {
            try
            {
                _initialUnitsSpawned = true;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}

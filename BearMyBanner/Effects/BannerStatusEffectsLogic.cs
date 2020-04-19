using System.Collections.Generic;
using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Effects
{
    public class BannerStatusEffectsLogic : MissionLogic
    {
        private readonly IBMBSettings _settings;
        private readonly GameObjectEditor _gameObjectEditor;
        public IEnumerable<Agent> AgentsWithBanners => _agentsWithBanners;
        private readonly List<Agent> _agentsWithBanners;

        public BannerStatusEffectsLogic(IBMBSettings settings, GameObjectEditor gameObjectEditor)
        {
            _settings = settings;
            _gameObjectEditor = gameObjectEditor;
            _agentsWithBanners = new List<Agent>();
        }

        public override void OnMissionTick(float dt)
        {
            _agentsWithBanners.Clear();
            foreach (var agent in Mission.Agents)
            {
                if (_gameObjectEditor.CheckIfAgentHasBanner(new CampaignAgent(agent)))
                {
                    _agentsWithBanners.Add(agent);
                }
            }

            base.OnMissionTick(dt);
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            agent.AddComponent(new BannerStatusEffectsComponent(agent, _gameObjectEditor));

            base.OnAgentBuild(agent, banner);
        }
    }
}
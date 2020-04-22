using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Effects
{
    public class BannerEffectsMissionLogic : MissionLogic
    {
        private readonly IBMBSettings _settings;
        private readonly GameObjectEditor _gameObjectEditor;
        public IReadOnlyDictionary<int, Agent> AgentsWithBanners => _agentsWithBanners;
        private readonly Dictionary<int, Agent> _agentsWithBanners;

        public Dictionary<int, float> TimeSinceLoosingBanner = new Dictionary<int, float>();

        private readonly Dictionary<int, Agent> _startingBannerCarriers = new Dictionary<int, Agent>();
        private readonly List<Team> _teamsHandled = new List<Team>();

        public BannerEffectsMissionLogic(IBMBSettings settings, GameObjectEditor gameObjectEditor)
        {
            _settings = settings;
            _gameObjectEditor = gameObjectEditor;
            _agentsWithBanners = new Dictionary<int, Agent>();
        }

        public override void OnMissionTick(float dt)
        {
            _agentsWithBanners.Clear();
            foreach (var agent in Mission.Agents)
            {
                if (!_gameObjectEditor.CheckIfAgentHasBanner(new CampaignAgent(agent), BannerType.big))
                    continue;
                
                //handle player picking up enemy team's banner
                if (_agentsWithBanners.TryGetValue(agent.Team.TeamIndex, out var existingCarrier))
                {
                    if (agent.IsAIControlled && !existingCarrier.IsAIControlled)
                    {
                        _agentsWithBanners[agent.Team.TeamIndex] = agent;
                    }

                    continue;
                }

                _agentsWithBanners.Add(agent.Team.TeamIndex, agent);
            }

            foreach (var team in Mission.Teams)
            {
                if (!TimeSinceLoosingBanner.ContainsKey(team.TeamIndex))
                {
                    TimeSinceLoosingBanner.Add(team.TeamIndex, 0.0f);
                }

                if (!_teamsHandled.Contains(team))
                {
                    continue;
                }

                TimeSinceLoosingBanner[team.TeamIndex] += dt;
                if (_agentsWithBanners.ContainsKey(team.TeamIndex))
                {
                    TimeSinceLoosingBanner[team.TeamIndex] = 0.0f;
                }
            }

            base.OnMissionTick(dt);
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            if (_teamsHandled.Contains(team) || team.TeamAgents.Count(a => a.IsHuman) < 50)
            {
                return;
            }

            Agent bestBannerCarrier = null;

            foreach (var agent in team.ActiveAgents)
            {
                if (bestBannerCarrier == null)
                {
                    bestBannerCarrier = agent;
                    continue;
                }

                if (agent.Character.GetPower() > bestBannerCarrier.Character.GetPower())
                {
                    bestBannerCarrier = agent;
                }
            }

            Main.LogInMessageLog($"Adding banner for {team}");
            _gameObjectEditor.AddBigBannerToAgentSpawnEquipment(new CampaignAgent(bestBannerCarrier));
            _teamsHandled.Add(team);

            base.OnFormationUnitsSpawned(team);
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            agent.AddComponent(new BannerEffectsAgentComponent(agent, _gameObjectEditor));

            base.OnAgentBuild(agent, banner);
        }
    }
}
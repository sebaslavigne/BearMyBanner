using System.Linq;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Effects
{
    public class BannerEffectsAgentComponent : AgentComponent
    {
        private readonly IGameObjectEditor _editor;
        private readonly BannerEffectsMissionLogic _logic;
        private float _lastMoraleChange;

        public BannerEffectsAgentComponent(Agent agent, IGameObjectEditor editor) : base(agent)
        {
            _editor = editor;
            _logic = Agent.Mission.GetMissionBehaviour<BannerEffectsMissionLogic>();
        }

        protected override void OnTickAsAI(float dt)
        {
            base.OnTickAsAI(dt);

            if (!Agent.IsHuman || !Agent.IsAIControlled || Agent.Mission.MissionResult?.BattleResolved == true)
            {
                return;
            }

            var moraleChange = _logic.TimeSinceLoosingBanner[Agent.Team.TeamIndex] / 60.0f * -100.0f;
            var currentMorale = Agent.GetMorale() - _lastMoraleChange;
            _lastMoraleChange = 0;
            if (moraleChange < 0.0f)
            {
                Main.LogInMessageLog("lost morale by " + moraleChange);
                _lastMoraleChange = moraleChange;
                Agent.SetMorale(currentMorale + moraleChange);
            }

            if (!_logic.AgentsWithBanners.ContainsKey(Agent.Team.TeamIndex))
                return;

            var teamsBannerBearer = _logic.AgentsWithBanners[Agent.Team.TeamIndex];

            var distance = (Agent.Position - teamsBannerBearer.Position).Length;
            if (distance < 5)
            {
                moraleChange += 20.0f * Falloff(5);
                _lastMoraleChange += moraleChange;
                Agent.SetMorale(currentMorale + moraleChange);
            }
        }

        private float Falloff(float distance)
        {
            return distance / 5;
        }
    }
}
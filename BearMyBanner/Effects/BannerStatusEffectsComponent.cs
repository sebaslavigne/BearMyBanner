using System.Linq;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Effects
{
    public class BannerStatusEffectsComponent : AgentComponent
    {
        private readonly IGameObjectEditor _editor;
        private readonly BannerStatusEffectsLogic _logic;

        public BannerStatusEffectsComponent(Agent agent, IGameObjectEditor editor) : base(agent)
        {
            _editor = editor;
            _logic = Agent.Mission.GetMissionBehaviour<BannerStatusEffectsLogic>();
        }

        protected override void OnTickAsAI(float dt)
        {
            base.OnTickAsAI(dt);

            if (_logic.AgentsWithBanners.IsEmpty() || !Agent.IsHuman || !Agent.IsAIControlled)
            {
                return;
            }

            var distance = (Agent.Position - _logic.AgentsWithBanners.First().Position).Length;
            if (distance < 20)
            {
                Agent.SetMorale(0);
            }
        }
    }
}
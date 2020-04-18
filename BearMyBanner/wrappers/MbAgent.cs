using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.wrappers
{
    public class MbAgent : IAgent
    {
        public MbAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
        }

        public Agent WrappedAgent { get; }
        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public CharacterObject Character => WrappedAgent.Character as CharacterObject;
        public PartyBase Party => ((PartyGroupAgentOrigin) WrappedAgent.Origin).Party;
    }
}
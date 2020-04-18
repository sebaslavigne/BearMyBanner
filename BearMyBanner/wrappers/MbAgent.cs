using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.wrappers
{
    public class MbAgent : IAgent
    {
        public MbAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
            Character = new MbCharacter((CharacterObject)WrappedAgent.Character);
        }

        public Agent WrappedAgent { get; }
        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public ICharacter Character { get; }
        public PartyBase Party => ((PartyGroupAgentOrigin) WrappedAgent.Origin).Party;
    }
}
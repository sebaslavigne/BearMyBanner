using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public class CampaignAgent : IBmbAgent
    {
        public CampaignAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
            Character = new CampaignCharacter((CharacterObject)WrappedAgent.Character);
            var partyToWrap = ((PartyGroupAgentOrigin) WrappedAgent.Origin)?.Party;
            if (partyToWrap != null)
                PartyName = partyToWrap.Name.ToString();
        }

        public Agent WrappedAgent { get; }
        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public IBmbCharacter Character { get; }
        public string PartyName { get; }
    }
}
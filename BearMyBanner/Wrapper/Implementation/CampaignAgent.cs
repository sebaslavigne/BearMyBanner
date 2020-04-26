using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public class CampaignAgent : IBMBAgent
    {
        public CampaignAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
            Character = new CampaignCharacter((CharacterObject)WrappedAgent.Character);
            var partyToWrap = ((PartyGroupAgentOrigin)WrappedAgent.Origin)?.Party;
            if (partyToWrap != null)
            {
                PartyName = partyToWrap.Name.ToString();
                PartyColor = partyToWrap.PrimaryColorPair.Item1;
                IsInPlayerParty = partyToWrap.Leader.IsPlayerCharacter;
            }
            Formation = wrappedAgent.Formation != null ? (FormationGroup)(int)wrappedAgent.Formation.FormationIndex : FormationGroup.Unset;
        }

        public Agent WrappedAgent { get; }
        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public IBMBCharacter Character { get; }
        public string PartyName { get; }
        public uint PartyColor { get; }
        public bool IsInPlayerParty { get; }
        public bool HasRangedWeapons => throw new System.NotImplementedException();
        public FormationGroup Formation { get; }
    }
}
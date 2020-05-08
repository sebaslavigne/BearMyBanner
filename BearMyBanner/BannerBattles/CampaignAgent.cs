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
            _wrappedParty = ((PartyGroupAgentOrigin)WrappedAgent.Origin)?.Party;
            if (_wrappedParty != null)
            {
                PartyName = _wrappedParty.Name.ToString();
                PartyColor = _wrappedParty.PrimaryColorPair.Item1;
            }
            Formation = wrappedAgent.Formation != null ? (FormationGroup)(int)wrappedAgent.Formation.FormationIndex : FormationGroup.Unset;
        }

        public Agent WrappedAgent { get; }

        private const string CaravanMasterPartialID = "caravan_master";

        private PartyBase _wrappedParty;
        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public IBMBCharacter Character { get; }
        public string PartyName { get; }
        public uint PartyColor { get; }
        public bool IsInPlayerParty => _wrappedParty.Leader != null ? _wrappedParty.Leader.IsPlayerCharacter : false;
        public bool HasRangedWeapons => Character.Type == TroopSpecialization.Archer || Character.Type == TroopSpecialization.Cavalry;
        public FormationGroup Formation { get; }
        public bool ServesUnderLord => _wrappedParty.Leader != null ? new CampaignCharacter(_wrappedParty.Leader).IsHero : false;

        public bool IsInCaravanParty => _wrappedParty.Leader != null ? _wrappedParty.Leader.StringId.Contains(CaravanMasterPartialID) : false;
        public bool IsCaravanPartyLeader => CheckIsCaravanPartyLeader();

        private bool CheckIsCaravanPartyLeader()
        {
            if (IsInCaravanParty)
            {
                return Character.Equals(new CampaignCharacter(_wrappedParty.Leader));
            }
            else
            {
                return false;
            }
        }
    }
}
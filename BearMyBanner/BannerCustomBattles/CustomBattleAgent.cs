using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public class CustomBattleAgent : IBMBAgent
    {

        public CustomBattleAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
            Character = new CustomBattleCharacter((BasicCharacterObject)WrappedAgent.Character);
            var partyToWrap = ((CustomBattleAgentOrigin)WrappedAgent.Origin);
            if (partyToWrap != null)
            {
                PartyName = partyToWrap.IsUnderPlayersCommand ? "Player party" : "Enemy party";
                PartyColor = WrappedAgent.Origin.FactionColor;
                IsInPlayerParty = partyToWrap.IsUnderPlayersCommand;
            }
        }

        public Agent WrappedAgent { get; }

        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public IBMBCharacter Character { get; }
        public string PartyName { get; }
        public bool IsInPlayerParty { get; }
        public bool HasRangedWeapons => Character.Type == TroopSpecialization.Archer || Character.Type == TroopSpecialization.Cavalry;
        public uint PartyColor { get; }
        public bool ServesUnderLord => true;
        public bool IsInCaravanParty => false;
        public bool IsCaravanPartyLeader => false;
        public FormationGroup Formation => GetFormationGroup();

        private FormationGroup GetFormationGroup()
        {
            switch (Character.Type)
            {
                case TroopSpecialization.Infantry:
                    return FormationGroup.Infantry;
                case TroopSpecialization.Archer:
                    return FormationGroup.Ranged;
                case TroopSpecialization.Cavalry:
                    return FormationGroup.Cavalry;
                case TroopSpecialization.HorseArcher:
                    return FormationGroup.HorseArcher;
                default:
                    return FormationGroup.Unset;
            }
        }
    }
}

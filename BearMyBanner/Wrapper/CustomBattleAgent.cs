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
                PartyName = partyToWrap.IsUnderPlayersCommand ? "Player" : "Enemy";
                IsInPlayerParty = partyToWrap.IsUnderPlayersCommand;
            }
        }

        public Agent WrappedAgent { get; }

        public bool IsAttacker => WrappedAgent.Team.IsAttacker;
        public bool IsDefender => WrappedAgent.Team.IsDefender;
        public IBMBCharacter Character { get; }
        public string PartyName { get; }
        public bool IsInPlayerParty { get; }
        public bool HasRangedWeapons => throw new System.NotImplementedException();
    }
}

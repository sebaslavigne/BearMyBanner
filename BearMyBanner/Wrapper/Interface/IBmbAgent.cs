namespace BearMyBanner.Wrapper
{
    public interface IBMBAgent
    {
        bool IsAttacker { get; }
        bool IsDefender { get; }
        IBMBCharacter Character { get; }
        string PartyName { get; }
        bool IsInPlayerParty { get; }
        bool HasRangedWeapons { get; }
        bool ServesUnderLord { get; }
        bool IsInCaravanParty { get; }
        bool IsCaravanPartyLeader { get; }
        FormationGroup Formation { get; }
    }
}
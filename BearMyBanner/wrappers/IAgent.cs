namespace BearMyBanner.Wrappers
{
    public interface IAgent
    {
        bool IsAttacker { get; }
        bool IsDefender { get; }
        ICharacter Character { get; }
        string PartyName { get; }
    }
}
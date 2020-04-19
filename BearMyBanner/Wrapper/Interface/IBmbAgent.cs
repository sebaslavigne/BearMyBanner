using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public interface IBMBAgent
    {
        Agent WrappedAgent { get; }
        bool IsAttacker { get; }
        bool IsDefender { get; }
        IBMBCharacter Character { get; }
        string PartyName { get; }
    }
}
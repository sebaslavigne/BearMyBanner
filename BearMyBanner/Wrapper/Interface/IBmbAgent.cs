using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public interface IBmbAgent
    {
        Agent WrappedAgent { get; }
        bool IsAttacker { get; }
        bool IsDefender { get; }
        IBmbCharacter Character { get; }
        string PartyName { get; }
    }
}
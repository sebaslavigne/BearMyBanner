using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.wrappers
{
    public interface IAgent
    {
        bool IsAttacker { get; }
        bool IsDefender { get; }
        CharacterObject Character { get; }
        PartyBase Party { get; }
    }
}
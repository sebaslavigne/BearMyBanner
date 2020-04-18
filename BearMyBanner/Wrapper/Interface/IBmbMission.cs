using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    interface IBmbMission
    {
        Mission WrappedMission { get; }
        MissionType MissionType { get; }
        bool IsFieldBattle { get; }
        bool IsSiege { get; }
        bool IsHideout { get; }
        bool IsCustomBattle { get; }
        bool IsTournament { get; }
        bool IsVisit { get; }
    }
}

using System.Linq;
using TaleWorlds.MountAndBlade;
using SandBox.Source.Missions;

namespace BearMyBanner.Wrapper
{
    public class TypedMission : IBmbMission
    {
        public TypedMission(Mission wrappedMission)
        {
            WrappedMission = wrappedMission;
            if (wrappedMission.IsFieldBattle)
            {
                MissionType = MissionType.FieldBattle;
            }
            else if (wrappedMission.MissionLogics.OfType<SiegeMissionController>().Any()) {
                MissionType = MissionType.Siege;
            }
            else if (wrappedMission.MissionLogics.OfType<HideoutMissionController>().Any())
            {
                MissionType = MissionType.Hideout;
            }
        }

        public Mission WrappedMission { get; }
        public MissionType MissionType { get; }
        public bool IsFieldBattle => WrappedMission.IsFieldBattle;
        public bool IsSiege => MissionType == MissionType.Siege;
        public bool IsHideout => MissionType == MissionType.Hideout;
        public bool IsCustomBattle => MissionType == MissionType.CustomBattle;
        public bool IsTournament => MissionType == MissionType.Tournament;
        public bool IsVisit => MissionType == MissionType.Visit;
    }
}

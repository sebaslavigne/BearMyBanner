using System.Linq;
using SandBox;
using SandBox.Source.Missions;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public static class MissionUtils
    {

        public static MissionType GetMissionType(this Mission mission)
        {
            if (mission.IsFieldBattle()) return MissionType.FieldBattle;
            if (mission.IsSiege()) return MissionType.Siege;
            if (mission.IsHideout()) return MissionType.Hideout;
            if (mission.IsTournament()) return MissionType.Tournament;
            if (mission.IsHideout()) return MissionType.Hideout;
            if (mission.IsVisit()) return MissionType.Visit;
            return MissionType.Unknown;
        }

        public static bool IsFieldBattle(this Mission mission)
        {
            return mission.IsFieldBattle;
        }

        public static bool IsSiege(this Mission mission)
        {
            return mission.MissionLogics.OfType<SiegeMissionController>().Any();
        }

        public static bool IsHideout(this Mission mission)
        {
            return mission.MissionLogics.OfType<HideoutMissionController>().Any();
        }

        public static bool IsTournament(this Mission mission)
        {
            return mission.MissionLogics.OfType<TournamentFightMissionController>().Any();
        }

        public static bool IsCustomBattle(this Mission mission)
        {
            //TODO untested
            return mission.MissionLogics.OfType<CustomBattleAgentLogic>().Any();
        }

        public static bool IsVisit(this Mission mission)
        {
            //TODO for town and village visits
            return false;
        }

        public static TournamentFightMissionController GetNativeTournamentController(this Mission mission)
        {
            return mission.MissionLogics.OfType<TournamentFightMissionController>().First();
        }
    }
}

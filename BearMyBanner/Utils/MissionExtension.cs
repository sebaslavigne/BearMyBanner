using System.Linq;
using SandBox;
using SandBox.Source.Missions;
using TaleWorlds.Core;
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
            if (mission.IsTownVisit()) return MissionType.TownVisit;
            if (mission.IsVillageVisit()) return MissionType.VillageVisit;
            if (mission.IsCustomBattle()) return MissionType.CustomBattle;
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

        public static bool IsTownVisit(this Mission mission)
        {
            return mission.MissionLogics.OfType<TownCenterMissionController>().Any();
        }
        public static bool IsVillageVisit(this Mission mission)
        {
            return mission.MissionLogics.OfType<VillageMissionController>().Any();
        }

        public static TournamentFightMissionController GetNativeTournamentController(this Mission mission)
        {
            return mission.MissionLogics.OfType<TournamentFightMissionController>().First();
        }

        public static Culture GetTournamentCulture(this Mission mission)
        {
            if (mission.SceneName.Contains("aserai")) return Culture.Aserai;
            if (mission.SceneName.Contains("battania")) return Culture.Battania;
            if (mission.SceneName.Contains("empire")) return Culture.Empire;
            if (mission.SceneName.Contains("khuzait")) return Culture.Khuzait;
            if (mission.SceneName.Contains("sturgia")) return Culture.Sturgia;
            if (mission.SceneName.Contains("vlandia")) return Culture.Vlandia;
            return Culture.Unknown;
        }
    }
}

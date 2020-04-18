using System.Linq;
using TaleWorlds.MountAndBlade;
using SandBox.Source.Missions;

namespace BearMyBanner
{
    public static class MissionUtils
    {

        public static bool IsSiege(this Mission mission)
        {
            return mission.MissionLogics.OfType<SiegeMissionController>().Any();
        }
        
        public static bool IsHideout(this Mission mission)
        {
            return mission.MissionLogics.OfType<HideoutMissionController>().Any();
        }

        public static bool IsCustomBattle(this Mission mission)
        {
            return mission.MissionLogics.OfType<CustomBattleAgentLogic>().Any();
        }
    }
}

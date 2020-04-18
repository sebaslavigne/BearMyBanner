using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using SandBox.Source.Missions;

namespace BearMyBanner
{
    public class MissionUtils
    {

        public static bool IsSiege(Mission mission)
        {
            return mission.MissionLogics.OfType<SiegeMissionController>().Any();
        }
        
        public static bool IsHideout(Mission mission)
        {
            return mission.MissionLogics.OfType<HideoutMissionController>().Any();
        }
    }
}

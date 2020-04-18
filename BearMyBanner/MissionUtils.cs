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
    }
}

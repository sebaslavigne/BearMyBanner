using BearMyBanner.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class TournamentBannerAssignBehaviour : MissionLogic
    {

        private readonly IBMBSettings _settings;

        public TournamentBannerAssignBehaviour(IBMBSettings settings)
        {
            _settings = settings;
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            if (agent.IsHuman)
            {
                agent.Origin.SetBanner(agent.Team.Banner);
                agent.AddBannerToSpawnEquipment(new HashSet<ItemObject.ItemTypeEnum>());
            }
        }

        public override void OnAddTeam(Team team)
        {
            base.OnAddTeam(team);
            Main.PrintInMessageLog("Team " + team.TeamIndex, team.Color);
        }
    }
}

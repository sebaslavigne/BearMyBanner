using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    class TournamentTeam : IBMBTeam
    {
        public TournamentTeam(Team team)
        {
            TeamColor = (TeamColor)team.TeamIndex;
        }

        public TeamColor TeamColor { get; set; }
        public string BannerKey { get; set; }
        public int Participants { get; set; }
        public int MountedParticipants { get; set; }
    }
}

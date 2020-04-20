using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Wrapper
{
    class TournamentTeam : IBMBTeam
    {
        public TournamentTeam(int teamIndex)
        {
            Color = (Color)teamIndex;
        }

        public Color Color { get;set; }
        public string BannerKey { get; set; }
        public int participants { get; set; }
        public int mountedParticipants { get; set; }
    }
}

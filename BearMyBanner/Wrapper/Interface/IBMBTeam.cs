using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Wrapper
{
    interface IBMBTeam
    {
        TeamColor TeamColor { get; }
        IBMBBanner Banner { get; set; }
        int Participants { get; set; }
        int ParticipantsWithBanner { get; set; }
    }
}

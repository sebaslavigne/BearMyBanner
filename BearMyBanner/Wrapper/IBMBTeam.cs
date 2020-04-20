using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Wrapper
{
    interface IBMBTeam
    {
        Color Color { get; set; }
        string BannerKey { get; set; }
        int participants { get; set; }
        int mountedParticipants { get; set; }
    }
}

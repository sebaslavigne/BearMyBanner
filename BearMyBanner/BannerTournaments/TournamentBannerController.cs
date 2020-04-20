using BearMyBanner.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner
{
    class TournamentBannerController
    {
        private readonly IBMBSettings _settings;
        private Dictionary<int, int> _teamMountedParticipants;

        public int CurrentTeam { get; set; }

        public TournamentBannerController(IBMBSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Keeps track of how many mounted participants have been built per team
        /// </summary>
        public void countMountedParticipant()
        {
            _teamMountedParticipants.TryGetValue(CurrentTeam, out var count);
            _teamMountedParticipants[CurrentTeam] = count + 1;
        }

        /// <summary>
        /// Only the first mounted participant of each team gets a banner
        /// </summary>
        /// <returns></returns>
        public bool participantGetsBanner()
        {
            return _teamMountedParticipants[CurrentTeam] == 1;
        }
    }
}

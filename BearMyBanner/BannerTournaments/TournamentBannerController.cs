using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
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

        private Dictionary<TeamColor, TournamentTeam> _teams = new Dictionary<TeamColor, TournamentTeam>();
        private Dictionary<TeamColor, string> _teamBanners = new Dictionary<TeamColor, string>();
        private TeamColor _currentTeam;

        private void GenerateBanners()
        {
            _teamBanners[TeamColor.Blue] = "3.119.104.1747.1536.768.768.1.0.0";
            _teamBanners[TeamColor.Red] = "23.118.111.1747.1536.768.768.1.1.0";
            _teamBanners[TeamColor.Green] = "12.149.120.1747.1536.768.768.1.0.0";
            _teamBanners[TeamColor.Yellow] = "13.131.149.1747.1536.768.768.1.0.0";
        }

        public TournamentBannerController(IBMBSettings settings)
        {
            _settings = settings;
            GenerateBanners();
        }

        public void RegisterTeam(TournamentTeam team)
        {
            _teams[team.TeamColor] = team;
            _currentTeam = team.TeamColor;
            team.BannerKey = _teamBanners[team.TeamColor];
        }

        /// <summary>
        /// Keeps track of how many mounted participants have been built per team
        /// </summary>
        public void CountMountedParticipant()
        {
            _teams[_currentTeam].MountedParticipants++;
        }

        /// <summary>
        /// Only the first mounted participant of each team gets a banner
        /// </summary>
        /// <returns></returns>
        public bool ParticipantGetsBanner()
        {
            return _teams[_currentTeam].MountedParticipants == 1;
        }
    }

}

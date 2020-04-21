using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System.Collections.Generic;

namespace BearMyBanner
{
    class TournamentBannerController
    {
        private readonly IBMBSettings _settings;

        private Dictionary<TeamColor, TournamentTeam> _teams = new Dictionary<TeamColor, TournamentTeam>();
        private Dictionary<TeamColor, string> _teamBanners = new Dictionary<TeamColor, string>();
        private TeamColor _currentTeam;

        public TournamentBannerController(IBMBSettings settings)
        {
            _settings = settings;
            GenerateBanners();
        }

        private void GenerateBanners()
        {
            _teamBanners[TeamColor.Blue] = "3.119.104.1747.1536.768.768.1.0.0";
            _teamBanners[TeamColor.Red] = "23.118.111.1747.1536.768.768.1.1.0";
            _teamBanners[TeamColor.Green] = "12.149.120.1747.1536.768.768.1.0.0";
            _teamBanners[TeamColor.Yellow] = "13.131.149.1747.1536.768.768.1.0.0";
        }

        /// <summary>
        /// Keeps track of which team the behaviour will be adding agents to.
        /// Assigns a banner to the team.
        /// </summary>
        /// <param name="team"></param>
        public void RegisterTeam(TournamentTeam team)
        {
            _teams[team.TeamColor] = team;
            _currentTeam = team.TeamColor;
            team.BannerKey = _teamBanners[team.TeamColor];
        }

        /// <summary>
        /// Gives
        /// For now, only the first mounted participant of each team gets a banner
        /// </summary>
        /// <returns>true if the agent should get a banner</returns>
        public bool ParticipantGetsBanner(TournamentAgent agent)
        {
            if (_teams[_currentTeam].ParticipantsWithBanner == 0 && agent.HasMount && !agent.HasRangedWeapons)
            {
                _teams[_currentTeam].ParticipantsWithBanner = 1;
                return true;
            }
            return false;
        }

    }
}
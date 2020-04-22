using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System;
using System.Collections.Generic;

namespace BearMyBanner
{
    class TournamentBannerController
    {
        private readonly IBMBSettings _settings;

        private Dictionary<TeamColor, TournamentTeam> _teams = new Dictionary<TeamColor, TournamentTeam>();
        private Dictionary<TeamColor, TournamentBanner> _teamBanners = new Dictionary<TeamColor, TournamentBanner>();
        private TeamColor _currentTeam;
        

        public TournamentBannerController(IBMBSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Generates the banners for this tournament
        /// </summary>
        public void GenerateBanners(Culture culture)
        {
            TournamentThemes themes = new TournamentThemes(culture);
            Random random = new Random();

            //Select theme
            int themeIndex = random.Next(themes.AvailableThemes.Count);
            var theme = themes.AvailableThemes[themeIndex];

            //Select white or black secondary
            int secondary = random.Next(2) == 0 ? themes.SecondaryColors.white : themes.SecondaryColors.black;
            bool swapColors = false;
            if (theme.swappable) swapColors = random.Next(2) == 0;
            //For plain banners
            if (themeIndex == 0) secondary = themes.PrimaryColors[0];

            //Shuffle meshes
            for (int i = 0; i < theme.meshes.Length; i++)
            {
                int swapTo = random.Next(theme.meshes.Length);
                int aux = theme.meshes[i];
                theme.meshes[i] = theme.meshes[swapTo];
                theme.meshes[swapTo] = aux;
            }

            //Generate banners
            for (int i = 0; i < theme.meshes.Length; i++)
            {
                TournamentBanner banner = new TournamentBanner();
                banner.Mesh = theme.meshes[i];
                banner.HasIcon = theme.isIcon;
                if (theme.mirrorable) banner.Mirrored = random.Next(2) == 0;
                if (!swapColors)
                {
                    banner.PrimaryColor = themes.PrimaryColors[i];
                    banner.SecondaryColor = secondary;
                } else
                {
                    banner.PrimaryColor = secondary;
                    banner.SecondaryColor = themes.PrimaryColors[i];
                }
                banner.GenerateKey();
                _teamBanners[(TeamColor)i] = banner;
            }
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
            team.Banner = _teamBanners[team.TeamColor];
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
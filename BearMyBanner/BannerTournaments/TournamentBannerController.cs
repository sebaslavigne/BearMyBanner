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
        private readonly (bool mirrorable, bool isIcon, bool swappable, int[] meshes)[] _themes =
            {
                (true, false, false, new int[] { 11, 11, 11, 11 }), // plain
                (true, false, true, new int[] { 1, 3, 12, 14 }), // two tone A
                (true, false, true, new int[] { 12, 13, 14, 16 }), // two tone B
                (true, false, false, new int[] { 23, 24, 27, 28 }), // lines
                (true, false, true, new int[] { 31, 32, 33, 34 }), // base shapes
                (true, true, true, new int[] { 512, 513, 514, 221 }), // poker
                (true, true, false, new int[] { 202, 205, 206, 207 }), // small flora
                (true, true, true, new int[] { 217, 218, 219, 220 }), // big flora
                (false, true, true, new int[] { 425, 426, 427, 428 }), // moons
                (false, true, true, new int[] { 523, 525, 526, 534 }), // runes A
                (false, true, false, new int[] { 530, 531, 532, 533 }), // runes B
                (false, true, false, new int[] { 423, 423, 423, 423 }) // imperial
            };
        private readonly int[] _primaryColors = new int[] { 119, 118, 120, 121};
        private readonly (int white, int black) _secondaryColors = (128, 149);

        public TournamentBannerController(IBMBSettings settings)
        {
            _settings = settings;
            GenerateBanners();
        }

        /// <summary>
        /// Generates the banners for this tournament
        /// </summary>
        private void GenerateBanners()
        {
            Random random = new Random();
            //Select theme
            int themeIndex = random.Next(_themes.Length);
            var theme = _themes[themeIndex];

            //Select white or black secondary
            int secondary = random.Next(2) == 0 ? _secondaryColors.white : _secondaryColors.black;
            bool swapColors = false;
            if (theme.swappable) swapColors = random.Next(2) == 0;
            //For plain banners
            if (themeIndex == 0) secondary = _primaryColors[0];

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
                    banner.PrimaryColor = _primaryColors[i];
                    banner.SecondaryColor = secondary;
                } else
                {
                    banner.PrimaryColor = secondary;
                    banner.SecondaryColor = _primaryColors[i];
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
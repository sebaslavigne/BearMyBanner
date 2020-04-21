using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
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
        private readonly TournamentBannerController _controller;

        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        public TournamentBannerAssignBehaviour(IBMBSettings settings)
        {
            _controller = new TournamentBannerController(settings);
            _settings = settings;
            _forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Shield
            };
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                var tournamentAgent = new TournamentAgent(agent);
                //Luckily, mounted agents are built with their mount already assigned
                if (_controller.ParticipantGetsBanner(tournamentAgent))
                {
                    agent.Origin.SetBanner(agent.Team.Banner);
                    agent.AddBannerToSpawnEquipment(new HashSet<ItemObject.ItemTypeEnum>());
                    agent.AddComponent(new DropBannerComponent(agent));
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        public override void AfterAddTeam(Team team)
        {
            base.AfterAddTeam(team);
            try
            {
                TournamentTeam tournamentTeam = new TournamentTeam(team);
                _controller.RegisterTeam(tournamentTeam);
                team.Banner.ChangeBanner(tournamentTeam.Banner);
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

    }
}

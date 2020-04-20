using BearMyBanner.Settings;
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
                //Luckily, mounted agents are built with their mount already assigned
                if (agent.IsHuman && agent.MountAgent != null)
                {


                    agent.Origin.SetBanner(agent.Team.Banner);
                    agent.AddBannerToSpawnEquipment(new HashSet<ItemObject.ItemTypeEnum>());
                    agent.AddComponent(new DropBannerComponent(agent));
                    //TODO
                    //Use a controller for horses, one on ones, random chance...
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
                _controller.CurrentTeam = team.TeamIndex;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

    }
}

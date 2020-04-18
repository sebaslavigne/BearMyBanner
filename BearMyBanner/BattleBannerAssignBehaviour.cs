using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BattleBannerAssignBehaviour : MissionLogic
    {
        private readonly BannerAssignmentController _bannerAssignmentController;

        public BattleBannerAssignBehaviour()
        {
            _bannerAssignmentController = new BannerAssignmentController();
        }

        public override void OnCreated()
        {
            base.OnCreated();

            try
            {
                _bannerAssignmentController.FilterAllowedBearerTypes(Mission);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                _bannerAssignmentController.ProcessAgentOnBuild(agent, banner, this.Mission);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);//Use LINQ for team parties
            try
            {
                _bannerAssignmentController.DisplayBannersEquippedMessage();
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }
    }
}

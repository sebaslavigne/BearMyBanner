using System;
using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BattleBannerAssignBehaviour : MissionLogic
    {
        private readonly BannerAssignmentController _bannerAssignmentController;

        public BattleBannerAssignBehaviour(IBMBSettings settings)
        {
            // For battles, we don't want ranged units dropping banners because they had a bow
            var forbiddenWeapons = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };
            _bannerAssignmentController = new BannerAssignmentController(settings, forbiddenWeapons);
        }

        public override void OnCreated()
        {
            base.OnCreated();

            try
            {
                _bannerAssignmentController.FilterAllowedBearerTypes(this.Mission.IsHideout());
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
                _bannerAssignmentController.ProcessBuiltAgent(new CampaignAgent(agent), Mission);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);//TODO Use LINQ for team parties
            try
            {
                _bannerAssignmentController.ShowBannersEquippedByPartiesInTeam(team);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }
    }
}

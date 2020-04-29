using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class VisitBannerBehaviour : MissionLogic
    {

        private bool _bannerGiven = false;

        public override void OnPreMissionTick(float dt)
        {
            base.OnPreMissionTick(dt);
            try
            {
                if (_bannerGiven || this.Mission.PlayerTeam == null) return;
                Agent playerAgent = null;
                Agent companionAgent = null;
                foreach (Agent agent in this.Mission.PlayerTeam.ActiveAgents)
                {
                    if (agent.Character.IsPlayerCharacter)
                    {
                        playerAgent = agent;
                    }
                    else
                    {
                        companionAgent = agent;
                    }
                }
                companionAgent?.EquipBanner();
                _bannerGiven = true;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }


}

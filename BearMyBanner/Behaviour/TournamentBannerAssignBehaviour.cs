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
        private readonly GameObjectEditor _editor;

        public TournamentBannerAssignBehaviour(IBMBSettings settings)
        {
            _settings = settings;
            _editor = new GameObjectEditor(_settings);
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            if (agent.IsHuman)
            {
                //Test
                _editor.AddBannerToAgentSpawnEquipment(agent, new HashSet<ItemObject.ItemTypeEnum>());
            }
        }
        public override void AfterAddTeam(Team team)
        {
            base.AfterAddTeam(team);
        }

        public override void OnAddTeam(Team team)
        {
            base.OnAddTeam(team);
            //Maybe change banners here
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);
        }

        public override void AfterStart()
        {
            base.AfterStart();
        }

    }
}

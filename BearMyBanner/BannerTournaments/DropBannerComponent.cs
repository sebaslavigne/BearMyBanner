using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class DropBannerComponent : AgentComponent
    {
        public DropBannerComponent(Agent agent) : base(agent)
        {
        }

        protected override void OnDismount(Agent mount)
        {
            base.OnDismount(mount);
            if (mount.Health <= 0f)
            {
                this.Agent.DropBanner();
            }
        }

    }
}

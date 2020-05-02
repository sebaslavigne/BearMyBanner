using BearMyBanner.Settings;
using System;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class DropBannerComponent : AgentComponent
    {
        private readonly DropBannerController _controller;
        private readonly IBMBSettings _settings;

        private bool _dropsOnLowHealth;
        private bool _dropsOnRetreat;

        public DropBannerComponent(Agent agent, IBMBSettings settings, DropBannerController controller) : base(agent)
        {
            try
            {
                _settings = settings;
                _controller = controller;
                _dropsOnLowHealth = _controller.DropsOnLowHealth();
                _dropsOnRetreat = _controller.DropsOnRetreat(agent.Character.GetPower());
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        protected override void OnDismount(Agent mount)
        {
            try
            {
                if (mount.Health <= 0f)
                {
                    this.Agent.DropBanner();
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        protected override void OnHit(Agent affectorAgent, int damage, int weaponKind, float perkEffectOnMorale)
        {
            try
            {
                if (_dropsOnLowHealth && Agent.Health < _settings.DropHealthThreshold)
                {
                    Agent.DropBanner();
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        protected override void OnRetreating()
        {
            try
            {
                if (_dropsOnRetreat)
                {
                    Agent.DropBanner();
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

    }
}

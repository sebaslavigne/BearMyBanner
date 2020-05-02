using BearMyBanner.Settings;
using System;

namespace BearMyBanner
{
    public class DropBannerController
    {
        private readonly Random _rd = new Random();
        private readonly IBMBSettings _settings;

        public DropBannerController(IBMBSettings settings)
        {
            _settings = settings;
        }

        public bool DropsOnLowHealth()
        {
            if (!_settings.DropOnLowHealth) return false;

            return true;
        }

        public bool DropsOnRetreat(float power)
        {
            if (!_settings.DropOnRetreat) return false;

            float chance = _settings.DropRetreatChance / 100f;
            if (_settings.DropWeightedRetreat) chance = chance * 2 / power - 0.5f;

            return (float)_rd.NextDouble() < chance;
        }

    }
}

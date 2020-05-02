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
            float chance = 0f;
            switch (_settings.DropRetreatMode)
            {
                case DropRetreatMode.Disabled:
                    return false;
                case DropRetreatMode.Fixed:
                    chance = _settings.DropRetreatChance;
                    break;
                case DropRetreatMode.Weighted:
                    chance = _settings.DropRetreatChance * 2 / power - 0.5f;
                    break;
                default:
                    break;
            }

            return (float)_rd.NextDouble() < chance;
        }

    }
}

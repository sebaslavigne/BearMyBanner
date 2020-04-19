using BearMyBanner.Settings;
using System.Collections.Generic;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class GameObjectEditor : IGameObjectEditor
    {
        public const string CampaignBannerID = "campaign_banner_small";

        private readonly IBMBSettings _settings;
        private readonly HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        public GameObjectEditor(IBMBSettings settings, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            _settings = settings;
            _forbiddenWeapons = forbiddenWeapons;
        }

        public void AddBannerToAgentSpawnEquipment(IBMBAgent agent)
        {
            var mbAgent = ((CampaignAgent)agent).WrappedAgent;

            EquipmentElement weaponElement0 = mbAgent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon0);
            EquipmentElement weaponElement1 = mbAgent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon1);
            EquipmentElement weaponElement2 = mbAgent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon2);
            EquipmentElement weaponElement3 = mbAgent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon3);
            //Clones the equipment without weapons. Apparently arrows are not a weapon, but it doesn't matter
            Equipment clonedEquipment = mbAgent.SpawnEquipment.Clone(true);

            if (weaponElement0.Item != null && !_forbiddenWeapons.Contains(weaponElement0.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon0, weaponElement0);
            if (weaponElement1.Item != null && !_forbiddenWeapons.Contains(weaponElement1.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, weaponElement1);
            if (weaponElement2.Item != null && !_forbiddenWeapons.Contains(weaponElement2.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, weaponElement2);
            if (weaponElement3.Item != null && !_forbiddenWeapons.Contains(weaponElement3.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, weaponElement3);

            var bannerElement = GetBannerElement();
            clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon4, bannerElement);

            mbAgent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }

        private static EquipmentElement GetBannerElement()
        {
            EquipmentElement bannerElement =
                new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID));
            return bannerElement;
        }

        public bool CheckIfAgentHasBanner(IBMBAgent agent)
        {
            var mbAgent = ((CampaignAgent) agent).WrappedAgent;

            if (!mbAgent.IsHuman)
            {
                return false;
            }
            var bannerElement = GetBannerElement();
            var offhandWeapon = mbAgent.WieldedOffhandWeapon;
            if (offhandWeapon.Equals(MissionWeapon.Invalid))
            {
                return false;
            }
            var hasBanner = offhandWeapon.PrimaryItem == bannerElement.Item;
            return hasBanner;
        }
    }
}

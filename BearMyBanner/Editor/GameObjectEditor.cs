using BearMyBanner.Settings;
using System.Collections.Generic;
using BearMyBanner.Wrapper;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public static class GameObjectEditor
    {
        public const string CampaignBannerID = "campaign_banner_small";

        /// <summary>
        /// Alters the equipment this an Agent will spawn with.
        /// Agents don't have anything equipped at this point, SpawnEquipment defines what the game will equip them with at spawn
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="forbiddenWeapons">A set of weapons the agent has removed from their equipment</param>
        public static void AddBannerToSpawnEquipment(this Agent agent, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            EquipmentElement weaponElement0 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon0);
            EquipmentElement weaponElement1 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon1);
            EquipmentElement weaponElement2 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon2);
            EquipmentElement weaponElement3 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon3);
            //Clones the equipment without weapons. Apparently arrows are not a weapon, but it doesn't matter
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(true);

            if (weaponElement0.Item != null && !forbiddenWeapons.Contains(weaponElement0.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon0, weaponElement0);
            if (weaponElement1.Item != null && !forbiddenWeapons.Contains(weaponElement1.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, weaponElement1);
            if (weaponElement2.Item != null && !forbiddenWeapons.Contains(weaponElement2.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, weaponElement2);
            if (weaponElement3.Item != null && !forbiddenWeapons.Contains(weaponElement3.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, weaponElement3);

            EquipmentElement bannerElement = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID));
            clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon4, bannerElement);

            agent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }
    }
}

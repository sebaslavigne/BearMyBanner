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
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(false);

            for (int i = 0; i < (int) EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (clonedEquipment[i].Item != null && forbiddenWeapons.Contains(clonedEquipment[i].Item.Type))
                {
                    clonedEquipment[i] = new EquipmentElement(null, null);
                }
            }

            EquipmentElement bannerElement = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID));
            clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, bannerElement);

            agent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }

        public static void DropBanner(this Agent agent)
        {
            MissionWeapon extraSlot = agent.Equipment[EquipmentIndex.ExtraWeaponSlot];
            if (!extraSlot.IsEmpty && extraSlot.CurrentUsageItem.Item.Type == ItemObject.ItemTypeEnum.Banner)
            {
                agent.DropItem(EquipmentIndex.ExtraWeaponSlot);
            }
        }

        public static void ChangeBanner(this Banner banner, string newBanner)
        {
            banner.Deserialize(newBanner);
        }
    }
}

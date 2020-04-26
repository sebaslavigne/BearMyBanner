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
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="forbiddenWeapons">A set of weapon types that get removed from the agent's spawn equipment</param>
        public static void RemoveFromSpawnEquipment(this Agent agent, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(false);

            for (int i = 0; i < (int) EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (clonedEquipment[i].Item != null && forbiddenWeapons.Contains(clonedEquipment[i].Item.Type))
                {
                    clonedEquipment[i] = new EquipmentElement(null, null);
                }
            }
        }

        /// <summary>
        /// Adds a banner to the extra item slot in the agent's spawn equipment
        /// Understand it as an instruction to equip a banner when the agent spawns
        /// </summary>
        /// <param name="agent"></param>
        public static void AddBannerToSpawnEquipment(this Agent agent)
        {
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(false);

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

        public static void RemoveFromEquipment(this Agent agent, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (!agent.Equipment[i].IsEmpty && forbiddenWeapons.Contains(agent.Equipment[i].PrimaryItem.Type))
                {
                    agent.RemoveEquippedWeapon((EquipmentIndex)i);
                }
            }
        }

        public static void EquipBanner(this Agent agent)
        {
            EquipBanner(agent, agent.Origin.Banner);
        }

        public static void EquipBanner(this Agent agent, string key)
        {
            EquipBanner(agent, new Banner(key));
        }

        public static void EquipBanner(this Agent agent, Banner banner)
        {
            MissionWeapon bannerWeapon = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID), banner);
            agent.EquipWeaponToExtraSlotAndWield(ref bannerWeapon);
        }

        public static void ChangeBanner(this Banner banner, IBMBBanner newBanner)
        {
            banner.Deserialize(newBanner.Key);
        }

        public static void ChangeBaseColors(this Banner banner, int colorId, int colorId2)
        {
            banner.BannerDataList[0].ColorId = colorId;
            banner.BannerDataList[0].ColorId2 = colorId2;
        }

        public static void ChangeIconColor(this Banner banner, int colorId)
        {
            for (int i = 1; i < banner.BannerDataList.Count; i++)
            {
                banner.BannerDataList[i].ColorId = colorId;
            }
        }

        public static void ChangeIconMesh(this Banner banner, int meshId)
        {
            for (int i = 1; i < banner.BannerDataList.Count; i++)
            {
                banner.BannerDataList[i].MeshId = meshId;
            }
        }
    }
}

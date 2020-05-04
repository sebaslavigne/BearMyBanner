using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace BearMyBanner
{
    public static class AgentExtension
    {
        public const string CampaignBannerID = "campaign_banner_small";
        public const string ThrowingStonesID = "throwing_stone";

        /// <summary>
        /// Alters the equipment this an Agent will spawn with.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="forbiddenWeapons">A set of weapon types that get removed from the agent's spawn equipment</param>
        public static void RemoveFromSpawnEquipment(this Agent agent, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(false);

            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
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

        public static bool DropBanner(this Agent agent)
        {
            MissionWeapon extraSlot = agent.Equipment[EquipmentIndex.ExtraWeaponSlot];
            if (!extraSlot.IsEmpty && extraSlot.CurrentUsageItem.Item.Type == ItemObject.ItemTypeEnum.Banner)
            {
                agent.DropItem(EquipmentIndex.ExtraWeaponSlot);
                return true;
            }
            return false;
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

        public static void SwitchShieldBanner(this Agent agent, Banner banner)
        {
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
            {
                MissionWeapon paintedShield = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID), banner);
                if (!agent.Equipment[i].IsEmpty && agent.Equipment[i].PrimaryItem.Type == ItemObject.ItemTypeEnum.Shield)
                {
                    string shieldId = agent.Equipment[i].PrimaryItem.StringId;
                    agent.RemoveEquippedWeapon((EquipmentIndex)i);
                    paintedShield = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>(shieldId), banner);
                    agent.EquipWeaponWithNewEntity((EquipmentIndex)i, ref paintedShield);
                }
            }
        }

        public static bool HasWeaponOfType(this Agent agent, ItemObject.ItemTypeEnum itemType)
        {
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (!agent.Equipment[i].IsEmpty && agent.Equipment[i].PrimaryItem.Type == itemType)
                {
                    return true;
                }
            }
            return false;
        }

        public static void UnequipAllAndEquipStones(this Agent agent)
        {
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots - 1; i++)
            {
                if (!agent.Equipment[i].IsEmpty)
                {
                    agent.RemoveEquippedWeapon((EquipmentIndex)i);
                }
            }

            MissionWeapon rockWeapon = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>(ThrowingStonesID), null);
            agent.EquipWeaponWithNewEntity(EquipmentIndex.Weapon0, ref rockWeapon);
            agent.WieldNextWeapon(Agent.HandIndex.MainHand);
        }


        public static bool HasRangedWeapon(this Equipment equipment)
        {
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (!equipment[i].IsEmpty
                    && (equipment[i].Item.Type == ItemObject.ItemTypeEnum.Bow
                    || equipment[i].Item.Type == ItemObject.ItemTypeEnum.Crossbow))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

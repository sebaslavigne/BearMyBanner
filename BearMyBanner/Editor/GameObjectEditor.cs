using BearMyBanner.Settings;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class GameObjectEditor
    {
        public const string CampaignBannerID = "campaign_banner_small";

        private readonly IBMBSettings _settings;

        public GameObjectEditor(IBMBSettings settings)
        {
            _settings = settings;
        }

        public void AddBannerToAgentSpawnEquipment(Agent agent)
        {
            EquipmentElement weaponElement0 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon0);
            EquipmentElement weaponElement1 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon1);
            EquipmentElement weaponElement2 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon2);
            EquipmentElement weaponElement3 = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.Weapon3);
            //Clones the equipment without weapons. Apparently arrows are not a weapon, but it doesn't matter
            Equipment clonedEquipment = agent.SpawnEquipment.Clone(true);

            HashSet<ItemObject.ItemTypeEnum> forbiddenItemTypes = new HashSet<ItemObject.ItemTypeEnum>()
            {
                ItemObject.ItemTypeEnum.Arrows,
                ItemObject.ItemTypeEnum.Bolts,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Crossbow
            };

            if (weaponElement0.Item != null && !forbiddenItemTypes.Contains(weaponElement0.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon0, weaponElement0);
            if (weaponElement1.Item != null && !forbiddenItemTypes.Contains(weaponElement1.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, weaponElement1);
            if (weaponElement2.Item != null && !forbiddenItemTypes.Contains(weaponElement2.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, weaponElement2);
            if (weaponElement3.Item != null && !forbiddenItemTypes.Contains(weaponElement3.Item.Type)) clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, weaponElement3);

            EquipmentElement bannerElement = new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>(CampaignBannerID));
            clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon4, bannerElement);

            agent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BattleBannerAssignBehaviour : MissionLogic
    {

        private List<CharacterObject> allowedBearerTypes;

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);
            try
            {
                if (allowedBearerTypes == null || allowedBearerTypes.IsEmpty<CharacterObject>())
                {
                    FilterAllowedBearerTypes();
                }
                GiveBannersInTeam(team);
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        private void FilterAllowedBearerTypes()
        {
            List<CharacterObject> characterTypes = new List<CharacterObject>();
            MBObjectManager.Instance.GetAllInstancesOfObjectType<CharacterObject>(ref characterTypes);

            /* Add types to a list of allowed troops to carry a banner */
            allowedBearerTypes = new List<CharacterObject>();

            /* Add troops */
            if (BMBSettings.Instance.AllowSoldiers) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Soldier)); }
            if (BMBSettings.Instance.AllowCaravanGuards) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.CaravanGuard)); }
            if (BMBSettings.Instance.AllowMercenaries) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Mercenary)); }
            if (BMBSettings.Instance.AllowBandits) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit)); }

            /* Filter by formation */
            allowedBearerTypes = allowedBearerTypes
                .Where(t => (BMBSettings.Instance.AllowInfantry && !t.IsArcher && !t.IsMounted)
                    || (BMBSettings.Instance.AllowMounted && !t.IsArcher && t.IsMounted)
                    || (BMBSettings.Instance.AllowRanged && t.IsArcher && !t.IsMounted)
                    || (BMBSettings.Instance.AllowMountedRanged && t.IsArcher && t.IsMounted))
                .ToList();

            /* Filter by tier */
            if (BMBSettings.Instance.FilterTiers)
            {
                List<int> allowedTiers = new List<int>();
                if (BMBSettings.Instance.AllowTier1) allowedTiers.Add(1);
                if (BMBSettings.Instance.AllowTier2) allowedTiers.Add(2);
                if (BMBSettings.Instance.AllowTier3) allowedTiers.Add(3);
                if (BMBSettings.Instance.AllowTier4) allowedTiers.Add(4);
                if (BMBSettings.Instance.AllowTier5) allowedTiers.Add(5);
                if (BMBSettings.Instance.AllowTier6) allowedTiers.Add(6);
                if (BMBSettings.Instance.AllowTier7Plus) allowedTiers.AddRange(new List<int>() { 7, 8, 9, 10, 11, 12, 13, 14 }); //This'll do for now
                allowedBearerTypes = allowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (BMBSettings.Instance.AllowPlayer) { allowedBearerTypes.Add(characterTypes.Find(character => character.IsPlayerCharacter)); }
            if (BMBSettings.Instance.AllowCompanions) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.IsHero && character.Occupation == Occupation.Wanderer)); }
            if (BMBSettings.Instance.AllowNobles) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => !character.IsPlayerCharacter && (character.Occupation == Occupation.Lord || character.Occupation == Occupation.Lady))); }
        }

        private void GiveBannersInTeam(Team team)
        {
            Dictionary<CharacterObject, List<Agent>> teamTroopMap = team.TeamAgents
                .GroupBy(ta => (CharacterObject)ta.Character)
                .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            HashSet<CharacterObject> presentAllowedTypes = allowedBearerTypes.Where(type => teamTroopMap.ContainsKey(type)).ToHashSet();

            Dictionary<CharacterObject, List<Agent>> eligibleTroopMap = teamTroopMap
                .Where(kv => presentAllowedTypes.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            int bannersGiven = 0;
            foreach (KeyValuePair<CharacterObject, List<Agent>> entry in eligibleTroopMap)
            {

                if (entry.Key.IsHero || entry.Value.Count >= BMBSettings.Instance.MinTroopTypeAmount)
                {
                    int ratioCount = 0;
                    foreach (Agent agent in entry.Value)
                    {
                        if (ratioCount % BMBSettings.Instance.BearerToTroopRatio == 0)
                        {
                            if (((CharacterObject)agent.Character).IsArcher)
                            {
                                StripWeaponsFromArcher(agent);
                            }

                            MissionWeapon bannerWeapon = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>("campaign_banner_small"), agent.Origin.Banner);
                            agent.EquipWeaponToExtraSlotAndWield(ref bannerWeapon);
                            bannersGiven++;
                        }
                        ratioCount++;
                    }
                }
            }
            Main.LogInMessageLog(bannersGiven + " banners given to " + team.Leader.Name + "'s party");
        }

        private static void StripWeaponsFromArcher(Agent agent)
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

            if (weaponElement0.Item != null && !forbiddenItemTypes.Contains(weaponElement0.Item.Type))
            {
                clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon0, weaponElement0);
            }
            if (weaponElement1.Item != null && !forbiddenItemTypes.Contains(weaponElement1.Item.Type))
            {
                clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, weaponElement1);
            }
            if (weaponElement2.Item != null && !forbiddenItemTypes.Contains(weaponElement2.Item.Type))
            {
                clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, weaponElement2);
            }
            if (weaponElement3.Item != null && !forbiddenItemTypes.Contains(weaponElement3.Item.Type))
            {
                clonedEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, weaponElement3);
            }

            agent.ClearEquipment();
            agent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }

    }
}

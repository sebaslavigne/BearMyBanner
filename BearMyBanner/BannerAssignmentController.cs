using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BannerAssignmentController
    {
        private List<CharacterObject> AllowedBearerTypes;
        private Dictionary<PartyBase, int> EquippedBannersByParty;
        private bool FirstSpawnInitialized = false;

        private enum TroopSpecialization
        {
            Infantry,
            Archer,
            Cavalry,
            HorseArcher
        }

        public void ProcessAgentOnBuild(Agent agent, Banner banner, Mission mission)
        {
            if (AllowedBearerTypes.Contains((CharacterObject) agent.Character))
            {
                if (mission.IsFieldBattle)
                {
                    ProcessAgent(agent, banner);
                }
                else if (BMBSettings.Instance.AllowSieges && mission.IsSiege())
                {
                    if ((BMBSettings.Instance.SiegeAttackersUseBanners && agent.Team.IsAttacker)
                        || (BMBSettings.Instance.SiegeDefendersUseBanners && agent.Team.IsDefender))
                    {
                        ProcessAgent(agent, banner);
                    }
                }
                else if (BMBSettings.Instance.AllowHideouts && mission.IsHideout())
                {
                    if ((BMBSettings.Instance.HideoutAttackersUseBanners && agent.Team.IsAttacker)
                        || (BMBSettings.Instance.HideoutBanditsUseBanners && agent.Team.IsDefender))
                    {
                        ProcessAgent(agent, banner);
                    }
                }
            }
        }

        private void ProcessAgent(Agent agent, Banner banner)
        {
            PartyBase agentParty = ((PartyGroupAgentOrigin)agent.Origin).Party;
            CharacterObject agentCharacter = (CharacterObject)agent.Character;
            TroopSpecialization agentSpec = DetermineAgentSpec(agentCharacter);

            /* Add to maps */
            var processedTroopsByType = new Dictionary<PartyBase, Dictionary<CharacterObject, List<Agent>>>();
            if (!processedTroopsByType.ContainsKey(agentParty)) processedTroopsByType.Add(agentParty, new Dictionary<CharacterObject, List<Agent>>());
            if (!processedTroopsByType[agentParty].ContainsKey(agentCharacter)) processedTroopsByType[agentParty].Add(agentCharacter, new List<Agent>());

            var processedTroopsBySpec = new Dictionary<PartyBase, Dictionary<TroopSpecialization, List<Agent>>>();
            if (!processedTroopsBySpec.ContainsKey(agentParty)) processedTroopsBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<Agent>>());
            if (!processedTroopsBySpec[agentParty].ContainsKey(agentSpec)) processedTroopsBySpec[agentParty].Add(agentSpec, new List<Agent>());

            processedTroopsByType[agentParty][agentCharacter].Add(agent);
            processedTroopsBySpec[agentParty][agentSpec].Add(agent);

            /* Give banner or skip */
            int processedTroops = BMBSettings.Instance.UseTroopSpecs ? processedTroopsBySpec[agentParty][agentSpec].Count : processedTroopsByType[agentParty][agentCharacter].Count;

            if (agentCharacter.IsHero || processedTroops % BMBSettings.Instance.BearerToTroopRatio == 0)
            {
                EquipAgentWithBanner(agent, banner);
                EquippedBannersByParty.TryGetValue(agentParty, out var count);
                EquippedBannersByParty[agentParty] = count + 1;
            }
        }

        private void EquipAgentWithBanner(Agent agent, Banner banner)
        {
            if (((CharacterObject)agent.Character).IsArcher)
            {
                StripWeaponsFromArcher(agent);
            }

            MissionWeapon bannerWeapon = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>("campaign_banner_small"), agent.Origin.Banner);
            agent.EquipWeaponToExtraSlotAndWield(ref bannerWeapon);
        }

        private TroopSpecialization DetermineAgentSpec(CharacterObject character)
        {
            if (!character.IsArcher && !character.IsMounted) return TroopSpecialization.Infantry;
            if (character.IsArcher && !character.IsMounted) return TroopSpecialization.Archer;
            if (!character.IsArcher && character.IsMounted) return TroopSpecialization.Cavalry;
            return TroopSpecialization.HorseArcher;
        }

        public void DisplayBannersEquippedMessage()
        {
            if (!FirstSpawnInitialized)
            {
                FirstSpawnInitialized = true;
                foreach (KeyValuePair<PartyBase, int> entry in EquippedBannersByParty)
                {
                    Main.LogInMessageLog(entry.Key.Name + " received " + entry.Value + " banners");
                }
            }
        }

        public void FilterAllowedBearerTypes(Mission mission)
        {
            EquippedBannersByParty = new Dictionary<PartyBase, int>();
            List<CharacterObject> characterTypes = new List<CharacterObject>();
            MBObjectManager.Instance.GetAllInstancesOfObjectType<CharacterObject>(ref characterTypes);

            /* Add types to a list of allowed troops to carry a banner */
            AllowedBearerTypes = new List<CharacterObject>();

            /* Add troops */
            if (BMBSettings.Instance.AllowSoldiers) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Soldier)); }
            if (BMBSettings.Instance.AllowCaravanGuards) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.CaravanGuard)); }
            if (BMBSettings.Instance.AllowMercenaries) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Mercenary)); }
            if (BMBSettings.Instance.AllowBandits) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit)); }

            /* Filter by formation */
            AllowedBearerTypes = AllowedBearerTypes
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
                AllowedBearerTypes = AllowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (BMBSettings.Instance.AllowPlayer) { AllowedBearerTypes.Add(characterTypes.Find(character => character.IsPlayerCharacter)); }
            if (BMBSettings.Instance.AllowCompanions) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.IsHero && character.Occupation == Occupation.Wanderer)); }
            if (BMBSettings.Instance.AllowNobles) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => !character.IsPlayerCharacter && (character.Occupation == Occupation.Lord || character.Occupation == Occupation.Lady))); }

            /* Add bandits for hideout missions */
            if (BMBSettings.Instance.AllowHideouts && BMBSettings.Instance.HideoutBanditsUseBanners && MissionUtils.IsHideout(mission))
            {
                AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit));
            }
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

            agent.ClearEquipment();//Maybe this is not needed
            agent.UpdateSpawnEquipmentAndRefreshVisuals(clonedEquipment);
        }
    }
}
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

        private List<CharacterObject> AllowedBearerTypes;

        private enum TroopSpecialization
        {
            Infantry,
            Archer,
            Cavalry,
            HorseArcher
        }

        private Dictionary<PartyBase, Dictionary<CharacterObject, List<Agent>>> ProcessedTroopsByType;
        private Dictionary<PartyBase, Dictionary<TroopSpecialization, List<Agent>>> ProcessedTroopsBySpec;
        private Dictionary<PartyBase, int> EquippedBannersByParty;

        private bool FirstSpawnInitialized = false;

        public const string CampaignBannerID = "campaign_banner_small";

        public override void OnCreated()
        {
            base.OnCreated();

            try
            {
                FilterAllowedBearerTypes();
                ProcessedTroopsByType = new Dictionary<PartyBase, Dictionary<CharacterObject, List<Agent>>>();
                ProcessedTroopsBySpec = new Dictionary<PartyBase, Dictionary<TroopSpecialization, List<Agent>>>();
                EquippedBannersByParty = new Dictionary<PartyBase, int>();
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);
            try
            {
                if (AllowedBearerTypes.Contains((CharacterObject)agent.Character))
                {
                    if (this.Mission.IsFieldBattle)
                    {
                        ProcessAgent(agent, banner);
                    }
                    else if (BMBSettings.Instance.AllowSieges && MissionUtils.IsSiege(this.Mission))
                    {
                        if ((BMBSettings.Instance.SiegeAttackersUseBanners && agent.Team.IsAttacker)
                            || (BMBSettings.Instance.SiegeDefendersUseBanners && agent.Team.IsDefender))
                        {
                            ProcessAgent(agent, banner);
                        }
                    }
                    else if (BMBSettings.Instance.AllowHideouts && MissionUtils.IsHideout(this.Mission))
                    {
                        if ((BMBSettings.Instance.HideoutAttackersUseBanners && agent.Team.IsAttacker)
                            || (BMBSettings.Instance.HideoutBanditsUseBanners && agent.Team.IsDefender))
                        {
                            ProcessAgent(agent, banner);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.LogInMessageLog("BMB Error: " + ex.Message);
            }
        }

        private void ProcessAgent(Agent agent, Banner banner)
        {
            PartyBase agentParty = ((PartyGroupAgentOrigin)agent.Origin).Party;
            CharacterObject agentCharacter = (CharacterObject)agent.Character;
            TroopSpecialization agentSpec = DetermineAgentSpec(agentCharacter);

            /* Add to maps */
            if (!ProcessedTroopsByType.ContainsKey(agentParty)) ProcessedTroopsByType.Add(agentParty, new Dictionary<CharacterObject, List<Agent>>());
            if (!ProcessedTroopsByType[agentParty].ContainsKey(agentCharacter)) ProcessedTroopsByType[agentParty].Add(agentCharacter, new List<Agent>());

            if (!ProcessedTroopsBySpec.ContainsKey(agentParty)) ProcessedTroopsBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<Agent>>());
            if (!ProcessedTroopsBySpec[agentParty].ContainsKey(agentSpec)) ProcessedTroopsBySpec[agentParty].Add(agentSpec, new List<Agent>());

            ProcessedTroopsByType[agentParty][agentCharacter].Add(agent);
            ProcessedTroopsBySpec[agentParty][agentSpec].Add(agent);

            /* Give banner or skip */
            int processedTroops = BMBSettings.Instance.UseTroopSpecs ? ProcessedTroopsBySpec[agentParty][agentSpec].Count : ProcessedTroopsByType[agentParty][agentCharacter].Count;

            if (agentCharacter.IsHero || processedTroops % BMBSettings.Instance.BearerToTroopRatio == 0)
            {
                AddBannerToSpawnEquipment(agent);
                EquippedBannersByParty.TryGetValue(agentParty, out var count);
                EquippedBannersByParty[agentParty] = count + 1;
            }
        }

        private TroopSpecialization DetermineAgentSpec(CharacterObject character)
        {
            if (!character.IsArcher && !character.IsMounted) return TroopSpecialization.Infantry;
            if (character.IsArcher && !character.IsMounted) return TroopSpecialization.Archer;
            if (!character.IsArcher && character.IsMounted) return TroopSpecialization.Cavalry;
            return TroopSpecialization.HorseArcher;
        }

        public override void OnFormationUnitsSpawned(Team team)
        {
            base.OnFormationUnitsSpawned(team);//Use LINQ for team parties
            try
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
            if (BMBSettings.Instance.AllowHideouts && BMBSettings.Instance.HideoutBanditsUseBanners && MissionUtils.IsHideout(this.Mission))
            {
                AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit));
            }
        }

        private static void AddBannerToSpawnEquipment(Agent agent)
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

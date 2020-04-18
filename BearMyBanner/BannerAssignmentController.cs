using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class BannerAssignmentController
    {
        private readonly IBMBSettings _settings;
        private List<CharacterObject> AllowedBearerTypes;
        private Dictionary<PartyBase, int> EquippedBannersByParty;
        private bool FirstSpawnInitialized = false;
        private Dictionary<PartyBase, Dictionary<CharacterObject, List<Agent>>> _processedTroopsByType;
        private Dictionary<PartyBase, Dictionary<TroopSpecialization, List<Agent>>> _processedTroopsBySpec;

        private enum TroopSpecialization
        {
            Infantry,
            Archer,
            Cavalry,
            HorseArcher
        }

        private List<Agent> _agentsThatShouldReceiveBanners;
        public IEnumerable<Agent> AgentsThatShouldReceiveBanners => _agentsThatShouldReceiveBanners;

        public BannerAssignmentController(IBMBSettings settings)
        {
            _settings = settings;
        }

        public void ProcessAgentOnBuild(Agent agent, Banner banner, Mission mission)
        {
            if (AllowedBearerTypes.Contains((CharacterObject) agent.Character))
            {
                if (mission.IsFieldBattle)
                {
                    ProcessAgent(agent, banner);
                }
                else if (_settings.AllowSieges && mission.IsSiege())
                {
                    if ((_settings.SiegeAttackersUseBanners && agent.Team.IsAttacker)
                        || (_settings.SiegeDefendersUseBanners && agent.Team.IsDefender))
                    {
                        ProcessAgent(agent, banner);
                    }
                }
                else if (_settings.AllowHideouts && mission.IsHideout())
                {
                    if ((_settings.HideoutAttackersUseBanners && agent.Team.IsAttacker)
                        || (_settings.HideoutBanditsUseBanners && agent.Team.IsDefender))
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
            if (!_processedTroopsByType.ContainsKey(agentParty)) _processedTroopsByType.Add(agentParty, new Dictionary<CharacterObject, List<Agent>>());
            if (!_processedTroopsByType[agentParty].ContainsKey(agentCharacter)) _processedTroopsByType[agentParty].Add(agentCharacter, new List<Agent>());

            if (!_processedTroopsBySpec.ContainsKey(agentParty)) _processedTroopsBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<Agent>>());
            if (!_processedTroopsBySpec[agentParty].ContainsKey(agentSpec)) _processedTroopsBySpec[agentParty].Add(agentSpec, new List<Agent>());

            _processedTroopsByType[agentParty][agentCharacter].Add(agent);
            _processedTroopsBySpec[agentParty][agentSpec].Add(agent);

            /* Give banner or skip */
            int processedTroops = _settings.UseTroopSpecs ? _processedTroopsBySpec[agentParty][agentSpec].Count : _processedTroopsByType[agentParty][agentCharacter].Count;

            if (agentCharacter.IsHero || processedTroops % _settings.BearerToTroopRatio == 0)
            {
                _agentsThatShouldReceiveBanners.Add(agent);
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

        public void FilterAllowedBearerTypes(bool isHideout)
        {
            _agentsThatShouldReceiveBanners = new List<Agent>();
            _processedTroopsByType = new Dictionary<PartyBase, Dictionary<CharacterObject, List<Agent>>>();
            _processedTroopsBySpec = new Dictionary<PartyBase, Dictionary<TroopSpecialization, List<Agent>>>();
            EquippedBannersByParty = new Dictionary<PartyBase, int>();
            List<CharacterObject> characterTypes = new List<CharacterObject>();
            MBObjectManager.Instance.GetAllInstancesOfObjectType(ref characterTypes);

            /* Add types to a list of allowed troops to carry a banner */
            AllowedBearerTypes = new List<CharacterObject>();

            /* Add troops */
            if (_settings.AllowSoldiers) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Soldier)); }
            if (_settings.AllowCaravanGuards) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.CaravanGuard)); }
            if (_settings.AllowMercenaries) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Mercenary)); }
            if (_settings.AllowBandits) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit)); }

            /* Filter by formation */
            AllowedBearerTypes = AllowedBearerTypes
                .Where(t => (_settings.AllowInfantry && !t.IsArcher && !t.IsMounted)
                            || (_settings.AllowMounted && !t.IsArcher && t.IsMounted)
                            || (_settings.AllowRanged && t.IsArcher && !t.IsMounted)
                            || (_settings.AllowMountedRanged && t.IsArcher && t.IsMounted))
                .ToList();

            /* Filter by tier */
            if (_settings.FilterTiers)
            {
                List<int> allowedTiers = new List<int>();
                if (_settings.AllowTier1) allowedTiers.Add(1);
                if (_settings.AllowTier2) allowedTiers.Add(2);
                if (_settings.AllowTier3) allowedTiers.Add(3);
                if (_settings.AllowTier4) allowedTiers.Add(4);
                if (_settings.AllowTier5) allowedTiers.Add(5);
                if (_settings.AllowTier6) allowedTiers.Add(6);
                if (_settings.AllowTier7Plus) allowedTiers.AddRange(new List<int>() { 7, 8, 9, 10, 11, 12, 13, 14 }); //This'll do for now
                AllowedBearerTypes = AllowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (_settings.AllowPlayer) { AllowedBearerTypes.Add(characterTypes.Find(character => character.IsPlayerCharacter)); }
            if (_settings.AllowCompanions) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.IsHero && character.Occupation == Occupation.Wanderer)); }
            if (_settings.AllowNobles) { AllowedBearerTypes.AddRange(characterTypes.FindAll(character => !character.IsPlayerCharacter && (character.Occupation == Occupation.Lord || character.Occupation == Occupation.Lady))); }

            /* Add bandits for hideout missions */
            if (_settings.AllowHideouts && _settings.HideoutBanditsUseBanners && isHideout)
            {
                AllowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit));
            }
        }
    }
}
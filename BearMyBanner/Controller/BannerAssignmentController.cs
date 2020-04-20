using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;

namespace BearMyBanner
{
    public class BannerAssignmentController
    {
        private readonly IBMBSettings _settings;
        private readonly IGameObjectEditor _gameObjectEditor;

        public List<IBMBCharacter> AllowedBearerTypes { get; set; }
        private Dictionary<string, int> _equippedBannersByParty;
        private Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>> _processedTroopsByType;
        private Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>> _processedTroopsBySpec;

        public BannerAssignmentController(IBMBSettings settings, IGameObjectEditor gameObjectEditor)
        {
            _settings = settings;
            _gameObjectEditor = gameObjectEditor;
        }

        /// <summary>
        /// Keeps track of agents in dictionaries and decides if they get banners
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>true if the agent should receive a banner</returns>
        public bool ProcessAgent(IBMBAgent agent)
        {
            string agentParty = agent.PartyName;
            IBMBCharacter agentCharacter = agent.Character;
            TroopSpecialization agentSpec = agent.Character.Type;

            /* Add to maps */
            if (!_processedTroopsByType.ContainsKey(agentParty)) _processedTroopsByType.Add(agentParty, new Dictionary<IBMBCharacter, List<IBMBAgent>>());
            if (!_processedTroopsByType[agentParty].ContainsKey(agentCharacter)) _processedTroopsByType[agentParty].Add(agentCharacter, new List<IBMBAgent>());

            if (!_processedTroopsBySpec.ContainsKey(agentParty)) _processedTroopsBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<IBMBAgent>>());
            if (!_processedTroopsBySpec[agentParty].ContainsKey(agentSpec)) _processedTroopsBySpec[agentParty].Add(agentSpec, new List<IBMBAgent>());

            _processedTroopsByType[agentParty][agentCharacter].Add(agent);
            _processedTroopsBySpec[agentParty][agentSpec].Add(agent);

            /* Give banner or skip */
            int processedTroops = _settings.UseTroopSpecs ? _processedTroopsBySpec[agentParty][agentSpec].Count : _processedTroopsByType[agentParty][agentCharacter].Count;

            if (agentCharacter.IsHero || processedTroops % _settings.BearerToTroopRatio == 0)
            {
                _equippedBannersByParty.TryGetValue(agentParty, out var equippedCount);
                _equippedBannersByParty[agentParty] = equippedCount + 1;
                return true;
            }
            return false;
        }
      
        /// <summary>
        /// Shows a message with each party banner count in the parties color
        /// </summary>
        /// <param name="team"></param>
        public void PrintBannersEquippedByPartiesInTeam(Dictionary<string, uint> partiesInTeam)
        {
            foreach (KeyValuePair<string, uint> entry in partiesInTeam)
            {
                if (_equippedBannersByParty.TryGetValue(entry.Key, out var count))
                {
                    Main.LogInMessageLog(count + " banners given to " + entry.Key, entry.Value);
                }
            }
        }

        /// <summary>
        /// Generates a list of allowed troop types according to settings
        /// </summary>
        /// <param name="characterTypes"></param>
        /// <param name="isHideout"></param>
        public void FilterAllowedBearerTypes(IReadOnlyList<IBMBCharacter> characterTypes, bool isHideout)
        {
            _processedTroopsByType = new Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>>();
            _processedTroopsBySpec = new Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>>();
            _equippedBannersByParty = new Dictionary<string, int>();

            /* Add types to a list of allowed troops to carry a banner */
            AllowedBearerTypes = new List<IBMBCharacter>();

            /* Add troops */
            if (_settings.AllowSoldiers) { AllowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Soldier)); }
            if (_settings.AllowCaravanGuards) { AllowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.CaravanGuard)); }
            if (_settings.AllowMercenaries) { AllowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Mercenary)); }
            if (_settings.AllowBandits) { AllowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit)); }

            /* Filter by formation */
            AllowedBearerTypes = AllowedBearerTypes
                .Where(t => (_settings.AllowInfantry && t.Type == TroopSpecialization.Infantry)
                            || (_settings.AllowMounted && t.Type == TroopSpecialization.Cavalry)
                            || (_settings.AllowRanged && t.Type == TroopSpecialization.Archer)
                            || (_settings.AllowMountedRanged && t.Type == TroopSpecialization.HorseArcher))
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
            if (_settings.AllowPlayer) { AllowedBearerTypes.Add(characterTypes.First(character => character.IsPlayerCharacter)); }
            if (_settings.AllowCompanions) { AllowedBearerTypes.AddRange(characterTypes.Where(character => character.IsHero && character.Occupation == CharacterOccupation.Wanderer)); }
            if (_settings.AllowNobles) { AllowedBearerTypes.AddRange(characterTypes.Where(character => !character.IsPlayerCharacter && (character.Occupation == CharacterOccupation.Lord || character.Occupation == CharacterOccupation.Lady))); }

            /* Add bandits for hideout missions */
            if (_settings.AllowHideouts && _settings.HideoutBanditsUseBanners && isHideout)
            {
                AllowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit));
            }
        }
    }
}
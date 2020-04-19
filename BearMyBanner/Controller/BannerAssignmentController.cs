using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;

namespace BearMyBanner
{
    public class BannerAssignmentController
    {
        private readonly IBMBSettings _settings;
        private readonly GameObjectEditor _gameObjectEditor;

        private List<IBMBCharacter> _allowedBearerTypes;
        private Dictionary<string, int> _equippedBannersByParty;
        private Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>> _processedTroopsByType;
        private Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>> _processedTroopsBySpec;
        private HashSet<ItemObject.ItemTypeEnum> _forbiddenWeapons;

        public BannerAssignmentController(IBMBSettings settings, HashSet<ItemObject.ItemTypeEnum> forbiddenWeapons)
        {
            _settings = settings;
            _forbiddenWeapons = forbiddenWeapons;
            _gameObjectEditor = new GameObjectEditor(settings);
        }

        /// <summary>
        /// Checks if an agent is elligible for a banner and processes them if they are
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="mission"></param>
        public void ProcessBuiltAgent(IBMBAgent agent, Mission mission)
        {
            if (_allowedBearerTypes.Contains(agent.Character))
            {
                if (mission.IsFieldBattle)
                {
                    ProcessAgent(agent);
                }
                else if (_settings.AllowSieges && mission.IsSiege())
                {
                    if ((_settings.SiegeAttackersUseBanners && agent.IsAttacker)
                        || (_settings.SiegeDefendersUseBanners && agent.IsDefender))
                    {
                        ProcessAgent(agent);
                    }
                }
                else if (_settings.AllowHideouts && mission.IsHideout())
                {
                    if ((_settings.HideoutAttackersUseBanners && agent.IsAttacker)
                        || (_settings.HideoutBanditsUseBanners && agent.IsDefender))
                    {
                        ProcessAgent(agent);
                    }
                }
            }
        }

        /// <summary>
        /// Keeps track of agents in dictionaries and decides if they get banners
        /// </summary>
        /// <param name="agent"></param>
        private void ProcessAgent(IBMBAgent agent)
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
                _gameObjectEditor.AddBannerToAgentSpawnEquipment(agent.WrappedAgent, _forbiddenWeapons);
                _equippedBannersByParty.TryGetValue(agentParty, out var equippedCount);
                _equippedBannersByParty[agentParty] = equippedCount + 1;
            }
        }

        /// <summary>
        /// Shows a message with each party banner count in the parties color
        /// </summary>
        /// <param name="team"></param>
        public void ShowBannersEquippedByPartiesInTeam(Team team)
        {
            Dictionary<string, uint> partiesInTeam = team.TeamAgents
                .Select(ta => new CampaignAgent(ta))
                .DistinctBy(ca => ca.PartyName)
                .ToDictionary(ca => ca.PartyName, ca => ca.PartyColor);
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
        /// <param name="isHideout"></param>
        public void FilterAllowedBearerTypes(bool isHideout)
        {
            var nativeCharacterTypes = new List<CharacterObject>();
            MBObjectManager.Instance.GetAllInstancesOfObjectType(ref nativeCharacterTypes);
            var characterTypes = nativeCharacterTypes.Select(t => new CampaignCharacter(t)).ToList();

            _processedTroopsByType = new Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>>();
            _processedTroopsBySpec = new Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>>();
            _equippedBannersByParty = new Dictionary<string, int>();

            /* Add types to a list of allowed troops to carry a banner */
            _allowedBearerTypes = new List<IBMBCharacter>();

            /* Add troops */
            if (_settings.AllowSoldiers) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Soldier)); }
            if (_settings.AllowCaravanGuards) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.CaravanGuard)); }
            if (_settings.AllowMercenaries) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Mercenary)); }
            if (_settings.AllowBandits) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit)); }

            /* Filter by formation */
            _allowedBearerTypes = _allowedBearerTypes
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
                _allowedBearerTypes = _allowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (_settings.AllowPlayer) { _allowedBearerTypes.Add(characterTypes.First(character => character.IsPlayerCharacter)); }
            if (_settings.AllowCompanions) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.IsHero && character.Occupation == CharacterOccupation.Wanderer)); }
            if (_settings.AllowNobles) { _allowedBearerTypes.AddRange(characterTypes.Where(character => !character.IsPlayerCharacter && (character.Occupation == CharacterOccupation.Lord || character.Occupation == CharacterOccupation.Lady))); }

            /* Add bandits for hideout missions */
            if (_settings.AllowHideouts && _settings.HideoutBanditsUseBanners && isHideout)
            {
                _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit));
            }
        }
    }
}
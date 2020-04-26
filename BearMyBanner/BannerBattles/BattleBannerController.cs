using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;
using System;

namespace BearMyBanner
{
    public class BattleBannerController
    {
        private readonly IBMBSettings _settings;
        private readonly IBMBFormationBanners _formationBanners;

        private List<IBMBCharacter> _allowedBearerTypes { get; set; }
        private Dictionary<string, int> _equippedBannersByParty;
        private Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>> _processedTroopsByType;
        private Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>> _processedTroopsBySpec;

        public BattleBannerController(IBMBSettings settings, IBMBFormationBanners formationBanners)
        {
            _settings = settings;
            _formationBanners = formationBanners;
        }

        /// <summary>
        /// Decides if an agent qualifies for a banner based on settings and mission type
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="missionType"></param>
        /// <returns></returns>
        public bool AgentIsEligible(IBMBAgent agent, MissionType missionType)
        {
            if (_allowedBearerTypes.Contains(agent.Character))
            {
                if (missionType == MissionType.FieldBattle)
                {
                    return true;
                }
                else if (_settings.AllowSieges && missionType == MissionType.Siege)
                {
                    return ((_settings.SiegeAttackersUseBanners && agent.IsAttacker)
                        || (_settings.SiegeDefendersUseBanners && agent.IsDefender));
                }
                else if (_settings.AllowHideouts && missionType == MissionType.Hideout)
                {
                    return ((_settings.HideoutAttackersUseBanners && agent.IsAttacker)
                        || (_settings.HideoutBanditsUseBanners && agent.IsDefender));
                }
            }
            return false;
        }

        /// <summary>
        /// Keeps track of agents in dictionaries and decides if they get banners
        /// </summary>
        /// <param name="agent"></param>
        /// <returns>true if the agent should receive a banner</returns>
        public bool AgentGetsBanner(IBMBAgent agent)
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

        public bool AgentGetsFancyBanner(IBMBAgent agent)
        {
            if (!_formationBanners.EnableFormationBanners || !agent.IsInPlayerParty) return false;
            if (agent.Character.IsPlayerCharacter) return false;
            if (_settings.AllowCompanions && _formationBanners.CompanionsUseFormationBanners && agent.Character.Occupation == CharacterOccupation.Wanderer) return true;
            if (agent.Character.IsHero) return false;
            return true;
        }

        public bool AgentGetsFancyShield(IBMBAgent agent)
        {
            return (_formationBanners.UseInShields && AgentGetsFancyBanner(agent));
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
                    Main.PrintInMessageLog(count + " banners given to " + entry.Key, entry.Value);
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
                List<int> allowedTiers = _settings.AllowedTiers
                    .Split(',')
                    .Select(Int32.Parse)
                    .ToList();

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
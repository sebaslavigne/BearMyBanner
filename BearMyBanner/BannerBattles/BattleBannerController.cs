using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using BearMyBanner.Settings;
using System;
using TaleWorlds.MountAndBlade;
using System.Text;

namespace BearMyBanner
{
    public class BattleBannerController
    {
        private readonly IBMBSettings _settings;
        private readonly IBMBFormationBanners _formationBanners;

        private List<IBMBCharacter> _allowedBearerTypes { get; set; }
        private Dictionary<string, int> _equippedBannersByParty;
        private Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>> _processedBySpec;
        private Dictionary<string, Dictionary<FormationGroup, List<IBMBAgent>>> _processedByFormation;
        private Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>> _processedByTroop;
        private HashSet<FormationGroup> _allowedFormations;

        private MissionType _missionType;

        public BattleBannerController(IBMBSettings settings, IBMBFormationBanners formationBanners, MissionType missionType)
        {
            _settings = settings;
            _formationBanners = formationBanners;
            _missionType = missionType;

            _processedBySpec = new Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>>();
            _processedByFormation = new Dictionary<string, Dictionary<FormationGroup, List<IBMBAgent>>>();
            _processedByTroop = new Dictionary<string, Dictionary<IBMBCharacter, List<IBMBAgent>>>();
            _equippedBannersByParty = new Dictionary<string, int>();
            PopulateAllowedFormations();
        }

        /// <summary>
        /// Decides if an agent qualifies for a banner based on settings and mission type
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public bool AgentIsEligible(IBMBAgent agent)
        {
            if (_allowedBearerTypes.Contains(agent.Character) && _allowedFormations.Contains(agent.Formation))
            {
                if (_settings.AllowBandits == BanditAssignMode.RecruitedOnly)
                {
                    if (agent.Character.Occupation == CharacterOccupation.Bandit && !agent.ServesUnderLord)
                    {
                        return false;
                    }
                }

                if (_settings.AllowCaravanGuards == CaravanAssignMode.OnlyMasters && agent.IsInCaravanParty)
                {
                    if (agent.IsCaravanPartyLeader) return true;
                }

                if (_missionType == MissionType.FieldBattle || _missionType == MissionType.CustomBattle)
                {
                    return true;
                }
                else if (_settings.AllowSieges && _missionType == MissionType.Siege)
                {
                    return ((_settings.SiegeAttackersUseBanners && agent.IsAttacker)
                        || (_settings.SiegeDefendersUseBanners && agent.IsDefender));
                }
                else if (_settings.AllowHideouts && _missionType == MissionType.Hideout)
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
            FormationGroup agentFormation = agent.Formation;

            /* Caravan masters bypass count if they lead caravans */
            if (_settings.AllowCaravanGuards == CaravanAssignMode.OnlyMasters && agent.IsInCaravanParty)
            {
                if (agent.IsCaravanPartyLeader)
                {
                    CountBannerGivenToParty(agentParty);
                    return true;
                }
                return false;//Other caravan guards in the caravan party don't get banner
            }

            /* Add to maps */
            if (!_processedByTroop.ContainsKey(agentParty)) _processedByTroop.Add(agentParty, new Dictionary<IBMBCharacter, List<IBMBAgent>>());
            if (!_processedByTroop[agentParty].ContainsKey(agentCharacter)) _processedByTroop[agentParty].Add(agentCharacter, new List<IBMBAgent>());

            if (!_processedByFormation.ContainsKey(agentParty)) _processedByFormation.Add(agentParty, new Dictionary<FormationGroup, List<IBMBAgent>>());
            if (!_processedByFormation[agentParty].ContainsKey(agentFormation)) _processedByFormation[agentParty].Add(agentFormation, new List<IBMBAgent>());

            if (!_processedBySpec.ContainsKey(agentParty)) _processedBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<IBMBAgent>>());
            if (!_processedBySpec[agentParty].ContainsKey(agentSpec)) _processedBySpec[agentParty].Add(agentSpec, new List<IBMBAgent>());

            _processedBySpec[agentParty][agentSpec].Add(agent);
            _processedByFormation[agentParty][agentFormation].Add(agent);
            _processedByTroop[agentParty][agentCharacter].Add(agent);

            /* Give banner or skip */
            //int processedTroops = _settings.UnitCountMode == UnitCountMode.Type ? _processedByType[agentParty][agentSpec].Count : _processedByTroop[agentParty][agentCharacter].Count;
            int processedTroops = GetProcessedTroopsByMode(agentParty, agentSpec, agentFormation, agentCharacter);

            if (agentCharacter.IsHero || processedTroops % _settings.BearerToTroopRatio == 0)
            {
                CountBannerGivenToParty(agentParty);
                return true;
            }
            return false;
        }

        private int GetProcessedTroopsByMode(string agentParty, TroopSpecialization agentSpec, FormationGroup agentFormation, IBMBCharacter agentCharacter)
        {
            switch (_settings.UnitCountMode)
            {
                case UnitCountMode.Spec:
                default:
                    return _processedBySpec[agentParty][agentSpec].Count;
                case UnitCountMode.Formation:
                    return _processedByFormation[agentParty][agentFormation].Count;
                case UnitCountMode.Troop:
                    return _processedByTroop[agentParty][agentCharacter].Count;
            }
        }

        private void CountBannerGivenToParty(string agentParty)
        {
            _equippedBannersByParty.TryGetValue(agentParty, out var equippedCount);
            _equippedBannersByParty[agentParty] = equippedCount + 1;
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

        public void DebugController()
        {
            // Character \t isn't recognized. StringBuilder.AppendLine() shows unrecognized characters in-game
            StringBuilder sb = new StringBuilder();

            Main.PrintInMessageLog("=== BMB Debug ===", 4282569842U);

            sb.Append("Current settings:");
            sb.Append("\n    Banner ratio: " + _settings.BearerToTroopRatio);
            Main.PrintInMessageLog(sb.ToString());
            sb.Clear();

            Main.PrintInMessageLog("    Grouping by: " + _settings.UnitCountMode, 4282569842U);

            string allowedTiers = _settings.FilterTiers ? _settings.AllowedTiers : "All tiers";
            sb.Append("    Allowed tiers: " + allowedTiers);
            sb.Append("\nGrouping results (Total group count -> banners given)");
            sb.Append("\nFinal amounts may differ slightly in special cases");
            Main.PrintInMessageLog(sb.ToString());
            sb.Clear();

            sb.Append("Grouped by spec:");
            foreach (var party in _processedBySpec)
            {
                sb.Append("\n    " + party.Key);
                foreach (var group in party.Value)
                {
                    sb.Append("\n      " + group.Key + ": " + group.Value.Count + " -> " + group.Value.Count / _settings.BearerToTroopRatio);
                }
            }
            if (_settings.UnitCountMode == UnitCountMode.Spec)
            {
                Main.PrintInMessageLog(sb.ToString(), 4282569842U);
            }
            else
            {
                Main.PrintInMessageLog(sb.ToString());
            }
            sb.Clear();

            sb.Append("\nGrouped by formation:");
            foreach (var party in _processedByFormation)
            {
                sb.Append("\n    " + party.Key);
                foreach (var group in party.Value)
                {
                    sb.Append("\n      " + group.Key + ": " + group.Value.Count + " -> " + group.Value.Count / _settings.BearerToTroopRatio);
                }
            }
            if (_settings.UnitCountMode == UnitCountMode.Formation)
            {
                Main.PrintInMessageLog(sb.ToString(), 4282569842U);
            }
            else
            {
                Main.PrintInMessageLog(sb.ToString());
            }
            sb.Clear();

            sb.Append("\nGrouped by troop type:");
            foreach (var party in _processedByTroop)
            {
                sb.Append("\n    " + party.Key);
                foreach (var group in party.Value)
                {
                    sb.Append("\n      " + group.Key.Name + ": " + group.Value.Count + " -> " + group.Value.Count / _settings.BearerToTroopRatio);
                }
            }
            if (_settings.UnitCountMode == UnitCountMode.Troop)
            {
                Main.PrintInMessageLog(sb.ToString(), 4282569842U);
            }
            else
            {
                Main.PrintInMessageLog(sb.ToString());
            }
            sb.Clear();

            Main.PrintInMessageLog("=== Finished BMB Debug ===", 4282569842U);
        }

        /// <summary>
        /// Generates a list of allowed troop types according to settings
        /// </summary>
        /// <param name="characterTypes"></param>
        public void FilterAllowedBearerTypes(IReadOnlyList<IBMBCharacter> characterTypes)
        {
            /* Add types to a list of allowed troops to carry a banner */
            _allowedBearerTypes = new List<IBMBCharacter>();

            /* Add troops */
            if (_settings.AllowSoldiers) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Soldier)); }
            if (_settings.AllowCaravanGuards != CaravanAssignMode.NotAllowed) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.CaravanGuard)); }
            if (_settings.AllowMercenaries) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Mercenary)); }
            if (_settings.AllowBandits != BanditAssignMode.NotAllowed) { _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit)); }

            /* Filter by formation */
            _allowedBearerTypes = _allowedBearerTypes
                .Where(t => (_settings.AllowTypeInfantry && t.Type == TroopSpecialization.Infantry)
                            || (_settings.AllowTypeMounted && t.Type == TroopSpecialization.Cavalry)
                            || (_settings.AllowTypeRanged && t.Type == TroopSpecialization.Archer)
                            || (_settings.AllowTypeMountedRanged && t.Type == TroopSpecialization.HorseArcher))
                .ToList();

            /* Filter by tier */
            if (_settings.FilterTiers && _missionType != MissionType.CustomBattle)
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
            if (_settings.AllowHideouts && _settings.HideoutBanditsUseBanners && _missionType == MissionType.Hideout)
            {
                _allowedBearerTypes.AddRange(characterTypes.Where(character => character.Occupation == CharacterOccupation.Bandit));
            }
        }

        private void PopulateAllowedFormations()
        {
            _allowedFormations = new HashSet<FormationGroup>();
            if (_settings.AllowFormationInfantry) _allowedFormations.Add(FormationGroup.Infantry);
            if (_settings.AllowFormationRanged) _allowedFormations.Add(FormationGroup.Ranged);
            if (_settings.AllowFormationCavalry) _allowedFormations.Add(FormationGroup.Cavalry);
            if (_settings.AllowFormationHorseArcher) _allowedFormations.Add(FormationGroup.HorseArcher);
            if (_settings.AllowFormationSkirmisher) _allowedFormations.Add(FormationGroup.Skirmisher);
            if (_settings.AllowFormationHeavyInfantry) _allowedFormations.Add(FormationGroup.HeavyInfantry);
            if (_settings.AllowFormationLightCavalry) _allowedFormations.Add(FormationGroup.LightCavalry);
            if (_settings.AllowFormationHeavyCavalry) _allowedFormations.Add(FormationGroup.HeavyCavalry);
            _allowedFormations.Add(FormationGroup.Unset);
        }
    }
}
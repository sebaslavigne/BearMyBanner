using BearMyBanner.Settings;
using BearMyBanner.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner
{
    public class CustomBattleBannerController
    {
        private readonly IBMBSettings _settings;

        private List<IBMBCharacter> _allowedBearerTypes { get; set; }

        private Dictionary<string, int> _equippedBannersByParty;
        private Dictionary<string, Dictionary<TroopSpecialization, List<IBMBAgent>>> _processedTroopsBySpec;

        public CustomBattleBannerController(IBMBSettings settings)
        {
            _settings = settings;
        }

        public bool AgentIsEligible(IBMBAgent agent)
        {
            return _allowedBearerTypes.Contains(agent.Character);
        }

        public bool AgentGetsBanner(IBMBAgent agent)
        {
            string agentParty = agent.PartyName;
            IBMBCharacter agentCharacter = agent.Character;
            TroopSpecialization agentSpec = agent.Character.Type;

            /* Add to maps */
            if (!_processedTroopsBySpec.ContainsKey(agentParty)) _processedTroopsBySpec.Add(agentParty, new Dictionary<TroopSpecialization, List<IBMBAgent>>());
            if (!_processedTroopsBySpec[agentParty].ContainsKey(agentSpec)) _processedTroopsBySpec[agentParty].Add(agentSpec, new List<IBMBAgent>());

            _processedTroopsBySpec[agentParty][agentSpec].Add(agent);

            /* Give banner or skip */

            return true;
        }


    }
}

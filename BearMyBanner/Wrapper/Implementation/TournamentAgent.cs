using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public class TournamentAgent : IBMBAgent
    {

        public TournamentAgent(Agent wrappedAgent)
        {

        }

        public bool IsAttacker => false;

        public bool IsDefender => false;

        public IBMBCharacter Character => throw new NotImplementedException();

        public string PartyName => "Team " + WrappedAgent.Team.TeamIndex;

        public Agent WrappedAgent { get; }
    }
}

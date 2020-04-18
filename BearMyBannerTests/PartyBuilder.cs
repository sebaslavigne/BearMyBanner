using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BearMyBanner.Wrappers;
using Moq;

namespace BearMyBannerTests
{
    public class PartyBuilder
    {
        private readonly string _name;
        private readonly List<IAgent> _agents;

        public PartyBuilder(string name)
        {
            _name = name;
            _agents = new List<IAgent>();
        }

        public PartyBuilder AddTroops(ICharacter character, int count)
        {
            _agents.AddRange(Enumerable.Repeat(AgentFor(character, _name), count));

            return this;
        }

        public IEnumerable<IAgent> Build()
        {
            return _agents;
        }

        private IAgent AgentFor(ICharacter character, string party = "testParty")
        {
            var mock = new Mock<IAgent>();
            mock.Setup(m => m.Character)
                .Returns(character);
            mock.Setup(m => m.PartyName)
                .Returns(party);

            return mock.Object;
        }
    }
}
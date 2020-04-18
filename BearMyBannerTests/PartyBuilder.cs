using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BearMyBanner.Wrapper;
using Moq;

namespace BearMyBannerTests
{
    public class PartyBuilder
    {
        private readonly string _name;
        private readonly bool _isAttacking;
        private readonly List<IBmbAgent> _agents;

        public PartyBuilder(string name, bool isAttacking = true)
        {
            _name = name;
            _agents = new List<IBmbAgent>();
            _isAttacking = isAttacking;
        }

        public PartyBuilder AddTroops(IBmbCharacter character, int count)
        {
            _agents.AddRange(Enumerable.Repeat(AgentFor(character, _name), count));

            return this;
        }

        public IEnumerable<IBmbAgent> Build()
        {
            return _agents;
        }

        private IBmbAgent AgentFor(IBmbCharacter character, string party = "testParty")
        {
            var mock = new Mock<IBmbAgent>();
            mock.Setup(m => m.Character)
                .Returns(character);
            mock.Setup(m => m.PartyName)
                .Returns(party);
            mock.Setup(m => m.IsDefender)
                .Returns(!_isAttacking);
            mock.Setup(m => m.IsAttacker)
                .Returns(_isAttacking);

            return mock.Object;
        }
    }
}
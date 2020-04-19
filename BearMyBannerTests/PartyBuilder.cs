using System.Collections.Generic;
using System.Linq;
using BearMyBanner.Wrapper;
using Moq;

namespace BearMyBannerTests
{
    public class PartyBuilder
    {
        private readonly string _name;
        private readonly bool _isAttacking;
        private readonly List<IBMBAgent> _agents;

        public PartyBuilder(string name, bool isAttacking = true)
        {
            _name = name;
            _agents = new List<IBMBAgent>();
            _isAttacking = isAttacking;
        }

        public PartyBuilder AddTroops(IBMBCharacter character, int count)
        {
            _agents.AddRange(Enumerable.Repeat(AgentFor(character, _name), count));

            return this;
        }

        public IEnumerable<IBMBAgent> Build()
        {
            return _agents;
        }

        private IBMBAgent AgentFor(IBMBCharacter character, string party = "testParty")
        {
            var mock = new Mock<IBMBAgent>();
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
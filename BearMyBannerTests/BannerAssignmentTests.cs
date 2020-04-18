using System.Collections.Generic;
using System.Linq;
using BearMyBanner;
using BearMyBanner.Wrappers;
using BearMyBanner.Settings;
using Moq;
using Xunit;

namespace BearMyBannerTests
{
    public class BannerAssignmentTests
    {
        private readonly BannerAssignmentController _sut;
        private IBMBSettings _settings;

        public BannerAssignmentTests()
        {
            SetupSettings();
            _sut = new BannerAssignmentController(_settings);
        }

        private void SetupSettings()
        {
            _settings = new TestSettings();
            _settings.SetDefaults();
        }

        private ICharacter GetBasicInfantry()
        {
            var mock = new Mock<ICharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Infantry);
            mock.Setup(m => m.Tier)
                .Returns(5);

            return mock.Object;
        }

        private ICharacter GetArcher()
        {
            var mock = new Mock<ICharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Archer);
            mock.Setup(m => m.Tier)
                .Returns(5);

            return mock.Object;
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

        [Fact]
        public void TestCreation()
        {
            Assert.NotNull(_sut);
        }

        [Fact]
        public void TestBasicBattleWithDefaultSettings()
        {
            var basicInfantry = GetBasicInfantry();
            var characters = Enumerable.Repeat(basicInfantry, 1).ToList();
            _sut.FilterAllowedBearerTypes(characters, false);

            foreach (var agent in Enumerable.Repeat(AgentFor(basicInfantry), 21))
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(3, _sut.AgentsThatShouldReceiveBanners.Count());
        }

        [Fact]
        public void TestMixedTroops()
        {
            var basicInfantry = GetBasicInfantry();
            var archer = GetArcher();
            _sut.FilterAllowedBearerTypes(new List<ICharacter>(new []{archer, basicInfantry}), false);

            var agents = Enumerable.Repeat(AgentFor(basicInfantry), 21)
                .Concat(Enumerable.Repeat(AgentFor(archer), 14));

            foreach (var agent in agents)
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(3, _sut.AgentsThatShouldReceiveBanners.Count());
        }

        [Fact]
        public void TestTwoParties()
        {
            var basicInfantry = GetBasicInfantry();
            var archer = GetArcher();
            _sut.FilterAllowedBearerTypes(new List<ICharacter>(new[] { archer, basicInfantry }), false);

            var agentsForFirstParty = Enumerable.Repeat(AgentFor(basicInfantry, "firstParty"), 21)
                .Concat(Enumerable.Repeat(AgentFor(archer, "firstParty"), 14));

            var agentsForSecondParty = Enumerable.Repeat(AgentFor(basicInfantry, "secondParty"), 14)
                .Concat(Enumerable.Repeat(AgentFor(archer, "secondParty"), 7));

            foreach (var agent in agentsForFirstParty.Concat(agentsForSecondParty))
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(5, _sut.AgentsThatShouldReceiveBanners.Count());
        }
    }
}
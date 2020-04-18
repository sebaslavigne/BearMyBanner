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

        [Fact]
        public void TestCreation()
        {
            Assert.NotNull(_sut);
        }

        [Fact]
        public void TestBasicBattleWithDefaultSettings()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            _sut.FilterAllowedBearerTypes(new [] {basicInfantry}, false);

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .Build();

            foreach (var agent in party)
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(3, _sut.AgentsThatShouldReceiveBanners.Count());
        }

        [Fact]
        public void TestMixedTroops()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<ICharacter>(new []{archer, basicInfantry}), false);

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            foreach (var agent in party)
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(3, _sut.AgentsThatShouldReceiveBanners.Count());
        }

        [Fact]
        public void TestTwoParties()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<ICharacter>(new[] { archer, basicInfantry }), false);

            var firstParty = new PartyBuilder("firstParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            var secondParty = new PartyBuilder("secondParty")
                .AddTroops(basicInfantry, 14)
                .AddTroops(archer, 14)
                .Build();

            foreach (var agent in firstParty.Concat(secondParty))
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(5, _sut.AgentsThatShouldReceiveBanners.Count());
        }

        [Fact]
        public void TestWithLowTierInfantry()
        {

        }
    }
}
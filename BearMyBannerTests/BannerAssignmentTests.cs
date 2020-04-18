using System.Collections.Generic;
using System.Linq;
using BearMyBanner;
using BearMyBanner.Wrapper;
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

        private IBmbMission GetMission(MissionType type)
        {
            var mock = new Mock<IBmbMission>();
            mock.Setup(m => m.MissionType)
                .Returns(type);

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
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            _sut.FilterAllowedBearerTypes(new [] {basicInfantry}, false);

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .Build();

            var bannersCount = party.Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.FieldBattle)));

            Assert.Equal(3, bannersCount);
        }

        [Fact]
        public void TestMixedTroops()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBmbCharacter>(new []{archer, basicInfantry}), false);

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            var bannersCount = party.Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.FieldBattle)));

            Assert.Equal(3, bannersCount);
        }

        [Fact]
        public void TestTwoParties()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBmbCharacter>(new[] { archer, basicInfantry }), false);

            var firstParty = new PartyBuilder("firstParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            var secondParty = new PartyBuilder("secondParty")
                .AddTroops(basicInfantry, 14)
                .AddTroops(archer, 14)
                .Build();

            var bannersCount = firstParty
                .Concat(secondParty)
                .Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.FieldBattle)));

            Assert.Equal(5, bannersCount);
        }

        [Fact]
        public void TestWithLowTierInfantry()
        {
            var lowTierInfantry = CharacterFactory.GetLowTierInfantry();
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBmbCharacter>(new [] {archer, lowTierInfantry, basicInfantry}), false);

            var party = new PartyBuilder("testParty")
                .AddTroops(lowTierInfantry, 50)
                .AddTroops(basicInfantry, 10)
                .AddTroops(archer, 20)
                .Build();

            var bannersCount = party.Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.FieldBattle)));

            Assert.Equal(1, bannersCount);
        }


        [Fact]
        public void TestThatIgnoringTroopCharactersWorks()
        {
            _settings.UseTroopSpecs = true;
            _settings.BearerToTroopRatio = 10;

            var basicInfantryType1 = CharacterFactory.GetBasicInfantry();
            var basicInfantryType2 = CharacterFactory.GetBasicInfantry();
            var basicInfantryType3 = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBmbCharacter>(new[]
            {
                archer, basicInfantryType1, basicInfantryType2, basicInfantryType3
            }), false);

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantryType1, 7)
                .AddTroops(basicInfantryType2, 8)
                .AddTroops(basicInfantryType3, 8)
                .AddTroops(archer, 20)
                .Build();

            var bannersCount = party.Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.FieldBattle)));

            Assert.Equal(2, bannersCount);
        }

        [Fact]
        public void TestThatSiegeDefendersDontGetBanners()
        {
            var basicInfantryType1 = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBmbCharacter>(new[]
            {
                archer, basicInfantryType1
            }), false);

            var party = new PartyBuilder("testParty", false)
                .AddTroops(basicInfantryType1, 30)
                .AddTroops(archer, 20)
                .Build();

            var bannersCount = party.Count(agent =>
                _sut.ProcessBuiltAgent(agent, GetMission(MissionType.Siege)));

            Assert.Equal(0, bannersCount);
        }
    }
}
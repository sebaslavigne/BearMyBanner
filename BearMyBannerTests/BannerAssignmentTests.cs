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
        private readonly BattleBannerController _sut;
        private readonly BattleBannerController _sutSiege;
        private IBMBSettings _settings;
        private IBMBFormationBanners _formationBanners;

        public BannerAssignmentTests()
        {
            SetupSettings();

            _sut = new BattleBannerController(_settings, _formationBanners, MissionType.FieldBattle);
            _sutSiege = new BattleBannerController(_settings, _formationBanners, MissionType.Siege);
        }

        private void SetupSettings()
        {
            _settings = new TestSettings();
            _settings.SetDefaultSettings();
            _formationBanners = new TestFormations();
            _formationBanners.SetDefaultFormationSettings();
        }

        private void AssertBannerAddedTimes(int bannersExpected, int bannersActual)
        {
            Assert.Equal(bannersExpected, bannersActual);
        }


        private int ProcessAgents(IEnumerable<IBMBAgent> party, MissionType missionType = MissionType.FieldBattle)
        {
            int bannersAdded = 0;
            foreach (var agent in party)
            {
                if (missionType == MissionType.Siege)
                {
                    if (_sutSiege.AgentIsEligible(agent) && _sut.AgentGetsBanner(agent)) bannersAdded++;
                }
                else
                { 
                    if (_sut.AgentIsEligible(agent) && _sut.AgentGetsBanner(agent)) bannersAdded++;
                }
            }
            return bannersAdded;
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
            _sut.FilterAllowedBearerTypes(new [] {basicInfantry});

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .Build();

            int bannersAdded = 0;
            foreach (var agent in party)
            {
                if (_sut.AgentGetsBanner(agent)) bannersAdded++;
            }

            AssertBannerAddedTimes(3, bannersAdded);
        }

        [Fact]
        public void TestMixedTroops()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBMBCharacter>(new []{archer, basicInfantry}));

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            int bannersAdded = ProcessAgents(party);

            AssertBannerAddedTimes(3, bannersAdded);
        }

        [Fact]
        public void TestTwoParties()
        {
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBMBCharacter>(new[] { archer, basicInfantry }));

            var firstParty = new PartyBuilder("firstParty")
                .AddTroops(basicInfantry, 21)
                .AddTroops(archer, 14)
                .Build();

            var secondParty = new PartyBuilder("secondParty")
                .AddTroops(basicInfantry, 14)
                .AddTroops(archer, 14)
                .Build();

            int bannersAdded = ProcessAgents(firstParty.Concat(secondParty));

            AssertBannerAddedTimes(5, bannersAdded);
        }

        [Fact]
        public void TestWithLowTierInfantry()
        {
            var lowTierInfantry = CharacterFactory.GetLowTierInfantry();
            var basicInfantry = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBMBCharacter>(new [] {archer, lowTierInfantry, basicInfantry}));

            var party = new PartyBuilder("testParty")
                .AddTroops(lowTierInfantry, 50)
                .AddTroops(basicInfantry, 10)
                .AddTroops(archer, 20)
                .Build();

            int bannersAdded = ProcessAgents(party);

            AssertBannerAddedTimes(1, bannersAdded);
        }


        [Fact]
        public void TestThatIgnoringTroopCharactersWorks()
        {
            _settings.UnitCountMode = UnitCountMode.Spec;
            _settings.BearerToTroopRatio = 10;

            var basicInfantryType1 = CharacterFactory.GetBasicInfantry();
            var basicInfantryType2 = CharacterFactory.GetBasicInfantry();
            var basicInfantryType3 = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sut.FilterAllowedBearerTypes(new List<IBMBCharacter>(new[]
            {
                archer, basicInfantryType1, basicInfantryType2, basicInfantryType3
            }));

            var party = new PartyBuilder("testParty")
                .AddTroops(basicInfantryType1, 7)
                .AddTroops(basicInfantryType2, 8)
                .AddTroops(basicInfantryType3, 8)
                .AddTroops(archer, 20)
                .Build();

            int bannersAdded = ProcessAgents(party);

            AssertBannerAddedTimes(2, bannersAdded);
        }

        [Fact]
        public void TestThatSiegeDefendersDontGetBanners()
        {
            var basicInfantryType1 = CharacterFactory.GetBasicInfantry();
            var archer = CharacterFactory.GetArcher();
            _sutSiege.FilterAllowedBearerTypes(new List<IBMBCharacter>(new[]
            {
                archer, basicInfantryType1
            }));

            var party = new PartyBuilder("testParty", false)
                .AddTroops(basicInfantryType1, 30)
                .AddTroops(archer, 20)
                .Build();
            int bannersAdded = ProcessAgents(party, MissionType.Siege);

            AssertBannerAddedTimes(0, bannersAdded);
        }
    }
}
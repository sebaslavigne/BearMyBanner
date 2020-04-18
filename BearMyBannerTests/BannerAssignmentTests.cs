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

        private IAgent InfantryAgent(ICharacter character)
        {
            var mock = new Mock<IAgent>();
            mock.Setup(m => m.Character)
                .Returns(character);
            mock.Setup(m => m.PartyName)
                .Returns("testParty");

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

            foreach (var agent in Enumerable.Repeat(InfantryAgent(basicInfantry), 21))
            {
                _sut.ProcessAgentOnBuild(agent, BattleType.FieldBattle);
            }

            Assert.Equal(3, _sut.AgentsThatShouldReceiveBanners.Count());
        }
    }
}
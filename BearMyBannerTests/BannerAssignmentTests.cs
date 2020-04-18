using BearMyBanner;
using Moq;
using Xunit;

namespace BearMyBannerTests
{
    public class BannerAssignmentTests
    {
        private readonly BannerAssignmentController _sut;
        private Mock<IBMBSettings> _settings;

        public BannerAssignmentTests()
        {
            SetupSettings();
            _sut = new BannerAssignmentController(_settings.Object);
        }

        private void SetupSettings()
        {
            _settings = new Mock<IBMBSettings>();
            _settings.Object.SetDefaults();
        }

        [Fact]
        public void TestCreation()
        {
            Assert.NotNull(_sut);
        }
    }
}
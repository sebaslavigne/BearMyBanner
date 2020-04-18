using BearMyBanner;
using Xunit;

namespace BearMyBannerTests
{
    public class BannerAssignmentTests
    {
        private readonly BannerAssignmentController _sut;

        public BannerAssignmentTests()
        {
            _sut = new BannerAssignmentController();
        }

        [Fact]
        public void TestCreation()
        {
            Assert.NotNull(_sut);
        }
    }
}
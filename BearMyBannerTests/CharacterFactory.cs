using BearMyBanner.Wrapper;
using Moq;

namespace BearMyBannerTests
{
    public class CharacterFactory
    {
        public static IBmbCharacter GetBasicInfantry()
        {
            var mock = new Mock<IBmbCharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Infantry);
            mock.Setup(m => m.Tier)
                .Returns(5);

            return mock.Object;
        }

        public static IBmbCharacter GetLowTierInfantry()
        {
            var mock = new Mock<IBmbCharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Infantry);
            mock.Setup(m => m.Tier)
                .Returns(2);

            return mock.Object;
        }

        public static IBmbCharacter GetArcher()
        {
            var mock = new Mock<IBmbCharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Archer);
            mock.Setup(m => m.Tier)
                .Returns(5);

            return mock.Object;
        }
    }
}
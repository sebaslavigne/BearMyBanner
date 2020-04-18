using BearMyBanner.Wrappers;
using Moq;

namespace BearMyBannerTests
{
    public class CharacterFactory
    {
        public static ICharacter GetBasicInfantry()
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

        public static ICharacter GetLowTierInfantry()
        {
            var mock = new Mock<ICharacter>();
            mock.Setup(m => m.Occupation)
                .Returns(CharacterOccupation.Soldier);
            mock.Setup(m => m.Type)
                .Returns(TroopSpecialization.Infantry);
            mock.Setup(m => m.Tier)
                .Returns(2);

            return mock.Object;
        }

        public static ICharacter GetArcher()
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
    }
}
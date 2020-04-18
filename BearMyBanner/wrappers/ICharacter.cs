namespace BearMyBanner.Wrappers
{
    public interface ICharacter
    {
        bool IsHero { get; }
        TroopSpecialization Type { get; }
        CharacterOccupation Occupation { get; }
        int Tier { get; }
        bool IsPlayerCharacter { get; }
    }
}
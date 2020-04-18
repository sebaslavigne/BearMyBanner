namespace BearMyBanner.Wrapper
{
    public interface IBmbCharacter
    {
        bool IsHero { get; }
        TroopSpecialization Type { get; }
        CharacterOccupation Occupation { get; }
        int Tier { get; }
        bool IsPlayerCharacter { get; }
    }
}
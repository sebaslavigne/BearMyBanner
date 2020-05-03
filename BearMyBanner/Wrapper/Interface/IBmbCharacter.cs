namespace BearMyBanner.Wrapper
{
    public interface IBMBCharacter
    {
        bool IsHero { get; }
        TroopSpecialization Type { get; }
        CharacterOccupation Occupation { get; }
        int Tier { get; }
        bool IsPlayerCharacter { get; }
        string Id { get; }
    }
}
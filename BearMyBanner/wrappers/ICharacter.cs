namespace BearMyBanner.wrappers
{
    public interface ICharacter
    {
        bool IsHero { get; }
        TroopSpecialization Type { get; }
        CharacterOccupation Occupation { get; }
        int Tier { get; }
        bool IsPlayerCharacter { get; }
    }

    public enum CharacterOccupation
    {
        Soldier, CaravanGuard, Mercenary, Bandit, Lord, Lady, Wanderer
    }
}
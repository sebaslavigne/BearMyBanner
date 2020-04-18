using TaleWorlds.CampaignSystem;

namespace BearMyBanner.Wrapper
{
    public class CampaignCharacter : IBmbCharacter
    {
        private readonly CharacterObject _wrappedCharacter;

        public CampaignCharacter(CharacterObject wrappedCharacter)
        {
            _wrappedCharacter = wrappedCharacter;
        }

        public bool IsHero => _wrappedCharacter.IsHero;
        public TroopSpecialization Type => DetermineAgentSpec();
        public CharacterOccupation Occupation => DetermineOccupation();
        public int Tier => _wrappedCharacter.Tier;
        public bool IsPlayerCharacter => _wrappedCharacter.IsPlayerCharacter;


        private TroopSpecialization DetermineAgentSpec()
        {
            if (!_wrappedCharacter.IsArcher && !_wrappedCharacter.IsMounted) return TroopSpecialization.Infantry;
            if (_wrappedCharacter.IsArcher && !_wrappedCharacter.IsMounted) return TroopSpecialization.Archer;
            if (!_wrappedCharacter.IsArcher && _wrappedCharacter.IsMounted) return TroopSpecialization.Cavalry;
            return TroopSpecialization.HorseArcher;
        }

        private CharacterOccupation DetermineOccupation()
        {
            switch (_wrappedCharacter.Occupation)
            {
                //Soldier, CaravanGuard, Mercenary, Bandit, Lord, Lady, Wanderer
                case TaleWorlds.CampaignSystem.Occupation.Soldier:
                    return CharacterOccupation.Soldier;
                case TaleWorlds.CampaignSystem.Occupation.CaravanGuard:
                    return CharacterOccupation.CaravanGuard;
                case TaleWorlds.CampaignSystem.Occupation.Mercenary:
                    return CharacterOccupation.Mercenary;
                case TaleWorlds.CampaignSystem.Occupation.Bandit:
                    return CharacterOccupation.Bandit;
                case TaleWorlds.CampaignSystem.Occupation.Lord:
                    return CharacterOccupation.Lord;
                case TaleWorlds.CampaignSystem.Occupation.Lady:
                    return CharacterOccupation.Lady;
                case TaleWorlds.CampaignSystem.Occupation.Wanderer:
                    return CharacterOccupation.Wanderer;
            }

            return CharacterOccupation.Soldier;
        }

        public override bool Equals(object obj)
        {
            if (obj is CampaignCharacter other)
            {
                return _wrappedCharacter.Equals(other._wrappedCharacter);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _wrappedCharacter.GetHashCode();
        }
    }
}
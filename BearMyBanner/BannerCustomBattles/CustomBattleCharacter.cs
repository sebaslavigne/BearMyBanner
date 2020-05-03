using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace BearMyBanner.Wrapper
{
    public class CustomBattleCharacter : IBMBCharacter
    {

        private readonly BasicCharacterObject _wrappedCharacter;

        public CustomBattleCharacter(BasicCharacterObject wrappedCharacter)
        {
            _wrappedCharacter = wrappedCharacter;
        }
        public bool IsHero => _wrappedCharacter.IsHero;
        public TroopSpecialization Type => DetermineAgentSpec();
        public CharacterOccupation Occupation => CharacterOccupation.Soldier;
        public int Tier => 4;
        public bool IsPlayerCharacter => _wrappedCharacter.IsPlayerCharacter;
        public string Id => _wrappedCharacter.StringId;

        private TroopSpecialization DetermineAgentSpec()
        {
            if (!_wrappedCharacter.Equipment.HasRangedWeapon() && _wrappedCharacter.Equipment.Horse.IsEmpty) return TroopSpecialization.Infantry;
            if (_wrappedCharacter.Equipment.HasRangedWeapon() && _wrappedCharacter.Equipment.Horse.IsEmpty)  return TroopSpecialization.Archer;
            if (!_wrappedCharacter.Equipment.HasRangedWeapon() && !_wrappedCharacter.Equipment.Horse.IsEmpty) return TroopSpecialization.Cavalry;
            return TroopSpecialization.HorseArcher;
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomBattleCharacter other)
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

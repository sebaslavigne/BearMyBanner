using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner.Wrapper
{
    public class TournamentAgent : IBMBAgent
    {

        public TournamentAgent(Agent wrappedAgent)
        {
            WrappedAgent = wrappedAgent;
        }

        public bool IsAttacker => false;
        public bool IsDefender => false;
        public IBMBCharacter Character => throw new NotImplementedException();
        public string PartyName => (TeamColor)WrappedAgent.Team.TeamIndex + " team";
        public Agent WrappedAgent { get; }
        public bool HasMount => WrappedAgent.HasMount;
        public bool HasRangedWeapons => CheckForRangedWeapons();

        private bool CheckForRangedWeapons()
        {
            var equipment = WrappedAgent.SpawnEquipment;
            for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
            {
                if (!equipment[i].IsEmpty && (equipment[i].Item.Type == ItemObject.ItemTypeEnum.Bow || equipment[i].Item.Type == ItemObject.ItemTypeEnum.Crossbow))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

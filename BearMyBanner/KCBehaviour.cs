using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class KCBehaviour : MissionLogic
    {
        private readonly HashSet<InputKey> _allowedKeys = new HashSet<InputKey>() { InputKey.Up, InputKey.Down, InputKey.Left, InputKey.Right, InputKey.A, InputKey.B, InputKey.Enter };
        private readonly InputKey[] _konamiCode = new InputKey[] { InputKey.Up, InputKey.Up, InputKey.Down, InputKey.Down, InputKey.Left, InputKey.Right, InputKey.Left, InputKey.Right, InputKey.B, InputKey.A, InputKey.Enter };
        private int _index = 0;

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            try
            {
                foreach(InputKey key in _allowedKeys)
                {
                    if (Input.IsKeyPressed(key))
                    {
                        RegisterKey(key);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        private void RegisterKey(InputKey key)
        {
            if (key == _konamiCode[_index])
            {
                _index++;
            } 
            else
            {
                _index = 0;
                //So the first key is recognized after a failure
                if (key == _konamiCode[0]) RegisterKey(key);
            }

            if (_index == _konamiCode.Length)
            {
                _index = 0;
                Activate();
            }
        }
        
        private void Activate()
        {
            foreach (Agent agent in this.Mission.Agents)
            {
                if (agent.IsHuman)
                {
                    agent.EquipBanner(Banner.CreateRandomBanner());
                    agent.UnequipAllAndEquipStones();
                }
            }
            InformationManager.AddQuickInformation(new TextObject("Konami code!"), 0, null, "event:/mission/tutorial/finish_task");
        }
    }
}

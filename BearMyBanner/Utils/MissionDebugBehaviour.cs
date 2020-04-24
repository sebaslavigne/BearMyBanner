using System;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public class MissionDebugBehaviour : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            try
            {
                    if (Input.IsKeyPressed(InputKey.End))
                    {
                        ; //Empty for debugging
                    }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}

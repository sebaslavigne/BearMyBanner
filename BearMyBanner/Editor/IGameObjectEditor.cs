using BearMyBanner.Wrapper;
using TaleWorlds.MountAndBlade;

namespace BearMyBanner
{
    public interface IGameObjectEditor
    {
        void AddBannerToAgentSpawnEquipment(IBMBAgent agent);
        bool CheckIfAgentHasBanner(IBMBAgent agent, BannerType type);
        void AddBigBannerToAgentSpawnEquipment(IBMBAgent agent);
    }
}
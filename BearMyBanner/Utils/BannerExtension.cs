using BearMyBanner.Wrapper;
using TaleWorlds.Core;

namespace BearMyBanner
{
    public static class BannerExtension
    {
        public static void ChangeBanner(this Banner banner, IBMBBanner newBanner)
        {
            banner.Deserialize(newBanner.Key);
        }

        public static void ChangeBaseColors(this Banner banner, int colorId, int colorId2)
        {
            banner.BannerDataList[0].ColorId = colorId;
            banner.BannerDataList[0].ColorId2 = colorId2;
        }

        public static void ChangeIconColor(this Banner banner, int colorId)
        {
            for (int i = 1; i < banner.BannerDataList.Count; i++)
            {
                banner.BannerDataList[i].ColorId = colorId;
            }
        }

        public static void ChangeIconMesh(this Banner banner, int meshId)
        {
            for (int i = 1; i < banner.BannerDataList.Count; i++)
            {
                banner.BannerDataList[i].MeshId = meshId;
            }
        }
    }
}

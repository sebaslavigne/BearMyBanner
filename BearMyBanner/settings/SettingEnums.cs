using System;

namespace BearMyBanner.Settings
{
    public enum UnitCountMode
    {
        Spec, Formation, Troop
    }

    public enum DropRetreatMode
    {
        Disabled, Weighted, Fixed
    }

    public enum BanditAssignMode
    {
        NotAllowed, RecruitedOnly, Allowed
    }

    public enum CaravanAssignMode
    {
        NotAllowed, OnlyMasters, Allowed
    }

}

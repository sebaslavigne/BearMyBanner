using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using ModLib;
using ModLib.Attributes;

namespace BearMyBanner
{
    public class BMBSettings : SettingsBase
    {
        public const string InstanceID = "BMBSettings";
        public const string groupTroopFilters = "1. Troop filters";
        public const string subGroupOccupation = groupTroopFilters + "/1.1 Enable troops by occupation";
        public const string subGroupFormation = groupTroopFilters + "/1.2 Enable troops by formation";
        public const string subGroupTier = groupTroopFilters + "/1.3 Allow only select tiers";
        public const string groupHeroes = "2. Enable heroes";
        public const string groupRatios = "3. Bearer ratios";

        public override string ModName => "Bear my Banner";

        public override string ModuleFolderName => Main.ModuleFolderName;

        [XmlElement]
        public override string ID { get; set; } = InstanceID;

        public static BMBSettings Instance
        {
            get
            {
                return (BMBSettings)SettingsDatabase.GetSettings(InstanceID);
            }
        }

        /* PROPERTIES */
        /* TROOP BY OCCUPATION */
        [XmlElement]
        [SettingProperty("Allow soldiers", "Allows soldiers to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowSoldiers { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow caravan guards", "Allows caravan guards to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowCaravanGuards { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow mercenaries", "Allows mercenaries to bear banners (i.e. Watchmen, Hired Blades...).")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowMercenaries { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow bandits", "Allows bandits to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowBandits { get; set; } = false;

        /* TROOP BY FORMATION */
        [XmlElement]
        [SettingProperty("Allow infantry", "Allows infantry troops to bear banners.")]
        [SettingPropertyGroup(subGroupFormation)]
        public bool AllowInfantry { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow cavalry", "Allows mounted troops to bear banners.")]
        [SettingPropertyGroup(subGroupFormation)]
        public bool AllowMounted { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow archers and crossbowmen", "Allows ranged troops to bear banners.")]
        [SettingPropertyGroup(subGroupFormation)]
        public bool AllowRanged { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow horse archers", "Allows mounted archer troops to bear banners.")]
        [SettingPropertyGroup(subGroupFormation)]
        public bool AllowMountedRanged { get; set; } = false;

        /* TROOP BY TIER */
        [XmlElement]
        [SettingProperty("3. Allow only select troop tiers to bear banners", "If disabled, all tiers will bear banners.")]
        [SettingPropertyGroup(subGroupTier, true)]
        public bool FilterTiers { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 1", "Allow tier 1 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier1 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 2", "Allow tier 2 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier2 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 3", "Allow tier 3 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier3 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 4", "Allow tier 4 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier4 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 5", "Allow tier 5 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier5 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 6", "Allow tier 6 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier6 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 7+", "Allow tiers 7 and above units to bear banners. (Some mods might have 7+ tier units)")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier7Plus { get; set; } = false;

        /* HEROES */
        [XmlElement]
        [SettingProperty("Allow player", "Equip player with a banner at the start of a battle. (NOTE: Equip shields with numpad instead of scroll wheel)")]
        [SettingPropertyGroup(groupHeroes)]
        public bool AllowPlayer { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow companions", "Equips all companions with a banner at the start of a battle.")]
        [SettingPropertyGroup(groupHeroes)]
        public bool AllowCompanions { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow nobles", "Equips all nobles with a banner at the start of a battle (Might confuse archer nobles).")]
        [SettingPropertyGroup(groupHeroes)]
        public bool AllowNobles { get; set; } = false;

        /* RATIOS */
        [XmlElement]
        [SettingProperty("Banner bearers ratio", 1, 100, "Gives a banner to every 1 in X troops of the same type.")]
        [SettingPropertyGroup(groupRatios)]
        public int BearerToTroopRatio { get; set; } = 7;
        [XmlElement]
        [SettingProperty("Minimum type amount", 0, 100, "Will only give banners to a troop type when there are at least X of them.")]
        [SettingPropertyGroup(groupRatios)]
        public int MinTroopTypeAmount { get; set; } = 5;
    }
}

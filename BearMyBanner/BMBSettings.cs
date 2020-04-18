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
        public const string GroupTroopFilters = "1. Troop filters";
        public const string SubGroupOccupation = GroupTroopFilters + "/1.1 Enable troops by occupation";
        public const string SubGroupSpec = GroupTroopFilters + "/1.2 Enable troops by specialization";
        public const string SubGroupTier = GroupTroopFilters + "/1.3 Allow only select tiers";
        public const string SubGroupHeroes = GroupTroopFilters + "/1.4 Enable heroes";
        public const string GroupBanners = "2. Banner settings";
        public const string SubGroupRatios = GroupBanners + "/2.1 Ratio";
        public const string SubGroupSieges = GroupBanners + "/2.2 Use banners during sieges";
        public const string SubGroupHideouts = GroupBanners + "/2.2 Use banners during hideout attacks";
        public const string GroupMisc = "3. Misc";

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
        [SettingPropertyGroup(SubGroupOccupation)]
        public bool AllowSoldiers { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow caravan guards", "Allows caravan guards to bear banners.")]
        [SettingPropertyGroup(SubGroupOccupation)]
        public bool AllowCaravanGuards { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow mercenaries", "Allows mercenaries to bear banners (i.e. Watchmen, Hired Blades...).")]
        [SettingPropertyGroup(SubGroupOccupation)]
        public bool AllowMercenaries { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow bandits", "Allows bandits to bear banners.")]
        [SettingPropertyGroup(SubGroupOccupation)]
        public bool AllowBandits { get; set; } = false;

        /* TROOP BY SPECIALIZATION */
        [XmlElement]
        [SettingProperty("Allow infantry", "Allows infantry troops to bear banners.")]
        [SettingPropertyGroup(SubGroupSpec)]
        public bool AllowInfantry { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow cavalry", "Allows mounted troops to bear banners.")]
        [SettingPropertyGroup(SubGroupSpec)]
        public bool AllowMounted { get; set; } = true;
        [XmlElement]
        [SettingProperty("Allow archers and crossbowmen", "Allows ranged troops to bear banners. NOTE: Bearers won't have bows or crossbows and will rush the enemy if ordered to charge.")]
        [SettingPropertyGroup(SubGroupSpec)]
        public bool AllowRanged { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow horse archers", "Allows mounted archer troops to bear banners. NOTE: Bearers won't have bows and will rush the enemy if ordered to charge.")]
        [SettingPropertyGroup(SubGroupSpec)]
        public bool AllowMountedRanged { get; set; } = false;

        /* TROOP BY TIER */
        [XmlElement]
        [SettingProperty("3. Allow only select troop tiers to bear banners", "If disabled, all tiers will bear banners.")]
        [SettingPropertyGroup(SubGroupTier, true)]
        public bool FilterTiers { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 1", "Allow tier 1 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier1 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 2", "Allow tier 2 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier2 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 3", "Allow tier 3 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier3 { get; set; } = false;
        [XmlElement]
        [SettingProperty("Tier 4", "Allow tier 4 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier4 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 5", "Allow tier 5 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier5 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 6", "Allow tier 6 units to bear banners.")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier6 { get; set; } = true;
        [XmlElement]
        [SettingProperty("Tier 7+", "Allow tiers 7 and above units to bear banners. (Some mods might have 7+ tier units)")]
        [SettingPropertyGroup(SubGroupTier)]
        public bool AllowTier7Plus { get; set; } = false;

        /* HEROES */
        [XmlElement]
        [SettingProperty("Allow player", "Equip player with a banner at the start of a battle. NOTE: Equipping a shield with the scroll wheel stops working for some reason. Use Numpad (or your configured hotkey in game settings) instead.")]
        [SettingPropertyGroup(SubGroupHeroes)]
        public bool AllowPlayer { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow companions", "Equips all companions with a banner at the start of a battle.")]
        [SettingPropertyGroup(SubGroupHeroes)]
        public bool AllowCompanions { get; set; } = false;
        [XmlElement]
        [SettingProperty("Allow nobles", "Equips all nobles with a banner at the start of a battle NOTE: Nobles with bows might be confused.")]
        [SettingPropertyGroup(SubGroupHeroes)]
        public bool AllowNobles { get; set; } = false;

        /* RATIOS */
        [XmlElement]
        [SettingProperty("Banner bearers to troops ratio", 1, 100, "Gives a banner to every 1 in X troops of the same type.")]
        [SettingPropertyGroup(SubGroupRatios)]
        public int BearerToTroopRatio { get; set; } = 7;
        [XmlElement]
        [SettingProperty("Ignore troop types", "Uses specializations (Infantry, Archer, Cavalry and Horse Archer) instead of troop types to count equipped banners. Useful if you have diverse armies, but which units get banners is more random.")]
        [SettingPropertyGroup(SubGroupRatios)]
        public bool UseTroopSpecs { get; set; } = false;

        /* SIEGES*/
        [XmlElement]
        [SettingProperty("Allow banners during sieges", "Enables the use of banners in sieges.")]
        [SettingPropertyGroup(SubGroupSieges, true)]
        public bool AllowSieges { get; set; } = true;
        [XmlElement]
        [SettingProperty("Attackers use banners", "Gives banners to the attacking side.")]
        [SettingPropertyGroup(SubGroupSieges)]
        public bool SiegeAttackersUseBanners { get; set; } = true;
        [XmlElement]
        [SettingProperty("Defenders use banners", "Gives banners to the defending side.")]
        [SettingPropertyGroup(SubGroupSieges)]
        public bool SiegeDefendersUseBanners{ get; set; } = false;

        /* HIDEOUTS */
        [XmlElement]
        [SettingProperty("Allow banners during hideout attacks", "Enables the use of banners in hideout attacks.")]
        [SettingPropertyGroup(SubGroupHideouts, true)]
        public bool AllowHideouts { get; set; } = false;
        [XmlElement]
        [SettingProperty("Attackers use banners", "Gives banners to the attacking side.")]
        [SettingPropertyGroup(SubGroupHideouts)]
        public bool HideoutAttackersUseBanners { get; set; } = false;
        [XmlElement]
        [SettingProperty("Bandits use banners", "Gives banners to the bandits, regardless of the troop filter settings.")]
        [SettingPropertyGroup(SubGroupHideouts)]
        public bool HideoutBanditsUseBanners { get; set; } = false;

        /* MISC */
        [XmlElement]
        [SettingProperty("Show messages", "Shows mod messages in Message Log.")]
        [SettingPropertyGroup(GroupMisc)]
        public bool ShowMessages { get; set; } = true;
    }
}

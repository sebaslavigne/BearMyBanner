using System.Xml.Serialization;

using ModLib;
using ModLib.Attributes;

namespace BearMyBanner
{
    public class BMBSettings : SettingsBase, IBMBSettings
    {
        public const string InstanceID = "BMBSettings";
        public const string groupTroopFilters = "1. Troop filters";
        public const string subGroupOccupation = groupTroopFilters + "/1.1 Enable troops by occupation";
        public const string subGroupSpec = groupTroopFilters + "/1.2 Enable troops by specialization";
        public const string subGroupTier = groupTroopFilters + "/1.3 Allow only select tiers";
        public const string subGroupHeroes = groupTroopFilters + "/1.4 Enable heroes";
        public const string groupBanners = "2. Banner settings";
        public const string subGroupRatios = groupBanners + "/2.1 Ratio";
        public const string subGroupSieges = groupBanners + "/2.2 Use banners during sieges";
        public const string subGroupHideouts = groupBanners + "/2.2 Use banners during hideout attacks";
        public const string groupMisc = "3. Misc";

        public override string ModName => "Bear my Banner";

        public override string ModuleFolderName => Main.ModuleFolderName;

        [XmlElement]
        public override string ID { get; set; } = InstanceID;

        public static BMBSettings Instance => (BMBSettings)SettingsDatabase.GetSettings(InstanceID);

        /* PROPERTIES */
        /* TROOP BY OCCUPATION */
        [XmlElement]
        [SettingProperty("Allow soldiers", "Allows soldiers to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowSoldiers { get; set; }
        [XmlElement]
        [SettingProperty("Allow caravan guards", "Allows caravan guards to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowCaravanGuards { get; set; }
        [XmlElement]
        [SettingProperty("Allow mercenaries", "Allows mercenaries to bear banners (i.e. Watchmen, Hired Blades...).")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowMercenaries { get; set; }
        [XmlElement]
        [SettingProperty("Allow bandits", "Allows bandits to bear banners.")]
        [SettingPropertyGroup(subGroupOccupation)]
        public bool AllowBandits { get; set; }

        /* TROOP BY SPECIALIZATION */
        [XmlElement]
        [SettingProperty("Allow infantry", "Allows infantry troops to bear banners.")]
        [SettingPropertyGroup(subGroupSpec)]
        public bool AllowInfantry { get; set; }
        [XmlElement]
        [SettingProperty("Allow cavalry", "Allows mounted troops to bear banners.")]
        [SettingPropertyGroup(subGroupSpec)]
        public bool AllowMounted { get; set; }
        [XmlElement]
        [SettingProperty("Allow archers and crossbowmen", "Allows ranged troops to bear banners. NOTE: Bearers won't have bows or crossbows and will rush the enemy if ordered to charge.")]
        [SettingPropertyGroup(subGroupSpec)]
        public bool AllowRanged { get; set; }
        [XmlElement]
        [SettingProperty("Allow horse archers", "Allows mounted archer troops to bear banners. NOTE: Bearers won't have bows and will rush the enemy if ordered to charge.")]
        [SettingPropertyGroup(subGroupSpec)]
        public bool AllowMountedRanged { get; set; }

        /* TROOP BY TIER */
        [XmlElement]
        [SettingProperty("3. Allow only select troop tiers to bear banners", "If disabled, all tiers will bear banners.")]
        [SettingPropertyGroup(subGroupTier, true)]
        public bool FilterTiers { get; set; }
        [XmlElement]
        [SettingProperty("Tier 1", "Allow tier 1 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier1 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 2", "Allow tier 2 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier2 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 3", "Allow tier 3 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier3 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 4", "Allow tier 4 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier4 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 5", "Allow tier 5 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier5 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 6", "Allow tier 6 units to bear banners.")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier6 { get; set; }
        [XmlElement]
        [SettingProperty("Tier 7+", "Allow tiers 7 and above units to bear banners. (Some mods might have 7+ tier units)")]
        [SettingPropertyGroup(subGroupTier)]
        public bool AllowTier7Plus { get; set; }

        /* HEROES */
        [XmlElement]
        [SettingProperty("Allow player", "Equip player with a banner at the start of a battle. NOTE: Equipping a shield with the scroll wheel stops working for some reason. Use Numpad (or your configured hotkey in game settings) instead.")]
        [SettingPropertyGroup(subGroupHeroes)]
        public bool AllowPlayer { get; set; }
        [XmlElement]
        [SettingProperty("Allow companions", "Equips all companions with a banner at the start of a battle.")]
        [SettingPropertyGroup(subGroupHeroes)]
        public bool AllowCompanions { get; set; }
        [XmlElement]
        [SettingProperty("Allow nobles", "Equips all nobles with a banner at the start of a battle NOTE: Nobles with bows might be confused.")]
        [SettingPropertyGroup(subGroupHeroes)]
        public bool AllowNobles { get; set; }

        /* RATIOS */
        [XmlElement]
        [SettingProperty("Banner bearers to troops ratio", 1, 100, "Gives a banner to every 1 in X troops of the same type.")]
        [SettingPropertyGroup(subGroupRatios)]
        public int BearerToTroopRatio { get; set; }
        [XmlElement]
        [SettingProperty("Ignore troop types", "Uses specializations (Infantry, Archer, Cavalry and Horse Archer) instead of troop types to count equipped banners. Useful if you have diverse armies, but which units get banners is more random.")]
        [SettingPropertyGroup(subGroupRatios)]
        public bool UseTroopSpecs { get; set; }

        /* SIEGES*/
        [XmlElement]
        [SettingProperty("Allow banners during sieges", "Enables the use of banners in sieges.")]
        [SettingPropertyGroup(subGroupSieges, true)]
        public bool AllowSieges { get; set; }
        [XmlElement]
        [SettingProperty("Attackers use banners", "Gives banners to the attacking side.")]
        [SettingPropertyGroup(subGroupSieges)]
        public bool SiegeAttackersUseBanners { get; set; }
        [XmlElement]
        [SettingProperty("Defenders use banners", "Gives banners to the defending side.")]
        [SettingPropertyGroup(subGroupSieges)]
        public bool SiegeDefendersUseBanners{ get; set; }

        /* HIDEOUTS */
        [XmlElement]
        [SettingProperty("Allow banners during hideout attacks", "Enables the use of banners in hideout attacks.")]
        [SettingPropertyGroup(subGroupHideouts, true)]
        public bool AllowHideouts { get; set; }
        [XmlElement]
        [SettingProperty("Attackers use banners", "Gives banners to the attacking side.")]
        [SettingPropertyGroup(subGroupHideouts)]
        public bool HideoutAttackersUseBanners { get; set; }
        [XmlElement]
        [SettingProperty("Bandits use banners", "Gives banners to the bandits, regardless of the troop filter settings.")]
        [SettingPropertyGroup(subGroupHideouts)]
        public bool HideoutBanditsUseBanners { get; set; }

        /* MISC */
        [XmlElement]
        [SettingProperty("Show messages", "Shows mod messages in Message Log.")]
        [SettingPropertyGroup(groupMisc)]
        public bool ShowMessages { get; set; }
    }
}

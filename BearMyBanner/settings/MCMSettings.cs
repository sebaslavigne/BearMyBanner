using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Data;
using MBOptionScreen.Settings;

namespace BearMyBanner.Settings
{
    public class MCMSettings : AttributeSettings<MCMSettings>, IBMBSettings, IBMBFormationBanners
    {
        private const string g0 = "A. Main Settings";
        private const string g00 = g0 + "/I. Battle Types";
        private const string g000 = g00 + "/i. Sieges";
        private const string g001 = g00 + "/ii. Hideout Attacks";
        private const string g002 = g00 + "/iii. Tournaments";
        private const string g003 = g00 + "/iv. Towns and villages";
        private const string g01 = g0 + "/II. Filters";
        private const string g010 = g01 + "/i. Ratios";
        private const string g011 = g01 + "/ii. Occupation";
        private const string g012 = g01 + "/iii. Type";
        private const string g013 = g01 + "/iv. Tier";
        private const string g014 = g01 + "/v. Heroes";
        private const string g02 = g0 + "/III. Miscellaneous";
        private const string g1 = "B. Formation Banners Settings";

        public MCMSettings()
        {
            this.SetDefaultSettings();
            this.SetDefaultFormationSettings();
        }

        public override string Id { get; set; } = "BearMyBanner_v0_7_0";
        public override string ModuleFolderName => Main.ModuleFolderName;
        public override string ModName => Main.ModName;

        //==== Main Settings ========================================================================================

        //== Sieges ==

        [SettingPropertyBool(displayName: MCMDisplayName.AllowSieges, Order = 0, RequireRestart = false, HintText = MCMHint.AllowSieges)]
        [SettingPropertyGroup(g000)]
        public bool AllowSieges { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.SiegeAttackersUseBanners, Order = 1, RequireRestart = false, HintText = MCMHint.SiegeAttackersUseBanners)]
        [SettingPropertyGroup(g000)]
        public bool SiegeAttackersUseBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.SiegeDefendersUseBanners, Order = 2, RequireRestart = false, HintText = MCMHint.SiegeDefendersUseBanners)]
        [SettingPropertyGroup(g000)]
        public bool SiegeDefendersUseBanners { get; set; }

        //== Hideout attacks ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowHideouts, Order = 0, RequireRestart = false, HintText = MCMHint.AllowHideouts)]
        [SettingPropertyGroup(g001)]
        public bool AllowHideouts { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.HideoutAttackersUseBanners, Order = 1, RequireRestart = false, HintText = MCMHint.HideoutAttackersUseBanners)]
        [SettingPropertyGroup(g001)]
        public bool HideoutAttackersUseBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.HideoutBanditsUseBanners, Order = 2, RequireRestart = false, HintText = MCMHint.HideoutBanditsUseBanners)]
        [SettingPropertyGroup(g001)]
        public bool HideoutBanditsUseBanners { get; set; }

        //== Tournaments ==
        [SettingPropertyBool(displayName: MCMDisplayName.TournamentBanners, Order = 0, RequireRestart = false, HintText = MCMHint.TournamentBanners)]
        [SettingPropertyGroup(g002)]
        public bool TournamentBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.TournamentThemes, Order = 1, RequireRestart = false, HintText = MCMHint.TournamentThemes)]
        [SettingPropertyGroup(g002)]
        public bool TournamentThemes { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.TournamentBannersInShields, Order = 2, RequireRestart = false, HintText = MCMHint.TournamentBannersInShields)]
        [SettingPropertyGroup(g002)]
        public bool TournamentBannersInShields { get; set; }

        //== Town and village visits ==
        [SettingPropertyBool(displayName: MCMDisplayName.TownCastleVisitBanner, Order = 0, RequireRestart = false, HintText = MCMHint.TownCastleVisitBanner)]
        [SettingPropertyGroup(g003)]
        public bool TownCastleVisitBanner { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.VillageVisitBanner, Order = 1, RequireRestart = false, HintText = MCMHint.VillageVisitBanner)]
        [SettingPropertyGroup(g003)]
        public bool VillageVisitBanner { get; set; }

        //==== Troop filters ========================================================================================

        //== Banner ratios ==
        [SettingPropertyInteger(displayName: MCMDisplayName.BearerToTroopRatio, minValue: 1, maxValue: 50, Order = 0, RequireRestart = false, HintText = MCMHint.BearerToTroopRatio/*, valueFormat: "0"*/)]
        [SettingPropertyGroup(g010)]
        public int BearerToTroopRatio { get; set; }

        [SettingPropertyDropdown(displayName: MCMDisplayName.UnitCountMode, Order = 1, RequireRestart = false, HintText = MCMHint.UnitCountMode)]
        [SettingPropertyGroup(g010)]
        public DefaultDropdown<string> UnitCountModeSetting { get; set; } = new DefaultDropdown<string>(new string[]
        {
            "General occupation",
            "Specific troop type"
        }, 0);
        public UnitCountMode UnitCountMode { get => (UnitCountMode)UnitCountModeSetting.SelectedIndex; set => UnitCountModeSetting.SelectedIndex = (int)value; }

        //== Filter by occupation ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowSoldiers, Order = 0, RequireRestart = false, HintText = MCMHint.AllowSoldiers)]
        [SettingPropertyGroup(g011)]
        public bool AllowSoldiers { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowCaravanGuards, Order = 1, RequireRestart = false, HintText = MCMHint.AllowCaravanGuards)]
        [SettingPropertyGroup(g011)]
        public bool AllowCaravanGuards { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMercenaries, Order = 2, RequireRestart = false, HintText = MCMHint.AllowMercenaries)]
        [SettingPropertyGroup(g011)]
        public bool AllowMercenaries { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowBandits, Order = 3, RequireRestart = false, HintText = MCMHint.AllowBandits)]
        [SettingPropertyGroup(g011)]
        public bool AllowBandits { get; set; }

        //== Filter by type ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowInfantry, Order = 0, RequireRestart = false, HintText = MCMHint.AllowInfantry)]
        [SettingPropertyGroup(g012)]
        public bool AllowInfantry { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowRanged, Order = 1, RequireRestart = false, HintText = MCMHint.AllowRanged)]
        [SettingPropertyGroup(g012)]
        public bool AllowRanged { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMounted, Order = 2, RequireRestart = false, HintText = MCMHint.AllowMounted)]
        [SettingPropertyGroup(g012)]
        public bool AllowMounted { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMountedRanged, Order = 3, RequireRestart = false, HintText = MCMHint.AllowMountedRanged)]
        [SettingPropertyGroup(g012)]
        public bool AllowMountedRanged { get; set; }

        //== Filter by tier ==
        [SettingPropertyBool(displayName: MCMDisplayName.FilterTiers, Order = 0, RequireRestart = false, HintText = MCMHint.FilterTiers)]
        [SettingPropertyGroup(g013)]
        public bool FilterTiers { get; set; }

        [SettingPropertyText(displayName: MCMDisplayName.AllowedTiers, order: 1, requireRestart: false, hintText: MCMHint.AllowedTiers)]
        [SettingPropertyGroup(g013)]
        public string AllowedTiers { get; set; }

        //== Heroes ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowPlayer, Order = 0, RequireRestart = false, HintText = MCMHint.AllowPlayer)]
        [SettingPropertyGroup(g014)]
        public bool AllowPlayer { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowCompanions, Order = 1, RequireRestart = false, HintText = MCMHint.AllowCompanions)]
        [SettingPropertyGroup(g014)]
        public bool AllowCompanions { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowNobles, Order = 2, RequireRestart = false, HintText = MCMHint.AllowNobles)]
        [SettingPropertyGroup(g014)]
        public bool AllowNobles { get; set; }

        //==== Miscellaneous ========================================================================================
        [SettingPropertyBool(displayName: MCMDisplayName.ShowMessages, Order = 0, RequireRestart = false, HintText = MCMHint.ShowMessages)]
        [SettingPropertyGroup(g02)]
        public bool ShowMessages { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.WhiteMessages, Order = 1, RequireRestart = false, HintText = MCMHint.WhiteMessages)]
        [SettingPropertyGroup(g02)]
        public bool WhiteMessages { get; set; }

        public bool ReloadFiles { get { return false; } set { } }

        //======== FORMATIONS =======================================================================================================================
        //===========================================================================================================================================

        [SettingPropertyBool(displayName: MCMDisplayName.EnableFormationBanners, Order = 0, RequireRestart = false, HintText = MCMHint.EnableFormationBanners)]
        [SettingPropertyGroup(g1)]
        public bool EnableFormationBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.CompanionsUseFormationBanners, Order = 2, RequireRestart = false, HintText = MCMHint.CompanionsUseFormationBanners)]
        [SettingPropertyGroup(g1)]
        public bool CompanionsUseFormationBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.UseInShields, Order = 3, RequireRestart = false, HintText = MCMHint.UseInShields)]
        [SettingPropertyGroup(g1)]
        public bool UseInShields { get; set; }

        //== Banner codes ==

        //[SettingPropertyText(displayName: MCMDisplayName.Infantry, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string Infantry { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.Ranged, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string Ranged { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.Cavalry, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string Cavalry { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.HorseArcher, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string HorseArcher { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.Skirmisher, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string Skirmisher { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.HeavyInfantry, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string HeavyInfantry { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.LightCavalry, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string LightCavalry { get; set; }

        //[SettingPropertyText(displayName: MCMDisplayName.HeavyCavalry, order: 1, requireRestart: false, hintText: MCMHint.BannerCodeHint)]
        //[SettingPropertyGroup(g1)]
        public string HeavyCavalry { get; set; }
    }
}

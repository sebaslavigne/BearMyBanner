using System.Collections.Generic;
using MBOptionScreen.Attributes;
using MBOptionScreen.Attributes.v2;
using MBOptionScreen.Settings;

namespace BearMyBanner.Settings
{
    public class MCMSettings : AttributeSettings<MCMSettings>, IBMBSettings
    {
        private const string g0 = "Settings";
        private const string g00 = g0 + "/Battle Types";
        private const string g000 = g00 + "/Sieges";
        private const string g001 = g00 + "/Hideout Attacks";
        private const string g002 = g00 + "/Tournaments";
        private const string g003 = g00 + "/Towns and villages";
        private const string g01 = g0 + "/Filters";
        private const string g010 = g01 + "/Ratios";
        private const string g011 = g01 + "/Occupation";
        private const string g012 = g01 + "/Type";
        private const string g013 = g01 + "/Tier";
        private const string g014 = g01 + "/Heroes";
        private const string g02 = g0 + "/Miscellaneous";
        private const string g1 = "Formations";

        public MCMSettings()
        {
            this.SetDefaults();
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
        [SettingPropertyBool(displayName: MCMDisplayName.AllowHideouts, Order = 3, RequireRestart = false, HintText = MCMHint.AllowHideouts)]
        [SettingPropertyGroup(g001)]
        public bool AllowHideouts { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.HideoutAttackersUseBanners, Order = 4, RequireRestart = false, HintText = MCMHint.HideoutAttackersUseBanners)]
        [SettingPropertyGroup(g001)]
        public bool HideoutAttackersUseBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.HideoutBanditsUseBanners, Order = 5, RequireRestart = false, HintText = MCMHint.HideoutBanditsUseBanners)]
        [SettingPropertyGroup(g001)]
        public bool HideoutBanditsUseBanners { get; set; }

        //== Tournaments ==
        [SettingPropertyBool(displayName: MCMDisplayName.TournamentBanners, Order = 6, RequireRestart = false, HintText = MCMHint.TournamentBanners)]
        [SettingPropertyGroup(g002)]
        public bool TournamentBanners { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.TournamentThemes, Order = 7, RequireRestart = false, HintText = MCMHint.TournamentThemes)]
        [SettingPropertyGroup(g002)]
        public bool TournamentThemes { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.TournamentBannersInShields, Order = 8, RequireRestart = false, HintText = MCMHint.TournamentBannersInShields)]
        [SettingPropertyGroup(g002)]
        public bool TournamentBannersInShields { get; set; }

        //== Town and village visits ==
        public bool TownCastleVisitBanner { get; set; }
        public bool VillageVisitBanner { get; set; }

        //==== Troop filters ========================================================================================

        //== Banner ratios ==
        public int BearerToTroopRatio { get; set; }
        public UnitCountMode UnitCountMode { get; set; }

        //== Filter by occupation ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowSoldiers, Order = 9, RequireRestart = false, HintText = MCMHint.AllowSoldiers)]
        [SettingPropertyGroup(g011)]
        public bool AllowSoldiers { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowCaravanGuards, Order = 10, RequireRestart = false, HintText = MCMHint.AllowCaravanGuards)]
        [SettingPropertyGroup(g011)]
        public bool AllowCaravanGuards { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMercenaries, Order = 11, RequireRestart = false, HintText = MCMHint.AllowMercenaries)]
        [SettingPropertyGroup(g011)]
        public bool AllowMercenaries { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowBandits, Order = 12, RequireRestart = false, HintText = MCMHint.AllowBandits)]
        [SettingPropertyGroup(g011)]
        public bool AllowBandits { get; set; }

        //== Filter by type ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowInfantry, Order = 13, RequireRestart = false, HintText = MCMHint.AllowInfantry)]
        [SettingPropertyGroup(g012)]
        public bool AllowInfantry { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowRanged, Order = 14, RequireRestart = false, HintText = MCMHint.AllowRanged)]
        [SettingPropertyGroup(g012)]
        public bool AllowRanged { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMounted, Order = 15, RequireRestart = false, HintText = MCMHint.AllowMounted)]
        [SettingPropertyGroup(g012)]
        public bool AllowMounted { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowMountedRanged, Order = 16, RequireRestart = false, HintText = MCMHint.AllowMountedRanged)]
        [SettingPropertyGroup(g012)]
        public bool AllowMountedRanged { get; set; }

        //== Filter by tier ==
        [SettingPropertyBool(displayName: MCMDisplayName.FilterTiers, Order = 17, RequireRestart = false, HintText = MCMHint.FilterTiers)]
        [SettingPropertyGroup(g013)]
        public bool FilterTiers { get; set; }


        public string AllowedTiers { get; set; }

        //== Heroes ==
        [SettingPropertyBool(displayName: MCMDisplayName.AllowPlayer, Order = 18, RequireRestart = false, HintText = MCMHint.AllowPlayer)]
        [SettingPropertyGroup(g014)]
        public bool AllowPlayer { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowCompanions, Order = 19, RequireRestart = false, HintText = MCMHint.AllowCompanions)]
        [SettingPropertyGroup(g014)]
        public bool AllowCompanions { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.AllowNobles, Order = 20, RequireRestart = false, HintText = MCMHint.AllowNobles)]
        [SettingPropertyGroup(g014)]
        public bool AllowNobles { get; set; }

        //==== Miscellaneous ========================================================================================
        [SettingPropertyBool(displayName: MCMDisplayName.ShowMessages, Order = 21, RequireRestart = false, HintText = MCMHint.ShowMessages)]
        [SettingPropertyGroup(g02)]
        public bool ShowMessages { get; set; }

        [SettingPropertyBool(displayName: MCMDisplayName.WhiteMessages, Order = 22, RequireRestart = false, HintText = MCMHint.WhiteMessages)]
        [SettingPropertyGroup(g02)]
        public bool WhiteMessages { get; set; }


        public bool ReloadFiles { get { return false; } set { } }
    }
}

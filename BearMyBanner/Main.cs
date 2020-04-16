using System;
using System.Linq;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using ModLib;

namespace BearMyBanner
{
    public class Main : MBSubModuleBase
    {

        public const string ModuleFolderName = "BearMyBanner";

        private bool enteredMission = false;

        /* TROOP BY OCCUPATION */
        private bool allowSoldiers = true;
        private bool allowCaravanGuards = false;
        private bool allowMercenaries = false;
        private bool allowBandits = false;

        /* TROOP BY FORMATION */
        private bool allowInfantry = true;
        private bool allowMounted = true;
        private bool allowRanged = false;
        private bool allowMountedRanged = false;

        /* TROOP BY TIER */
        private bool filterTiers = true;
        private List<int> allowedTiers = new List<int>() { 4, 5, 6, 7 };

        /* HEROES */
        private bool allowPlayer = false;
        private bool allowCompanions = false;
        private bool allowNobles = false;

        /* RATIOS */
        private int bearerToTroopRatio = 7;
        private int minTroopTypeAmount = 5;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                FileDatabase.Initialise(ModuleFolderName);
                BMBSettings settings = FileDatabase.Get<BMBSettings>(BMBSettings.InstanceID);
                if (settings == null) settings = new BMBSettings();
                SettingsDatabase.RegisterSettings(settings);
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Error: " + ex.Message));
            }
        }

        /* This seems like an excellent point to give the banners, but agent's haven't been initialized yet
         * Might need to overwrite or hook onto a MissionBehaviour instead in a future version
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);
            InformationManager.DisplayMessage(new InformationMessage("OnMissionBehaviourInitialize"));
        }
        */

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            InformationManager.DisplayMessage(new InformationMessage("Loaded Bear my Banner", Color.FromUint(4282569842U)));
        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);
            try
            {
                if (Game.Current != null
                    && Game.Current.CurrentState == Game.State.Running
                    && GameStateManager.Current.ActiveState is MissionState
                    && Mission.Current != null
                    && Mission.Current.Mode == MissionMode.Battle
                    && Mission.Current.Agents.Count != 0) //Basically, after "fielding" the units
                {
                    if (enteredMission == false)
                    //if (true)
                    {
                        enteredMission = true;
                        Mission mission = Mission.Current;
                        //InformationManager.DisplayMessage(new InformationMessage("TEST Entered Mission"));
                        AssignBanners();
                    }
                }
                else if (enteredMission == true)
                {
                    enteredMission = false;
                }
            }
            catch (Exception e)
            {
                InformationManager.DisplayMessage(new InformationMessage("Error: " + e.Message));
            }
        }

        private void AssignBanners()
        {
            InformationManager.DisplayMessage(new InformationMessage("Assigning banners..."));

            List<CharacterObject> allowedBearerTypes = FilterAllowedBearerTypes();

            GiveBannersInTeam(Mission.Current.AttackerTeam, allowedBearerTypes);
            GiveBannersInTeam(Mission.Current.DefenderTeam, allowedBearerTypes);
        }

        private List<CharacterObject> FilterAllowedBearerTypes()
        {
            //Must use soldier types (IsSoldier)
            //Useful Occupations: Soldier, Bandit, Mercenary
            List<CharacterObject> characterTypes = new List<CharacterObject>();
            MBObjectManager.Instance.GetAllInstancesOfObjectType<CharacterObject>(ref characterTypes);

            /* Add types to a list of allowed troops to carry a banner */
            List<CharacterObject> allowedBearerTypes = new List<CharacterObject>();

            /* Add troops */
            if (allowBandits) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit)); }
            if (allowSoldiers) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Soldier)); }
            if (allowCaravanGuards) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.CaravanGuard)); }
            if (allowMercenaries) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Mercenary)); }

            /* Filter by formation */
            allowedBearerTypes = allowedBearerTypes
                .Where(t => (allowInfantry && !t.IsArcher && !t.IsMounted)
                    || (allowMounted && !t.IsArcher && t.IsMounted)
                    || (allowRanged && t.IsArcher && !t.IsMounted)
                    || (allowMountedRanged && t.IsArcher && t.IsMounted))
                .ToList();

            /* Filter by tier */
            if (filterTiers)
            {
                allowedBearerTypes = allowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (allowPlayer) { allowedBearerTypes.Add(characterTypes.Find(character => character.IsPlayerCharacter)); }
            if (allowCompanions) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.IsHero && character.Occupation == Occupation.Wanderer)); }
            if (allowNobles) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => !character.IsPlayerCharacter && (character.Occupation == Occupation.Lord || character.Occupation == Occupation.Lady))); }

            return allowedBearerTypes;
        }

        private void GiveBannersInTeam(Team team, List<CharacterObject> allowedBearerTypes)
        {
            Dictionary<CharacterObject, List<Agent>> teamTroopMap = team.TeamAgents
                .GroupBy(ta => (CharacterObject)ta.Character)
                .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            HashSet<CharacterObject> presentAllowedTypes = allowedBearerTypes.Where(type => teamTroopMap.ContainsKey(type)).ToHashSet();

            Dictionary<CharacterObject, List<Agent>> eligibleTroopMap = teamTroopMap
                .Where(kv => presentAllowedTypes.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            int bannersGiven = 0;
            foreach (KeyValuePair<CharacterObject, List<Agent>> entry in eligibleTroopMap)
            {

                if (entry.Key.IsHero || entry.Value.Count >= minTroopTypeAmount)
                {
                    int ratioCount = 0;
                    foreach (Agent agent in entry.Value)
                    {
                        if (ratioCount % bearerToTroopRatio == 0)
                        {
                            MissionWeapon bannerWeapon = new MissionWeapon(MBObjectManager.Instance.GetObject<ItemObject>("campaign_banner_small"), agent.Team.Banner);
                            agent.EquipWeaponToExtraSlotAndWield(ref bannerWeapon);
                            bannersGiven++;
                        }
                        ratioCount++;
                    }
                }
            }
            InformationManager.DisplayMessage(new InformationMessage(bannersGiven + " banners given to " + team.Leader.Name + "'s party"));
        }

    }
}

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
            if (BMBSettings.Instance.AllowSoldiers) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Soldier)); }
            if (BMBSettings.Instance.AllowCaravanGuards) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.CaravanGuard)); }
            if (BMBSettings.Instance.AllowMercenaries) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Mercenary)); }
            if (BMBSettings.Instance.AllowBandits) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.Occupation == Occupation.Bandit)); }

            /* Filter by formation */
            allowedBearerTypes = allowedBearerTypes
                .Where(t => (BMBSettings.Instance.AllowInfantry && !t.IsArcher && !t.IsMounted)
                    || (BMBSettings.Instance.AllowMounted && !t.IsArcher && t.IsMounted)
                    || (BMBSettings.Instance.AllowRanged && t.IsArcher && !t.IsMounted)
                    || (BMBSettings.Instance.AllowMountedRanged && t.IsArcher && t.IsMounted))
                .ToList();

            /* Filter by tier */
            if (BMBSettings.Instance.FilterTiers)
            {
                List<int> allowedTiers = new List<int>();
                if (BMBSettings.Instance.AllowTier1) allowedTiers.Add(1);
                if (BMBSettings.Instance.AllowTier2) allowedTiers.Add(2);
                if (BMBSettings.Instance.AllowTier3) allowedTiers.Add(3);
                if (BMBSettings.Instance.AllowTier4) allowedTiers.Add(4);
                if (BMBSettings.Instance.AllowTier5) allowedTiers.Add(5);
                if (BMBSettings.Instance.AllowTier6) allowedTiers.Add(6);
                if (BMBSettings.Instance.AllowTier7Plus) allowedTiers.AddRange(new List<int>() { 7, 8, 9, 10, 11, 12, 13, 14 }); //This'll do for now
                allowedBearerTypes = allowedBearerTypes
                    .Where(t => allowedTiers.Contains(t.Tier))
                    .ToList();
            }

            /* Add heroes */
            if (BMBSettings.Instance.AllowPlayer) { allowedBearerTypes.Add(characterTypes.Find(character => character.IsPlayerCharacter)); }
            if (BMBSettings.Instance.AllowCompanions) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => character.IsHero && character.Occupation == Occupation.Wanderer)); }
            if (BMBSettings.Instance.AllowNobles) { allowedBearerTypes.AddRange(characterTypes.FindAll(character => !character.IsPlayerCharacter && (character.Occupation == Occupation.Lord || character.Occupation == Occupation.Lady))); }

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

                if (entry.Key.IsHero || entry.Value.Count >= BMBSettings.Instance.MinTroopTypeAmount)
                {
                    int ratioCount = 0;
                    foreach (Agent agent in entry.Value)
                    {
                        if (ratioCount % BMBSettings.Instance.BearerToTroopRatio == 0)
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

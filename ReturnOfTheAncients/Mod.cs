﻿namespace RotA
{
    using ArchitectsLibrary.API;
    using ArchitectsLibrary.Handlers;
    using ECCLibrary;
    using HarmonyLib;
    using QModManager.API.ModLoading;
    using Mono.AlienBaseSpawners;
    using Patches;
    using Prefabs;
    using Prefabs.AlienBase;
    using Prefabs.AlienBase.Teleporter;
    using Prefabs.Buildable;
    using Prefabs.Creatures;
    using Prefabs.Equipment;
    using Prefabs.Initializers;
    using Prefabs.Modules;
    using Prefabs.Placeable;
    using Prefabs.Signals;
    using SMLHelper.V2.Crafting;
    using SMLHelper.V2.Handlers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UWE;
    
    [QModCore]
    public static partial class Mod
    {
        public static AssetBundle assetBundle;

        public static SeamothElectricalDefenseMK2 electricalDefenseMk2;
        public static GameObject electricalDefensePrefab;
        public static ExosuitZapModule exosuitZapModule;
        public static ExosuitDashModule exosuitDashModule;
        public static SuperDecoy superDecoy;

        public static GargantuanJuvenile gargJuvenilePrefab;
        public static GargantuanVoid gargVoidPrefab;
        public static GargantuanBaby gargBabyPrefab;
        public static SkeletonGarg spookySkeletonGargPrefab;
        public static GargantuanEgg gargEgg;
        public static AquariumGuppy aquariumGuppy;

        public static GenericSignalPrefab signal_cragFieldBase;
        public static GenericSignalPrefab signal_sparseReefBase;
        public static GenericSignalPrefab signal_kooshZoneBase;
        public static GenericSignalPrefab signal_ruinedGuardian;
        public static GenericSignalPrefab signal_cache_bloodKelp;
        public static GenericSignalPrefab signal_cache_sparseReef;
        public static GenericSignalPrefab signal_cache_dunes;
        public static GenericSignalPrefab signal_cache_lostRiver;
        

        public static DataTerminalPrefab tertiaryOutpostTerminalGrassy;
        public static DataTerminalPrefab tertiaryOutpostTerminalSparseReef;
        public static DataTerminalPrefab tertiaryOutpostTerminalLostRiver;
        public static DataTerminalPrefab guardianTerminal;
        public static DataTerminalPrefab researchBaseTerminal;
        public static DataTerminalPrefab supplyCacheTerminal;
        public static DataTerminalPrefab kooshBaseTerminal;
        public static DataTerminalPrefab archElectricityTerminal;
        public static DataTerminalPrefab voidBaseTerminal;
        public static DataTerminalPrefab cachePingsTerminal;
        public static DataTerminalPrefab spamTerminal;
        public static DataTerminalPrefab eggRoomTerminal;
        public static DataTerminalPrefab warpCannonTerminal;
        public static DataTerminalPrefab precursorMasterTechTerminal;
        public static DataTerminalPrefab redTabletHolder;
        public static DataTerminalPrefab devSecretTerminal;

        public static OmegaCubePedestal buildableOmegaCubePedestal;
        
        public static OmegaCube omegaCube;

        public static AtmosphereVolumePrefab precursorAtmosphereVolume;

        public static AlienRelicPrefab ingotRelic;
        public static AlienRelicPrefab rifleRelic;
        public static AlienRelicPrefab bladeRelic;
        public static AlienRelicPrefab builderRelic;

        public static GargPoster gargPoster;

        public static WarpCannonPrefab warpCannon;

        public static TechType architectElectricityMasterTech;
        public static TechType warpMasterTech;

        /// <summary>
        /// this value is only used by this mod, please dont use it or it'll cause conflicts.
        /// </summary>
        internal static DamageType architectElect = (DamageType)259745135;

        /// <summary>
        /// this value is only used by this mod, please dont use it or it'll cause conflicts.
        /// </summary>
        internal static EcoTargetType superDecoyTargetType = (EcoTargetType)49013491;

        private const string assetBundleName = "projectancientsassets";
        
        private const string alienSignalName = "Alien Signal";

        public static Config config = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        private static Assembly myAssembly = Assembly.GetExecutingAssembly();

        public static string warpCannonSwitchFireModeCurrentlyWarpKey = "WarpCannonSwitchFireModeWarp";
        public static string warpCannonSwitchFireModeCurrentlyCreatureKey = "WarpCannonSwitchFireModeCreature";
        public static string warpCannonSwitchFireModeCurrentlyManipulateFirePrimaryKey = "WarpCannonSwitchFireModeManipulatePrimary";
        public static string warpCannonSwitchFireModeCurrentlyManipulateFireSecondaryKey = "WarpCannonSwitchFireModeManipulateSecond";
        public static string warpCannonNotEnoughPowerError = "WarpCannonNotEnoughPowerError";

        public static string omegaTerminalHoverText = "OmegaTerminalHoverText";
        public static string omegaTerminalInteract = "OmegaTerminalInteract";
        public static string omegaTerminalRegenerateCube = "OmegaCubeRegenerateCube";
        
        [QModPatch]
        public static void Patch()
        {
            assetBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(Assembly.GetExecutingAssembly(), assetBundleName);
            ECCAudio.RegisterClips(assetBundle);

            #region Resources
            omegaCube = new OmegaCube();
            omegaCube.Patch();
            DisplayCaseServices.WhitelistTechType(omegaCube.TechType);
            #endregion

            #region Static asset references
            CoroutineHost.StartCoroutine(LoadElectricalDefensePrefab());
            #endregion
            
            PatchLanguage();

            #region Tech
            architectElectricityMasterTech = TechTypeHandler.AddTechType("ArchitectElectricityMaster", "Ionic Pulse Technology", "Plasma-generating nanotechnology with defensive and offensive capabilities.", false);
            warpMasterTech = TechTypeHandler.AddTechType("WarpingMasterTech", "Handheld Warping Device", "An alien device that enables short-range teleportation.", false);
            #endregion

            #region Achievements
            AchievementServices.RegisterAchievement("CraftWarpCannon", "Hybrid Technology", assetBundle.LoadAsset<Sprite>("WarpCannon_Popup"), "This achievement is hidden.", "Obtained the Handheld Warping Device.", true);
            AchievementServices.RegisterAchievement("VisitVoidBase", "Structure over the Abyss", assetBundle.LoadAsset<Sprite>("Popup_Green"), "This achievement is hidden.", "Encountered the alien structure in the crater edge.", true);
            AchievementServices.RegisterAchievement("TouchBlackHole", "Do Not Touch", assetBundle.LoadAsset<Sprite>("Popup_Green"), "This achievement is hidden.", "Really...?", true);
            AchievementServices.RegisterAchievement("WarpFar", "Teleportation Master", assetBundle.LoadAsset<Sprite>("Warper_Popup"), "Warp 2000 meters.", "Warped 2000 meters.", true, 2000);
            AchievementServices.RegisterAchievement("DevSecretAchievement", "Unknown Architects", assetBundle.LoadAsset<Sprite>("Popup_Purple"), "This achievement is hidden.", "Found the Hallway of the Architects.", true);
            #endregion
            
            PatchModules();

            PatchCreatures();

            InitSpawns();
            
            PatchInitializers();

            PatchSignals();
            
            PatchEncys();

            PatchAlienBasePrefabs();

            PatchAlienTerminals();

            PatchTeleporters();

            PatchAlienBases();

            CraftDataHandler.SetItemSize(TechType.PrecursorKey_White, new Vector2int(1, 1));
            CraftDataHandler.AddToGroup(TechGroup.Personal, TechCategory.Equipment, TechType.PrecursorKey_Red);
            CraftDataHandler.AddToGroup(TechGroup.Personal, TechCategory.Equipment, TechType.PrecursorKey_White);
            CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, TechType.PrecursorKey_White, new string[] { "Personal", "Equipment" });
            CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, TechType.PrecursorKey_Red, new string[] { "Personal", "Equipment" });

            Harmony harmony = new Harmony($"ArchitectsOfTheUnknown_{myAssembly.GetName().Name}");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            FixMapModIfNeeded(harmony);
        }
        
        [QModPostPatch]
        public static void PostPatch()
        {
            //post patch because decorations mod does stuff to this recipe
            var redTabletTD = new TechData
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.PrecursorIonCrystal, 1),
                    new Ingredient(TechType.AluminumOxide, 2)
                }
            };
            CraftDataHandler.SetTechData(TechType.PrecursorKey_Red, redTabletTD);
            var whiteTabletTD = new TechData
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>
                {
                    new Ingredient(TechType.PrecursorIonCrystal, 1),
                    new Ingredient(TechType.Silver, 2)
                }
            };
            CraftDataHandler.SetTechData(TechType.PrecursorKey_White, whiteTabletTD);

            //This stuff uses Architects Library resources so it must be patched AFTER that library
            warpCannon = new WarpCannonPrefab();
            warpCannon.Patch();
            PrecursorFabricatorService.SubscribeToFabricator(warpCannon.TechType, PrecursorFabricatorTab.Equipment);
            DisplayCaseServices.WhitelistTechType(warpCannon.TechType);

            warpCannonTerminal = new DataTerminalPrefab("WarpCannonTerminal", ency_warpCannonTerminal, terminalClassId: DataTerminalPrefab.orangeTerminalCID, techToAnalyze: warpMasterTech);
            warpCannonTerminal.Patch();

            gargPoster = new GargPoster();
            gargPoster.Patch();
            KnownTechHandler.SetAnalysisTechEntry(gargPoster.TechType, new List<TechType>() { gargPoster.TechType });
            buildableOmegaCubePedestal = new OmegaCubePedestal();
            buildableOmegaCubePedestal.Patch();

            CraftDataHandler.SetTechData(TechType.RocketStage2, new TechData() { craftAmount = 1, Ingredients = new List<Ingredient>() { new Ingredient(TechType.PlasteelIngot, 1), new Ingredient(TechType.Sulphur, 4), new Ingredient(TechType.Kyanite, 4), new Ingredient(TechType.PrecursorIonPowerCell, 1), new Ingredient(omegaCube.TechType, 1) } });
        }

        static void PatchCreatures()
        {
            gargJuvenilePrefab = new GargantuanJuvenile("GargantuanJuvenile", "Gargantuan Leviathan Juvenile", "A titan-class lifeform. How did it get in your inventory?", assetBundle.LoadAsset<GameObject>("GargJuvenile_Prefab"), null);
            gargJuvenilePrefab.Patch();

            gargVoidPrefab = new GargantuanVoid("GargantuanVoid", "Gargantuan Leviathan", "A titan-class lifeform. Indigineous to the void.", assetBundle.LoadAsset<GameObject>("GargAdult_Prefab"), null);
            gargVoidPrefab.Patch();

            gargBabyPrefab = new GargantuanBaby("GargantuanBaby", "Gargantuan Baby", "A very young specimen, raised in containment. Playful.", assetBundle.LoadAsset<GameObject>("GargBaby_Prefab"), assetBundle.LoadAsset<Texture2D>("GargantuanBaby_Icon"));
            gargBabyPrefab.Patch();

            spookySkeletonGargPrefab = new SkeletonGarg("SkeletonGargantuan", "Gargantuan Skeleton", "Spooky.", assetBundle.LoadAsset<GameObject>("SkeletonGarg_Prefab"), null);
            spookySkeletonGargPrefab.Patch();

            gargEgg = new GargantuanEgg();
            gargEgg.Patch();

            aquariumGuppy = new AquariumGuppy("AquariumGuppy", "Unknown Fish", "An interesting fish.", assetBundle.LoadAsset<GameObject>("AquariumGuppy"), null);
            aquariumGuppy.Patch();
        }

        static void InitSpawns()
        {
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(1245, -40, -716), "GargBehindAurora", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(1450, -100, 180), "GargBehindAurora2", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(-1386, -117, 346), "GargDunesMid", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(-1244, -117, 989), "GargDunesMidUpper", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(-1122, -117, 1420), "GargDunesUpper", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(-1370, -60, -29f), "GargDunesLower", 400f));
            StaticCreatureSpawns.RegisterStaticSpawn(new StaticSpawn(gargJuvenilePrefab, new Vector3(1430, -300, 1547), "MountainsGarg", 400f));
        }

        static void PatchInitializers()
        {
            var expRoar = new ExplosionRoarInitializer();
            expRoar.Patch();

            var adultGargSpawner = new AdultGargSpawnerInitializer();
            adultGargSpawner.Patch();

            var gargSecretCommand = new SecretCommandInitializer();
            gargSecretCommand.Patch();

            var miscInitializers = new MiscInitializers();
            miscInitializers.Patch();
        }

        static void PatchModules()
        {
            electricalDefenseMk2 = new();
            electricalDefenseMk2.Patch();
            PrecursorFabricatorService.SubscribeToFabricator(electricalDefenseMk2.TechType, PrecursorFabricatorTab.UpgradeModules);

            exosuitZapModule = new();
            exosuitZapModule.Patch();
            PrecursorFabricatorService.SubscribeToFabricator(exosuitZapModule.TechType, PrecursorFabricatorTab.UpgradeModules);

            superDecoy = new();
            superDecoy.Patch();
            PrecursorFabricatorService.SubscribeToFabricator(superDecoy.TechType, PrecursorFabricatorTab.Devices);

            exosuitDashModule = new();
            exosuitDashModule.Patch();
            PrecursorFabricatorService.SubscribeToFabricator(exosuitDashModule.TechType, PrecursorFabricatorTab.UpgradeModules);
        }

        static void PatchSignals()
        {
            signal_cragFieldBase = new GenericSignalPrefab("OutpostCSignal", "Precursor_Symbol04", "Downloaded co-ordinates", alienSignalName, new Vector3(-11, -178, -1155), 3); //white tablet icon
            signal_cragFieldBase.Patch();

            signal_sparseReefBase = new GenericSignalPrefab("OutpostDSignal", "Precursor_Symbol01", "Downloaded co-ordinates", alienSignalName, new Vector3(-810f, -184f, -590f), 3); //red tablet icon
            signal_sparseReefBase.Patch();

            signal_kooshZoneBase = new GenericSignalPrefab("KooshZoneBaseSignal", "Precursor_Symbol05", "Downloaded co-ordinates", alienSignalName, new Vector3(1489, -420, 1337), 3, new(true, "KooshBaseSignalSubtitle", "KooshBaseEncounter", "Discover koosh zone base text.", Mod.assetBundle.LoadAsset<AudioClip>("GuardianEncounter"), 2f)); //purple tablet icon
            signal_kooshZoneBase.Patch();

            signal_ruinedGuardian = new GenericSignalPrefab("RuinedGuardianSignal", "RuinedGuardian_Ping", "Unidentified tracking chip", "Distress signal", new Vector3(367, -333, -1747), 0, new(true, "GuardianEncounterSubtitle", "GuardianEncounter", "This machine appears to have recently collapsed to the seafloor. Further research required.", Mod.assetBundle.LoadAsset<AudioClip>("GuardianEncounter"), 3f));
            signal_ruinedGuardian.Patch();

            signal_cache_bloodKelp = new GenericSignalPrefab("BloodKelpCacheSignal", "CacheSymbol1", "Blood Kelp Zone Sanctuary", alienSignalName + " (535m)", new Vector3(-554, -534, 1518), defaultColorIndex: 2);
            signal_cache_bloodKelp.Patch();

            signal_cache_sparseReef = new GenericSignalPrefab("SparseReefCacheSignal", "CacheSymbol2", "Deep Sparse Reef Sanctuary", alienSignalName + " (287m)", new Vector3(-929, -287, -760), defaultColorIndex: 1);
            signal_cache_sparseReef.Patch();

            signal_cache_dunes = new GenericSignalPrefab("DunesCacheSignal", "CacheSymbol3", "Dunes Sanctuary", alienSignalName + " (380m)", new Vector3(-1187, -378, 1130), defaultColorIndex: 4);
            signal_cache_dunes.Patch();

            signal_cache_lostRiver = new GenericSignalPrefab("LostRiverCacheSignal", "CacheSymbol4", "Lost River Laboratory Cache", alienSignalName + " (685m)", new Vector3(-1111, -685, -655), defaultColorIndex: 3);
            signal_cache_lostRiver.Patch();
        }

        static IEnumerator LoadElectricalDefensePrefab()
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.Seamoth);
            yield return task;
            GameObject seamoth = task.GetResult();
            electricalDefensePrefab = seamoth.GetComponent<SeaMoth>().seamothElectricalDefensePrefab;
        }

        static void FixMapModIfNeeded(Harmony harmony)
        {
            var type = Type.GetType("SubnauticaMap.PingMapIcon, SubnauticaMap", false, false);
            if (type != null)
            {
                var pingOriginal = AccessTools.Method(type, "Refresh");
                var pingPrefix = new HarmonyMethod(AccessTools.Method(typeof(PingMapIcon_Patch), "Prefix"));
                harmony.Patch(pingOriginal, pingPrefix);
            }
        }

        public static void ApplyAlienUpgradeMaterials(Renderer renderer)
        {
            renderer.material.SetTexture("_MainTex", Mod.assetBundle.LoadAsset<Texture2D>("alienupgrademodule_diffuse"));
            renderer.material.SetTexture("_SpecTex", Mod.assetBundle.LoadAsset<Texture2D>("alienupgrademodule_spec"));
            renderer.material.SetTexture("_Illum", Mod.assetBundle.LoadAsset<Texture2D>("alienupgrademodule_illum"));
        }

        static void PatchEncy(string key, string path, string title, string desc, string popupName = null, string encyImageName = null)
        {
            Sprite popup = null;
            if (!string.IsNullOrEmpty(popupName))
            {
                popup = assetBundle.LoadAsset<Sprite>(popupName);
            }
            Texture2D encyImg = null;
            if (!string.IsNullOrEmpty(encyImageName))
            {
                encyImg = assetBundle.LoadAsset<Texture2D>(encyImageName);
            }
            PDAEncyclopediaHandler.AddCustomEntry(new PDAEncyclopedia.EntryData()
            {
                key = key,
                path = path,
                nodes = path.Split('/'),
                popup = popup,
                image = encyImg
            });
            LanguageHandler.SetLanguageLine("Ency_" + key, title);
            LanguageHandler.SetLanguageLine("EncyDesc_" + key, desc);
        }

        static void MakeObjectScannable(TechType techType, string encyKey, float scanTime)
        {
            PDAHandler.AddCustomScannerEntry(new PDAScanner.EntryData()
            {
                key = techType,
                encyclopedia = encyKey,
                scanTime = scanTime,
                isFragment = false
            });
        }

        private const string alienSpamEncyText = "This data terminal consists primarily of text in several unknown languages. Partially translated text is displayed below:\n\nTransfer of \u2580\u2596\u2517\u259b\u2584\u2596 failed. Sector Zero study of \u259c\u259a\u2523 \u259c\u259a\u2517\u2523\u2517\u252b\u2513\u250f\u2513 terminated for \u259b\u2584\u2596\u2505\u2517\u2596.\n\n\u2523\u2517\u250f\u259b\u2584\u2596\u259c\u250f\u2523 \u259a \u2596\u259e\u2523\u2517\u2596\u2517\u2523.\n\nVaccine progress: Awaiting termination.\n\nEmperor Apparatus status: Functioning.\n\n\u2523\u2517\u2596\u2503\u2580\u259a\u2597\u250f\u250f\u2513. \u2596\u251b\u2580\u2517\u259e\u2503\u250f\u2584 distress \u2580\u2596\u2517\u259b\u2596\u259c\u259a\u2523 data \u2505\u2596\u2517\u2501\u2596 \u2596\u2513\u252b\u259e\u2523 \u259a \u259b\u2584\u2505\u2517\u2596 \u259a \u2596\u259e\u2523\u2517\u2596\u2517\u2523 \u259a\u251b\u2598\u259e\u2501\u2596\u2505 \u259e\u2523\u2517\u2596\u2517\u2523.\n\n'Architects of the \u259a\u251b\u2598\u259e' status: missing. \u2501\u2596\u2505.\n\n\u2580\u2596\u2517\u259b\u259a\u2523 \u259c\u259a\u2517 \u259c\u259a.\n\nSpecimen of the Ancients terminated. Error: termination failed.";
    }
}

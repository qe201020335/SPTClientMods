using Aki.Common.Utils;
using Aki.Reflection.Patching;
using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using EFT;
using System.Threading.Tasks;
using Aki.Core.Patches;
using AntiFog.Patches;
using HarmonyLib;

namespace AntiFog;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static Plugin Instance { get; private set; }
    internal static GameObject PluginPersistentObj { get; private set; }
    
    internal static AntiFog AntiFog { get; private set; }
    
    public static ConfigEntry<KeyboardShortcut> GraphicsToggle { get; private set; }
    public static ConfigEntry<float> NVGCustomGlobalFogIntensity { get; private set; }
    public static ConfigEntry<float> CustomGlobalFogIntensity { get; private set; }

    public static ConfigEntry<float> StreetsFogLevel { get; private set; }
    public static ConfigEntry<float> CustomsFogLevel { get; private set; }
    public static ConfigEntry<float> LighthouseFogLevel { get; private set; }
    public static ConfigEntry<float> InterchangeFogLevel { get; private set; }
    public static ConfigEntry<float> WoodsFogLevel { get; private set; }
    public static ConfigEntry<float> ReserveFogLevel { get; private set; }
    public static ConfigEntry<float> ShorelineFogLevel { get; private set; }


    private void Awake()
    {
        Logger.LogDebug("AntiFog Awake()");
        Instance = this;
        PluginPersistentObj = gameObject;
        AntiFog = PluginPersistentObj.AddComponent<AntiFog>();
        DontDestroyOnLoad(PluginPersistentObj);
    }

    private void Start()
    {
        string AmandsExperimental = "AmandsGraphics Experimental";
        string AmandsFeatures = "AmandsGraphics Features";

        GraphicsToggle = Config.Bind(AmandsFeatures, "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert),
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 610 }));
        NVGCustomGlobalFogIntensity = Config.Bind(AmandsExperimental, "NVG CustomGlobalFog Intensity", 0.5f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 100, IsAdvanced = true }));
        CustomGlobalFogIntensity = Config.Bind(AmandsFeatures, "CustomGlobalFog Intensity", 0.1f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 270, IsAdvanced = true }));

        StreetsFogLevel = Config.Bind("Streets", "Fog Level", -250.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
        CustomsFogLevel = Config.Bind("Customs", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));

        LighthouseFogLevel = Config.Bind("Lighthouse", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));
        InterchangeFogLevel = Config.Bind("Interchange", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));

        WoodsFogLevel = Config.Bind("Woods", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));

        ReserveFogLevel = Config.Bind("Reserve", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));

        ShorelineFogLevel = Config.Bind("Shoreline", "Fog Level", -100.0f,
            new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 160 }));

        new LocalPlayerPatch().Enable();
        new NightVisionPatch().Enable();
        new PrismEffectsPatch().Enable();
        
        Logger.LogInfo("AntiFog Loaded");
    }
}

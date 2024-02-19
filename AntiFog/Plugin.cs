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
        GraphicsToggle = Config.Bind("General", "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert));
        NVGCustomGlobalFogIntensity = Config.Bind("General", "NVG CustomGlobalFog Intensity", 0.05f);
        CustomGlobalFogIntensity = Config.Bind("General", "CustomGlobalFog Intensity", 0.01f);
        StreetsFogLevel = Config.Bind("Maps", "Streets Fog Level", -2500.0f);
        CustomsFogLevel = Config.Bind("Maps", "Customs Fog Level", -1000.0f);
        LighthouseFogLevel = Config.Bind("Maps", "Lighthouse Fog Level", -1000.0f);
        InterchangeFogLevel = Config.Bind("Maps", "Interchange Fog Level", -1000.0f);
        WoodsFogLevel = Config.Bind("Maps", "Woods Fog Level", -1000.0f);
        ReserveFogLevel = Config.Bind("Maps", "Reserve Fog Level", -1000.0f);
        ShorelineFogLevel = Config.Bind("Maps", "Shoreline Fog Level", -1000.0f);

        new LocalPlayerPatch().Enable();
        new NightVisionPatch().Enable();
        new PrismEffectsPatch().Enable();
        
        Logger.LogInfo("AntiFog Loaded");
    }
}

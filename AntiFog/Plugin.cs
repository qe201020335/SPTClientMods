using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using AntiFog.Patches;
using BepInEx.Logging;

namespace AntiFog;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static Plugin Instance { get; private set; }
    internal static GameObject PluginPersistentObj { get; private set; }
    
    internal static AntiFog AntiFog { get; private set; }
    
    internal static ManualLogSource Log => Instance.Logger;
    
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
    public static ConfigEntry<float> EverywhereElseFogLevel { get; private set; }


    private void Awake()
    {
        Logger.LogDebug("AntiFog Awake()");
        Instance = this;
        PluginPersistentObj = gameObject;
        DontDestroyOnLoad(PluginPersistentObj);

        GraphicsToggle = Config.Bind("General", "GraphicsToggle", new KeyboardShortcut(KeyCode.Insert));
        NVGCustomGlobalFogIntensity = Config.Bind("General", "NVG CustomGlobalFog Intensity", 0.5f);
        CustomGlobalFogIntensity = Config.Bind("General", "CustomGlobalFog Intensity", 0.1f);
        StreetsFogLevel = Config.Bind("Maps", "Streets Fog Level", -250.0f);
        CustomsFogLevel = Config.Bind("Maps", "Customs Fog Level", -100.0f);
        LighthouseFogLevel = Config.Bind("Maps", "Lighthouse Fog Level", -100.0f);
        InterchangeFogLevel = Config.Bind("Maps", "Interchange Fog Level", -100.0f);
        WoodsFogLevel = Config.Bind("Maps", "Woods Fog Level", -100.0f);
        ReserveFogLevel = Config.Bind("Maps", "Reserve Fog Level", -100.0f);
        ShorelineFogLevel = Config.Bind("Maps", "Shoreline Fog Level", -100.0f);
        EverywhereElseFogLevel = Config.Bind("Maps", "Everywhere Else Fog Level", -100.0f);
        
        AntiFog = PluginPersistentObj.AddComponent<AntiFog>();
        
        // new LocalPlayerPatch().Enable();
        new NightVisionPatch().Enable();
        new PrismEffectsPatch().Enable();
        
        Logger.LogInfo("AntiFog Loaded");
    }
}

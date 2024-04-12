using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BSG.CameraEffects;
using EFT.CameraControl;
#if SIT
using StayInTarkov;
#elif AKI
using Aki.Reflection.Patching;
#endif

namespace OpticTweaks;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    // internal static ConfigEntry<float> ThermalVisionDepthFade;
    
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        // ThermalVisionDepthFade = Config.Bind("ThermalVision", "DepthFade", 0.03f, "ThermalVision DepthFade");
        
        new OpticSightPatch().Enable();
        new ThermalVisionPatch().Enable();
        new NightVisionPatch().Enable();
    }
}

public class ThermalVisionPatch : ModulePatch
{
    
    private new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(ThermalVisionPatch));
    
    protected override MethodBase GetTargetMethod()
    {
        return typeof(ThermalVision)
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .First(x => x.GetParameters().Length == 1 && x.GetParameters()[0].Name == "on" && x.Name != "StartSwitch");
    }
    
    // [PatchPrefix]
    // private static void PatchPrefix(ThermalVision __instance, bool on)
    // {
    //     if (!on) return;
    // }

    [PatchPostfix]
    private static void PatchPostfix(ThermalVision __instance, bool on)
    {
        Logger.LogDebug("ThermalVision is toggled");
        // Tweak tv head wear parameters here
        
        if (__instance.IsFpsStuck && __instance.StuckFpsUtilities != null)
        {
            Logger.LogInfo($"ThermalVision FpsStuck [{__instance.StuckFpsUtilities.MinFramerate}, {__instance.StuckFpsUtilities.MaxFramerate}]");
        }
        
        __instance.IsFpsStuck = false;
        __instance.IsGlitch = false;
        __instance.IsNoisy = false;
        __instance.TextureMask.enabled = false;
        __instance.ThermalVisionUtilities.DepthFade = 0;
        __instance.IsPixelated = false;

    }
}

public class NightVisionPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(NightVision)
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .First(x => x.GetParameters().Length == 1 && x.GetParameters()[0].Name == "on" && x.Name != "StartSwitch");
    }

    [PatchPostfix]
    private static void PatchPostfix(NightVision __instance)
    {
        // Tweak nvg parameters here
        __instance.TextureMask.enabled = false;
        __instance.NoiseIntensity = 0f;
    }

}


public class OpticSightPatch : ModulePatch
{
    
    private new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(OpticSightPatch));
    
    protected override MethodBase GetTargetMethod()
    {
        return typeof(OpticSight).GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void PatchPostfix(OpticSight __instance)
    {
        // Tweak sight parameters here
        
        if (__instance.NightVision && __instance.NightVision.enabled)
        {
            __instance.NightVision.NoiseIntensity = 0f;
        }
        
        if (__instance.ThermalVision && __instance.ThermalVision.enabled)
        {
            if (__instance.ThermalVision.IsFpsStuck)
            {
                Logger.LogDebug(
                    $"OpticSight {__instance.name} with Thermal Awake, fps is [{__instance.ThermalVision.StuckFpsUtilities.MinFramerate}, {__instance.ThermalVision.StuckFpsUtilities.MaxFramerate}]");
            }
            
            __instance.ThermalVision.IsFpsStuck = false;
            __instance.ThermalVision.IsGlitch = false;
            __instance.ThermalVision.IsNoisy = false;
            __instance.ThermalVision.IsPixelated = false;
            __instance.ThermalVision.ThermalVisionUtilities.DepthFade = 0;
        }
    }
}
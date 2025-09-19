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
using SPT.Reflection.Patching;
#endif

namespace OpticTweaks;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
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
        return typeof(OpticComponentUpdater).GetMethod(nameof(OpticComponentUpdater.CopyComponentFromOptic), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void PatchPostfix(OpticComponentUpdater __instance, OpticSight opticSight)
    {
        Logger.LogDebug($"CopyComponentFromOptic from {opticSight.name}");
        
        // Tweak sight parameters here
        
        if (__instance.nightVision_0.enabled)
        {
            Logger.LogDebug("CopyComponentFromOptic with NightVision");
            __instance.nightVision_0.NoiseIntensity = 0f;
            __instance.nightVision_0.TextureMask = null;
        }
        
        if (__instance.thermalVision_0.enabled)
        {
            Logger.LogDebug("CopyComponentFromOptic with ThermalVision");
            if (__instance.thermalVision_0.IsFpsStuck)
            {
                Logger.LogDebug(
                    $"OpticSight {__instance.name} with Thermal Awake, fps is [{__instance.thermalVision_0.StuckFpsUtilities.MinFramerate}, {__instance.thermalVision_0.StuckFpsUtilities.MaxFramerate}]");
            }
            
            __instance.thermalVision_0.IsFpsStuck = false;
            __instance.thermalVision_0.IsGlitch = false;
            __instance.thermalVision_0.IsNoisy = false;
            __instance.thermalVision_0.IsPixelated = false;
            __instance.thermalVision_0.ThermalVisionUtilities.DepthFade = 0;

            Logger.LogDebug($"OpticSight ThermalVision clip plane was [{__instance.camera_0.nearClipPlane} - {__instance.camera_0.farClipPlane}]");
            __instance.camera_0.farClipPlane = 1000f;
        }
    }
}
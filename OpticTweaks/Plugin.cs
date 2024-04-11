using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
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
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        new OpticSightPatch().Enable();
    }
}


public class OpticSightPatch : ModulePatch
{
    
    private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(OpticSightPatch));
    
    protected override MethodBase GetTargetMethod()
    {
        return typeof(OpticSight).GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void PatchPostfix(OpticSight __instance)
    {
        if (__instance.ThermalVision && __instance.ThermalVision.enabled)
        {
            Logger.LogInfo(
                $"OpticSight {__instance.name} with Thermal Awake, fps was [{__instance.ThermalVision.StuckFpsUtilities.MinFramerate}, {__instance.ThermalVision.StuckFpsUtilities.MaxFramerate}]");
            __instance.ThermalVision.StuckFpsUtilities.MinFramerate = 144;
            __instance.ThermalVision.StuckFpsUtilities.MaxFramerate = 144;
        }
    }
}
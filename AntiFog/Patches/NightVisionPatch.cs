using System.Linq;
using System.Reflection;
using BSG.CameraEffects;
#if SIT
using StayInTarkov;
#elif AKI
using Aki.Reflection.Patching;
#endif

namespace AntiFog.Patches;

public class NightVisionPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(NightVision)
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .First(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "on" && x.Name != "StartSwitch");
    }
    
    [PatchPostfix]
    private static void PatchPostFix(ref NightVision __instance, bool on)
    {
        if (Plugin.AntiFog.IsActive && Plugin.AntiFog.NVG != on && AntiFog.FPSCameraNightVision != null)
        {
            Plugin.AntiFog.NVG = on;
        }
    }
}
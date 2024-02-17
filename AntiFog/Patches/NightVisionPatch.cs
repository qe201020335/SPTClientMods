using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;

namespace AntiFog.Patches;

public class NightVisionPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(BSG.CameraEffects.NightVision)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(x => x.GetParameters().Count() == 1 && x.GetParameters()[0].Name == "on" && x.Name != "StartSwitch");
    }
    
    [PatchPostfix]
    private static void PatchPostFix(ref BSG.CameraEffects.NightVision __instance, bool on)
    {
        if (Plugin.AntiFog.GraphicsMode && AntiFog.localPlayer != null && AntiFog.NVG != on && AntiFog.FPSCameraNightVision != null)
        {
            AntiFog.NVG = on;
            Plugin.AntiFog.UpdateAmandsGraphics();
        }
        AntiFog.NVG = on;
    }
}
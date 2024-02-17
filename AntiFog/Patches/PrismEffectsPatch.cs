using System.Reflection;
using System.Threading.Tasks;
using Aki.Reflection.Patching;

namespace AntiFog.Patches;

public class PrismEffectsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(PrismEffects).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public);
    }
    
    [PatchPostfix]
    private static void PatchPostFix(PrismEffects __instance)
    {
        if (__instance.gameObject.name == "FPS Camera")
        {
            Plugin.AntiFog.GraphicsMode = false;
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(100);
                Plugin.AntiFog.ActivateAmandsGraphics(__instance.gameObject, __instance);
            });
        }
    }
}
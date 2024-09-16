using System.Reflection;
using System.Threading.Tasks;
#if SIT
using StayInTarkov;
#elif AKI
using SPT.Reflection.Patching;
#endif

namespace AntiFog.Patches;

public class PrismEffectsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(PrismEffects).GetMethod("OnEnable", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }
    
    [PatchPostfix]
    private static void PatchPostFix(PrismEffects __instance)
    {
        if (__instance.gameObject.name == "FPS Camera")
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(100);
                Plugin.AntiFog.InitComponents(__instance.gameObject, __instance);
            });
        }
    }
}
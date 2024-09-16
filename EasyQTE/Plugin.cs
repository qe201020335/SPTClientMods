using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using EFT.Hideout;
#if SIT
using StayInTarkov;
#elif AKI
using SPT.Reflection.Patching;
#endif

namespace EasyQTE;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        new ShrinkingCirclePatch().Enable();
    }
}

public class ShrinkingCirclePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(ShrinkingCircleQTE)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single(info => info.GetParameters().Length == 1 &&
                            info.GetParameters()[0].ParameterType == typeof(bool) &&
                            info.ReturnType == typeof(Task));
    }

    [PatchPrefix]
    private static void PatchPreFix(ref bool success)
    {
        success = true;
    }
}
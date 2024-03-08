using System.Reflection;
using Aki.Reflection.Patching;
using BepInEx;

namespace YeetWatermark;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        new NoClientWatermarkPatch().Enable();
    }
}

public class NoClientWatermarkPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(Aki.Debugging.Patches.DebugLogoPatch).GetMethod("PatchPrefix", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
    }
        
    [PatchPrefix]
    private static bool PatchPreFix()
    {
        return false;  // skip it
    }
}
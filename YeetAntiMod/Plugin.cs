using System.Reflection;
using Aki.Reflection.Patching;
using BepInEx;

namespace YeetAntiMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        new AllowClientModsPatch().Enable();
    }
}

public class AllowClientModsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(Aki.Core.Patches.PreventClientModsPatch).GetMethod("CheckForNonWhitelistedPlugins", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
    }
        
    [PatchPrefix]
    private static bool PatchPreFix()
    {
        return false;  // skip it
    }
        
    // [PatchTranspiler]
    // private IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    // {
    //     Debug.LogError("Transpiling Anti-Mods");
    //     foreach (var instruction in instructions)
    //     {
    //         if (instruction.opcode == OpCodes.Throw)
    //         {
    //             yield return new CodeInstruction(OpCodes.Nop);
    //         }
    //         else
    //         {
    //             yield return instruction;
    //         }
    //     }
    // }
}
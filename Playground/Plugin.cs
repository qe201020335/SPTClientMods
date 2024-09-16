using System;
using System.Reflection;
using SPT.Reflection.Patching;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EFT;
using EFT.Vaulting.Debug;

namespace Playground;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Configuration.Create(Config);
        new GameStartPatch().Enable();
        GameStartPatch.OnGameStarted += GameStarted;
    }
    
    private void GameStarted(GameWorld gameWorld)
    {
        gameWorld.gameObject.AddComponent<InRaidManager>().Init(gameWorld);
    }
}

public class GameStartPatch : ModulePatch
{
    private new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(GameStartPatch));
    
    internal static event Action<GameWorld> OnGameStarted;

    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void Postfix(GameWorld __instance)
    {
        Logger.LogDebug("Game started!");
        var action = OnGameStarted;
        try
        {
            action?.Invoke(__instance);
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }
}
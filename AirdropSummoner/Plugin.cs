using System;
using System.Linq;
using BepInEx;
using Comfort.Common;
using EFT;
using EFT.Airdrop;
using EFT.Console.Core;
using EFT.UI;
namespace AirdropSummoner;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
    }

    private void Start()
    {
        ConsoleScreen.Processor.RegisterCommandGroup<Commands>();
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} commands registered!");
    }
}

internal class Commands
{
    [ConsoleCommand("summon_airdrop", "",null, "Summons an airdrop.")]
    public static void SummonAirdrop()
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        if (gameWorld == null)
        {
            ConsoleScreen.LogError("You are not in a raid!");
            return;
        }

        if (gameWorld is not ClientGameWorld clientWorld)
        {
            ConsoleScreen.LogError("You are not in a ClientGameWorld!");
            return;
        }

        var player = clientWorld.MainPlayer;
        clientWorld.ClientSynchronizableObjectLogicProcessor.ServerAirdropManager.FlareSuccessEventHandler(player.ProfileId, player.Position, "");
        ConsoleScreen.Log("Airdrop summoned!");
    }
}
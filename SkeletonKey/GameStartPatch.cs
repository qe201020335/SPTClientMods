using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;

#if SIT
using StayInTarkov;
#elif AKI
using SPT.Reflection.Patching;
#endif

namespace SkeletonKey;

public class GameStartPatch : ModulePatch
{
    private const string KeyId = "5938603e86f77435642354f4";

    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void UnlockThings()
    {
        var hasTheKey = Singleton<GameWorld>.Instance.MainPlayer.Profile.Inventory.Equipment.GetAllItems().Any(item => item.TemplateId == KeyId);
        if (!hasTheKey)
        {
            return;
        }
        
        var allDoors = Object.FindObjectsOfType<Door>().Cast<WorldInteractiveObject>(); // mechanical doors
        var allKeyContainers = Object.FindObjectsOfType<LootableContainer>().Cast<WorldInteractiveObject>(); // locked loot containers
        var allTrunks = Object.FindObjectsOfType<Trunk>().Cast<WorldInteractiveObject>(); // locked car trunks

#if SIT
        var allKeyCardDoors = Object.FindObjectsOfType<KeycardDoor>().Cast<WorldInteractiveObject>().ToHashSet(); // keycard doors
#elif AKI // 471 doesn't have ToHashSet
        var allKeyCardDoors = new HashSet<WorldInteractiveObject>(Object.FindObjectsOfType<KeycardDoor>()); // keycard doors 
#endif

        // filter out keycard doors
        var doors = allDoors.Concat(allKeyContainers).Concat(allTrunks).Where(door => !allKeyCardDoors.Contains(door));

        foreach (var door in doors)
        {
            if (!string.IsNullOrWhiteSpace(door.KeyId))
            {
                door.KeyId = KeyId;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT;
using EFT.Interactive;
using Aki.Reflection.Patching;
using UnityEngine;

namespace SkeletonKey;

public class GameStartPatch: ModulePatch
{
    
    private const string KeyId = "5938603e86f77435642354f4";  // dorm 206 key
    
    protected override MethodBase GetTargetMethod()
    {
        return typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.Instance);
    }

    [PatchPostfix]
    private static void UnlockThings()
    {
        var allDoors = Object.FindObjectsOfType<Door>().Cast<WorldInteractiveObject>(); // mechanical doors
        var allKeyContainers = Object.FindObjectsOfType<LootableContainer>().Cast<WorldInteractiveObject>(); // locked loot containers
        var allTrunks = Object.FindObjectsOfType<Trunk>().Cast<WorldInteractiveObject>(); // locked car trunks
        
        var allKeyCardDoors = Object.FindObjectsOfType<KeycardDoor>().Cast<WorldInteractiveObject>().ToHashSet(); // keycard doors
        
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
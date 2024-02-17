﻿using System.Reflection;
using System.Threading.Tasks;
using Aki.Reflection.Patching;
using EFT;

namespace AntiFog.Patches;

public class LocalPlayerPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(LocalPlayer).GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
    }
    
    [PatchPostfix]
    private static void PatchPostFix(ref Task<LocalPlayer> __result)
    {
        var localPlayer = __result.Result;
        if (localPlayer != null && localPlayer.IsYourPlayer)
        {
            AntiFog.localPlayer = localPlayer;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using BepInEx;
using BepInEx.Logging;
using EFT;
using EFT.Vaulting.Debug;
using HarmonyLib;
using UnityEngine;

namespace NoInertia;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        // // Get Types
        // var methods = PatchConstants.EftTypes
        //     .Where(type => type.IsClass)
        //     .Select(type => type.GetMethod("OnWeightUpdated", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        //     .Where(method => method != null)
        //     .ToList();
        //
        // if (methods.Count != 2)
        // {
        //     Logger.LogError("Found not 2 OnWeightUpdated methods, found " + methods.Count);
        //     return;
        // }
        //
        // PhysicalOnWeightUpdatedPatch.InertiaField = methods[0].DeclaringType!.GetField("Inertia", BindingFlags.Instance | BindingFlags.Public)!;
        //
        // foreach (var method in methods)
        // {
        //     new PhysicalOnWeightUpdatedPatch(method).Enable();
        // }
        
        new CalculateValuePatch().Enable();
    }
    
}

public class CalculateValuePatch : ModulePatch
{
    // private new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(CalculateValuePatch));
    
    protected override MethodBase GetTargetMethod()
    {
        return PatchConstants.EftTypes.Where(type => type.IsClass)
            .Select(type =>
            {
                var method = type.GetMethod("CalculateValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var result = method != null
                             && method.DeclaringType == type
                             && method.GetParameters().Length == 2
                             && method.GetParameters()[0].ParameterType == typeof(Vector3)
                             && method.GetParameters()[1].ParameterType == typeof(float);
                return result ? method : null;
            }).Where(method => method != null)
            .SingleCustom(method => true);
    }
    
    [PatchPostfix]
    private static void PatchPostfix(ref float __result)
    {
        Logger.LogDebug($"CalculateValue is called with result {__result}");
        __result = 0f;
    }
}


// public class PhysicalOnWeightUpdatedPatch : ModulePatch
// {
//     private new static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(PhysicalOnWeightUpdatedPatch));
//     
//     private readonly MethodBase _method;
//     public static FieldInfo InertiaField;
//     
//     internal PhysicalOnWeightUpdatedPatch(MethodBase method)
//     {
//         _method = method;
//     }
//     
//     protected override MethodBase GetTargetMethod()
//     {
//         return _method;
//     }
//     
//     [PatchTranspiler]
//     private static IEnumerable<CodeInstruction> PatchTranspiler(IEnumerable<CodeInstruction> instructions, object __instance)
//     {
//         foreach (var instruction in instructions)
//         {
//             if (instruction.opcode == OpCodes.Stfld && (instruction.operand as FieldInfo) == InertiaField)
//             {
//                 // Replace assignment with our own
//                 yield return new CodeInstruction(OpCodes.Ldarg_0, __instance);
//                 yield return new CodeInstruction(OpCodes.Call, typeof(PhysicalOnWeightUpdatedPatch).GetMethod(nameof(SetInertia), BindingFlags.NonPublic));
//                 continue;
//             }
//             
//             
//             // Do something with the instruction
//             yield return instruction;
//         }
//     }
//
//     private static void SetInertia(object instance)
//     {
//         try
//         {
//             InertiaField.SetValue(instance, 0f);
//         }
//         catch (Exception e)
//         {
//             Logger.LogError($"Failed to set inertia field: {e}");
//         }
//     }
// }
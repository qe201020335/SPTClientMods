using System.Reflection;
using System.Threading.Tasks;
using EFT;
#if SIT
using StayInTarkov;
#elif AKI
using Aki.Reflection.Patching;
#endif

namespace AntiFog.Patches;

// public class LocalPlayerPatch : ModulePatch
// {
//     protected override MethodBase GetTargetMethod()
//     {
//         return typeof(LocalPlayer).GetMethod("Create", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
//     }
//     
//     [PatchPostfix]
//     private static void PatchPostFix(ref Task<LocalPlayer> __result)
//     {
//         var localPlayer = __result.Result;
//         if (localPlayer != null && localPlayer.IsYourPlayer)
//         {
//             AntiFog.localPlayer = localPlayer;
//         }
//     }
// }
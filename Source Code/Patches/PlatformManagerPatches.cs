using HarmonyLib;

namespace ShiftAtMidnightMorePlayers.Patches;

[HarmonyPatch(typeof(PlatformManager))]
internal static class PlatformManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlatformManager.Awake))]
    private static void AwakePostfix()
    {
        Plugin.ApplyHardCap();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlatformManager.Init))]
    private static void InitPostfix()
    {
        Plugin.ApplyHardCap();
    }
}

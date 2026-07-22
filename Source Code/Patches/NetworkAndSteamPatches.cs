using System;
using Fusion;
using HarmonyLib;
using Steamworks;

namespace ShiftAtMidnightMorePlayers.Patches;

[HarmonyPatch(typeof(NetworkRunner))]
internal static class NetworkRunnerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(NetworkRunner.StartGame))]
    private static void StartGamePrefix(
        ref StartGameArgs args
    )
    {
        try
        {
            GameMode mode = args.GameMode;

            if (mode == GameMode.Client)
                return;

            Plugin.ForcePendingLobbyMax(
                "NetworkRunner.StartGame"
            );

            int count = Plugin.GetEffectiveLobbyMax();

            args.PlayerCount =
                new Il2CppSystem.Nullable<int>(count);

            Plugin.Logger.LogInfo(
                $"NetworkRunner.StartGame PlayerCount=" +
                $"{count} (mode={mode})"
            );
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "NetworkRunner.StartGame patch falhou: " +
                exception
            );
        }
    }
}

[HarmonyPatch(typeof(FusionNetworkManager))]
internal static class FusionNetworkManagerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(FusionNetworkManager.StartAsHost))]
    private static void StartAsHostPrefix()
    {
        Plugin.ForcePendingLobbyMax(
            "FusionNetworkManager.StartAsHost"
        );

        Plugin.Logger.LogInfo(
            "StartAsHost pending maxPlayers=" +
            Plugin.GetEffectiveLobbyMax()
        );
    }
}

[HarmonyPatch(typeof(PlatformManager))]
internal static class PlatformManagerHostPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlatformManager.HostLobby))]
    private static void HostLobbyPrefix()
    {
        Plugin.ForcePendingLobbyMax(
            "PlatformManager.HostLobby"
        );
    }
}

[HarmonyPatch(typeof(PlatformManager_Steam))]
internal static class PlatformManagerSteamHostPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlatformManager_Steam.HostLobby))]
    private static void HostLobbyPrefix()
    {
        Plugin.ForcePendingLobbyMax(
            "PlatformManager_Steam.HostLobby prefix"
        );
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlatformManager_Steam.HostLobby))]
    private static void HostLobbyPostfix()
    {
        Plugin.ForcePendingLobbyMax(
            "PlatformManager_Steam.HostLobby postfix"
        );
    }
}

[HarmonyPatch(typeof(LobbyUIManager))]
internal static class LobbyUIManagerHostPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(LobbyUIManager.HostLobby))]
    private static void HostLobbyPrefix()
    {
        Plugin.ForcePendingLobbyMax(
            "LobbyUIManager.HostLobby"
        );
    }
}

[HarmonyPatch(typeof(SteamMatchmaking))]
internal static class SteamMatchmakingPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamMatchmaking.CreateLobby))]
    private static void CreateLobbyPrefix(
        ref ELobbyType eLobbyType,
        ref int cMaxMembers
    )
    {
        int forced = Plugin.GetEffectiveLobbyMax();

        Plugin.Logger.LogInfo(
            "SteamMatchmaking.CreateLobby cMaxMembers " +
            $"{cMaxMembers} -> {forced}"
        );

        cMaxMembers = forced;

        Plugin.ForcePendingLobbyMax(
            "SteamMatchmaking.CreateLobby"
        );
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamMatchmaking.SetLobbyMemberLimit))]
    private static void SetLobbyMemberLimitPrefix(
        CSteamID steamIDLobby,
        ref int cMaxMembers
    )
    {
        int forced = Plugin.GetEffectiveLobbyMax();

        if (cMaxMembers == forced)
            return;

        Plugin.Logger.LogInfo(
            "SteamMatchmaking.SetLobbyMemberLimit " +
            $"{cMaxMembers} -> {forced}"
        );

        cMaxMembers = forced;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamMatchmaking.SetLobbyData))]
    private static void SetLobbyDataPrefix(
        CSteamID steamIDLobby,
        string pchKey,
        ref string pchValue
    )
    {
        try
        {
            string maxKey =
                PlatformManager_Steam._maxPlayersKey;

            if (
                string.IsNullOrEmpty(maxKey) ||
                pchKey == null
            )
            {
                return;
            }

            if (
                !string.Equals(
                    pchKey,
                    maxKey,
                    StringComparison.Ordinal
                )
            )
            {
                return;
            }

            string forced =
                Plugin.GetEffectiveLobbyMax().ToString();

            if (pchValue == forced)
                return;

            Plugin.Logger.LogInfo(
                $"Steam SetLobbyData {pchKey}: " +
                $"'{pchValue}' -> '{forced}'"
            );

            pchValue = forced;
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogWarning(
                "SetLobbyData patch falhou: " +
                exception.Message
            );
        }
    }
}

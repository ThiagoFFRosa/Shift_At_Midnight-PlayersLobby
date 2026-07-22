using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace ShiftAtMidnightMorePlayers;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Plugin : BasePlugin
{
    public const string PluginGuid = "dev.fuzs.shiftatmidnight.moreplayers";
    public const string PluginName = "Shift At Midnight More Players";
    public const string PluginVersion = "0.5.0";

    internal static ManualLogSource Logger;

    internal static readonly int[] AllowedPlayerCounts =
    {
        1,
        2,
        3,
        8,
        16
    };

    internal static int DesiredLobbyMax = 3;

    public const int HardMaxPlayers = 16;

    public override void Load()
    {
        Logger = Log;

        try
        {
            DesiredLobbyMax = 3;
            ApplyHardCap();

            Harmony.CreateAndPatchAll(
                typeof(Plugin).Assembly,
                PluginGuid
            );

            Log.LogInfo(
                $"{PluginName} v{PluginVersion} carregado! " +
                $"Opções: 1, 2, 3, 8 e 16."
            );
        }
        catch (Exception exception)
        {
            Log.LogError(
                $"Erro ao carregar o plugin:\n{exception}"
            );
        }
    }

    internal static void ApplyHardCap()
    {
        try
        {
            PlatformManager.s_MAXPLAYERS = HardMaxPlayers;

            Logger?.LogInfo(
                $"PlatformManager.s_MAXPLAYERS=" +
                $"{HardMaxPlayers}"
            );
        }
        catch (Exception exception)
        {
            Logger?.LogWarning(
                "Não foi possível alterar " +
                "PlatformManager.s_MAXPLAYERS ainda: " +
                exception.Message
            );
        }
    }

    internal static bool IsAllowedPlayerCount(int value)
    {
        for (int i = 0; i < AllowedPlayerCounts.Length; i++)
        {
            if (AllowedPlayerCounts[i] == value)
                return true;
        }

        return false;
    }

    internal static int ResolveLobbyMaxPlayers(int requested)
    {
        if (IsAllowedPlayerCount(requested))
            return requested;

        if (requested <= 1)
            return 1;

        if (requested <= 2)
            return 2;

        if (requested <= 3)
            return 3;

        if (requested <= 8)
            return 8;

        return 16;
    }

    internal static void SetDesiredLobbyMax(int requested)
    {
        DesiredLobbyMax = ResolveLobbyMaxPlayers(requested);
    }

    internal static void ForcePendingLobbyMax(string reason)
    {
        try
        {
            ApplyHardCap();

            int count = ResolveLobbyMaxPlayers(
                DesiredLobbyMax
            );

            DesiredLobbyMax = count;
            PendingLobbySettings.maxPlayers = count;
            PlatformManager.s_MAXPLAYERS = HardMaxPlayers;

            Logger?.LogInfo(
                $"ForcePendingLobbyMax={count} ({reason})"
            );
        }
        catch (Exception exception)
        {
            Logger?.LogError(
                $"ForcePendingLobbyMax falhou ({reason}): " +
                exception
            );
        }
    }

    internal static int GetEffectiveLobbyMax()
    {
        int desired = ResolveLobbyMaxPlayers(
            DesiredLobbyMax
        );

        try
        {
            int pending = PendingLobbySettings.maxPlayers;

            if (
                IsAllowedPlayerCount(pending) &&
                pending > desired
            )
            {
                return pending;
            }
        }
        catch
        {
            // O valor desejado continua sendo usado.
        }

        return desired;
    }
}

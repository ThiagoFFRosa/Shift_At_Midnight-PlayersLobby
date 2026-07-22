using System;
using HarmonyLib;
using TMPro;

namespace ShiftAtMidnightMorePlayers.Patches;

[HarmonyPatch(typeof(CreateLobbySettings))]
internal static class CreateLobbySettingsPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("OnEnable")]
    private static void OnEnablePostfix(
        CreateLobbySettings __instance
    )
    {
        try
        {
            Plugin.ApplyHardCap();
            RebuildMaxPlayersDropdown(__instance);
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "Patch de CreateLobbySettings.OnEnable " +
                $"falhou: {exception}"
            );
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("UpdateDropdownLanguage")]
    private static void UpdateDropdownLanguagePostfix(
        CreateLobbySettings __instance
    )
    {
        try
        {
            RebuildMaxPlayersDropdown(__instance);
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "Patch de UpdateDropdownLanguage falhou: " +
                exception
            );
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CreateLobbySettings.ChangeMaxPlayers))]
    private static bool ChangeMaxPlayersPrefix(
        CreateLobbySettings __instance,
        int players
    )
    {
        try
        {
            int resolved = ResolveFromDropdownOrArg(
                __instance,
                players
            );

            ApplyLobbyMax(__instance, resolved);

            Plugin.Logger.LogInfo(
                $"ChangeMaxPlayers: índice/valor={players}, " +
                $"resolvido={resolved}"
            );
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "Patch de ChangeMaxPlayers falhou: " +
                exception
            );
        }

        // Impede a lógica original de limitar novamente em 3.
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CreateLobbySettings.StartLobby))]
    private static void StartLobbyPrefix(
        CreateLobbySettings __instance
    )
    {
        try
        {
            int resolved = ResolveFromDropdownOrArg(
                __instance,
                __instance.maxPlayers
            );

            ApplyLobbyMax(__instance, resolved);

            Plugin.Logger.LogInfo(
                $"StartLobby prefix maxPlayers={resolved}"
            );
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "StartLobby prefix falhou: " +
                exception
            );
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CreateLobbySettings.StartLobby))]
    private static void StartLobbyPostfix(
        CreateLobbySettings __instance
    )
    {
        try
        {
            int resolved = ResolveFromDropdownOrArg(
                __instance,
                Plugin.DesiredLobbyMax
            );

            ApplyLobbyMax(__instance, resolved);
            Plugin.ForcePendingLobbyMax(
                "CreateLobbySettings.StartLobby postfix"
            );
        }
        catch (Exception exception)
        {
            Plugin.Logger.LogError(
                "StartLobby postfix falhou: " +
                exception
            );
        }
    }

    private static int ResolveFromDropdownOrArg(
        CreateLobbySettings instance,
        int players
    )
    {
        TMP_Dropdown dropdown =
            instance.maxPlayersDropdown;

        if (
            dropdown != null &&
            dropdown.options != null &&
            dropdown.options.Count > 0
        )
        {
            int index = dropdown.value;

            if (
                index >= 0 &&
                index < dropdown.options.Count
            )
            {
                string text =
                    dropdown.options[index].text;

                int fromText;

                if (
                    int.TryParse(text, out fromText) &&
                    Plugin.IsAllowedPlayerCount(fromText)
                )
                {
                    return fromText;
                }

                if (
                    index >= 0 &&
                    index < Plugin.AllowedPlayerCounts.Length
                )
                {
                    return Plugin.AllowedPlayerCounts[index];
                }
            }
        }

        return Plugin.ResolveLobbyMaxPlayers(players);
    }

    private static void ApplyLobbyMax(
        CreateLobbySettings instance,
        int maxPlayers
    )
    {
        Plugin.SetDesiredLobbyMax(maxPlayers);

        instance.maxPlayers = Plugin.DesiredLobbyMax;
        PendingLobbySettings.maxPlayers =
            Plugin.DesiredLobbyMax;
        PlatformManager.s_MAXPLAYERS =
            Plugin.HardMaxPlayers;
    }

    private static void RebuildMaxPlayersDropdown(
        CreateLobbySettings instance
    )
    {
        TMP_Dropdown dropdown =
            instance.maxPlayersDropdown;

        if (dropdown == null)
            return;

        int previous = instance.maxPlayers;

        if (!Plugin.IsAllowedPlayerCount(previous))
            previous = Plugin.DesiredLobbyMax;

        if (!Plugin.IsAllowedPlayerCount(previous))
            previous = 3;

        dropdown.ClearOptions();

        var labels =
            new Il2CppSystem.Collections.Generic.List<string>();

        for (
            int i = 0;
            i < Plugin.AllowedPlayerCounts.Length;
            i++
        )
        {
            labels.Add(
                Plugin.AllowedPlayerCounts[i].ToString()
            );
        }

        dropdown.AddOptions(labels);

        int selectedIndex = 2;

        for (
            int i = 0;
            i < Plugin.AllowedPlayerCounts.Length;
            i++
        )
        {
            if (Plugin.AllowedPlayerCounts[i] == previous)
            {
                selectedIndex = i;
                break;
            }
        }

        dropdown.SetValueWithoutNotify(selectedIndex);
        dropdown.RefreshShownValue();

        int selectedPlayers =
            Plugin.AllowedPlayerCounts[selectedIndex];

        ApplyLobbyMax(instance, selectedPlayers);

        Plugin.Logger.LogInfo(
            "Dropdown reconstruído: 1, 2, 3, 8 e 16. " +
            $"Selecionado={selectedPlayers}"
        );
    }
}

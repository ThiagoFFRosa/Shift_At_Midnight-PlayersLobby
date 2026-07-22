# Shift At Midnight More Players

A BepInEx IL2CPP mod that expands the multiplayer lobby size in **Shift At Midnight**.

The mod adds additional player-count options to the lobby menu and patches the limits used by Steam Matchmaking and Photon Fusion.

## Features

- Adds new lobby size options:
  - 1 player
  - 2 players
  - 3 players
  - 8 players
  - 16 players
- Updates the Steam lobby member limit.
- Updates the Photon Fusion player count.
- Keeps the original lobby creation interface.
- Automatically applies the selected player limit when creating a lobby.

## Current Status

This mod is currently in beta.

During testing, lobbies with up to 6 players have shown better stability.

Lobbies with more than 6 players may experience:

- Increased latency and lag
- Player synchronization problems
- Connection instability
- Voice chat issues
- Problems when loading or starting a match
- Other unexpected game behavior

8-player lobbies can be created, but stability is not guaranteed in every situation.

16-player lobbies are currently experimental and may cause errors or fail to start correctly.

Shift At Midnight was not originally designed to support multiplayer lobbies of this size. Some issues may be caused by limitations in the game's original systems rather than only by the mod.

We intend to continue developing the mod and, when possible, fix or reduce problems related to larger multiplayer lobbies in future updates.

## Requirements

- Shift At Midnight
- BepInEx 6 IL2CPP

## Installation

### Thunderstore Mod Manager

1. Install the mod through Thunderstore Mod Manager or r2modman.
2. Launch the game using the modded profile.

### Manual Installation

1. Install BepInEx 6 IL2CPP for Shift At Midnight.
2. Download the latest release of this mod.
3. Extract the downloaded ZIP into the Shift At Midnight installation folder.
4. Confirm that the DLL is located at:

`BepInEx/plugins/ShiftAtMidnightMorePlayers/ShiftAtMidnightMorePlayers.dll`

5. Launch the game.

## Usage

1. Open the multiplayer lobby creation screen.
2. Select the desired number of players.
3. Create the lobby normally.

The mod will apply the selected limit to the lobby settings, Steam Matchmaking and Photon Fusion.

## Multiplayer Compatibility

Testing is still ongoing to determine whether every player needs the mod installed or only the lobby host.

For the best compatibility during the beta, it is recommended that every player installs the same version of the mod.

Using the same mod version does not guarantee that large lobbies will be completely stable, since several systems in the game were originally designed around a smaller player limit.

## Known Issues

- Lobbies with more than 6 players may experience lag and instability.
- Players may experience synchronization or connection problems.
- Voice chat may become unstable with several connected players.
- The lobby interface may not display every player correctly.
- Spawn points may not properly support larger groups.
- Some gameplay systems may only recognize the original number of players.
- Starting or loading a match with a large group may cause errors.
- 8-player lobbies may not remain stable in every situation.
- 16-player lobbies are currently experimental.
- Game updates may change internal methods used by the mod.
- Different mod versions between players may cause connection problems.

We plan to investigate these problems and improve support for larger lobbies in future versions.

## Reporting Issues

When reporting a problem, please include:

- The selected player limit
- The number of players attempting to join
- Whether every player had the mod installed
- The mod version
- The game version
- The BepInEx `LogOutput.log` file
- A description of what happened before the error

Issues can be reported through the GitHub repository:

[Shift At Midnight More Players — GitHub](https://github.com/ThiagoFFRosa/Shift_At_Midnight-PlayersLobby)

## Developers

This mod was developed collaboratively by:

- [ThiagoFFRosa](https://github.com/ThiagoFFRosa)
- [pestiantal1](https://github.com/pestiantal1)

Both developers contributed to researching the game's multiplayer systems, implementing player-limit patches, testing the mod and improving compatibility with Steam Matchmaking and Photon Fusion.

## Disclaimer

This is an unofficial community-created mod.

It is not affiliated with, authorized by or endorsed by the developers or publishers of Shift At Midnight.

Shift At Midnight was not originally designed for multiplayer lobbies of this size. Larger lobbies may cause lag, instability, synchronization problems, connection failures or other unexpected behavior.

Use the mod at your own risk. Multiplayer modifications may cause errors or become incompatible after game updates.
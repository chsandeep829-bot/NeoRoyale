# NEOROYALE

NEOROYALE is an original third-person 3D battle-royale **prototype** written in Unity C#. It is a starting point for Android and Windows PC, not a finished or online multiplayer game.

## Included prototype systems

- Third-person player movement and follow camera
- Health and damage
- Hitscan weapon firing and reloading
- Simple enemy bot that follows and attacks the player
- Shrinking safe-zone damage
- Ammo and healing pickups
- Basic on-screen HUD

## Requirements

- Unity 2022.3 LTS or newer
- A 3D Core project
- For Android builds: Unity Android Build Support, Android SDK & NDK Tools, and OpenJDK

## Install

1. Create a new Unity **3D Core** project named `NeoRoyale`.
2. Copy the `Assets/Scripts` folder from this repository into the Unity project's `Assets` folder.
3. In **Edit → Project Settings → Player → Active Input Handling**, select **Both** or **Input Manager (Old)**, then restart Unity if prompted. These prototype controls use Unity's legacy `Input` API.
4. Create a plane for the ground and add colliders to the ground and cover objects.
5. Create a capsule named `Player`, tag it `Player`, and add `CharacterController`, `Health`, `PlayerController`, `WeaponController`, and `HUD`. Assign the Main Camera to the camera fields.
6. Create enemy capsules with colliders, `Health`, and `BotAI`. Set their `target` to the player transform (or leave it blank and let the bot find the Player tag).
7. Create an empty object named `SafeZone` and add `SafeZone`. Place it at the center of the playable area.
8. For pickups, create trigger-collider objects and add `Pickup`. Set the pickup type in the Inspector.
9. Press Play and test on PC first.

## Controls (prototype)

- WASD: move
- Mouse: look
- Left Shift: sprint
- Space: jump
- Left mouse button: fire
- R: reload
- Escape: release mouse cursor

## Android status

The scripts are a foundation only. Dedicated touch controls, mobile UI scaling, graphics/performance tuning, and device testing still need to be added before an Android release.

## Multiplayer status

Online matchmaking, networking, server-authoritative combat, anti-cheat, accounts, and persistence are **not included**. Do not treat this prototype as a production-ready online game.

## Project structure

```
Assets/
  Scripts/
    BotAI.cs
    Health.cs
    HUD.cs
    Pickup.cs
    PlayerController.cs
    SafeZone.cs
    WeaponController.cs
```

## License

Choose and add a license before distributing the project.
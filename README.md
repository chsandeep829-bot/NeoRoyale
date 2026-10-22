# NEOROYALE

NEOROYALE is an original third-person 3D battle-royale **prototype** written in Unity C#. It is a starting point for Android and Windows PC, not a finished or online multiplayer game.

## Included prototype systems

- Third-person player movement and follow camera
- Desktop keyboard/mouse controls
- Configurable mobile touch joystick, look area, fire, sprint, jump and reload controls
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
3. In **Edit → Project Settings → Player → Active Input Handling**, select **Both** or **Input Manager (Old)**, then restart Unity if prompted. These scripts use Unity's legacy `Input` API alongside UI EventSystem pointer events.
4. Create a ground plane and add colliders to the ground and cover objects.
5. Create a capsule named `Player`, tag it `Player`, and add `CharacterController`, `Health`, `PlayerController`, `WeaponController`, and `HUD`. Assign the Main Camera to the camera fields.
6. Create enemy capsules with colliders, `Health`, and `BotAI`. Set their `target` to the player transform (or leave it blank so the bot finds the Player tag).
7. Create an empty object named `SafeZone` and add `SafeZone`. Place it at the center of the playable area.
8. For pickups, create trigger-collider objects and add `Pickup`. Set the pickup type in the Inspector.
9. Press Play and test on PC first.

## Configure Android touch UI

1. Create a Canvas (Screen Space - Overlay) and ensure the scene has an EventSystem.
2. Create a UI Image on the lower-left side of the Canvas. Make it a circular joystick background, add a child Image as the handle, then add `VirtualJoystick` to the background and assign the handle field.
3. Create a transparent UI Image covering the right half of the screen. Add `TouchLookArea` to it. Keep this panel behind buttons in the Canvas hierarchy so the buttons receive their pointer events.
4. Create four UI buttons and add `TouchActionButton` to each. Set the Action Type to **Fire**, **Sprint**, **Jump**, and **Reload** respectively. Place Fire/Jump/Reload on the right and Sprint near movement controls.
5. Make sure the joystick and look panel have **Raycast Target** enabled; transparent Images can still receive pointer events. Keep button graphics above the look panel.
6. Set Canvas Scaler to **Scale With Screen Size** and choose a reference resolution such as 1920 × 1080 (landscape). Test touch areas and button sizes on the target device.

## Controls

### PC
- WASD: move
- Mouse: look
- Left Shift: sprint
- Space: jump
- Left mouse button: fire
- R: reload
- Escape: release mouse cursor

### Android
- Left virtual joystick: move
- Drag right-side look area: look
- Fire, Sprint, Jump, Reload UI buttons: corresponding actions

## Android status

Touch input scripts are now included, but the Canvas must be created and wired in the Unity Editor. Performance tuning, UI polish, graphics settings, and device testing are still required before release.

## Multiplayer status

Online matchmaking, networking, server-authoritative combat, anti-cheat, accounts, and persistence are **not included**. Do not treat this prototype as a production-ready online game.

## Project structure

```
Assets/Scripts/
  BotAI.cs
  Health.cs
  HUD.cs
  Pickup.cs
  PlayerController.cs
  SafeZone.cs
  TouchActionButton.cs
  TouchInput.cs
  TouchLookArea.cs
  VirtualJoystick.cs
  WeaponController.cs
```

## License

Choose and add a license before distributing the project.
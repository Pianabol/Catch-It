# Catch It!: Cell Defense Arcade

A polished, high-performance hypercasual arcade game developed with Unity. This project focuses on sophisticated "Game Feel" techniques, optimized object pooling for performance, and a robust, state-managed game architecture.

## Gameplay Showcase

<p align="center">
  <img src="https://github.com/user-attachments/assets/25fd5170-6cb8-4ab2-9e90-e9225e26dd5c" width="31%" alt="Gameplay Showcase 1" />
  <img src="https://github.com/user-attachments/assets/68911182-513e-4ba7-9a06-b6852da20e7e" width="31%" alt="Gameplay Showcase 2" />
  <img src="https://github.com/user-attachments/assets/e3c02b84-a464-4ddc-81b4-f12910cc518f" width="31%" alt="In-Game Environment" />
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/3dfd2869-ae42-4d8a-a733-4208abc73733" width="31%" alt="Main Menu" />
  <img src="https://github.com/user-attachments/assets/8ef81692-b725-4d31-a82f-6ac708f598f9" width="31%" alt="Level Complete" />
  <img src="https://github.com/user-attachments/assets/4aa780cb-4ba5-4bc2-830d-df40b8541e50" width="31%" alt="Editor View & Layers" />
</p>

## Technical Overview

### Game Architecture

| System | Description |
| :--- | :--- |
| Central State Machine | A finite state machine in `GameManager` manages the distinct game states (`MENU`, `GAME`, `LEVELCOMPLETE`, `GAMEOVER`). Systems like UI, Spawner, and Timer are decoupled and listen for state changes. |
| Object Pooling | A dedicated `PoolManager` is utilized for spawning viruses, friendly items, and visual effects, completely avoiding the performance overhead of `Instantiate` and `Destroy` calls in high-frequency scenarios. |
| Level Management | `LevelManager` handles dynamic level loading via prefabs and persistent player progression using `PlayerPrefs`, supporting multiple distinct levels. |
| UI & Transitions | A centralized `UIManager` controls all UI panels. `CanvasFader` provides smooth scene fade transitions between game states using `CanvasGroup` and `LeanTween`. |

### "Juice" and Game Feel

| Feature | Description |
| :--- | :--- |
| Dynamic UI | UI buttons feature a continuous "Idle Breathing" effect. Button presses trigger a "Punch/Swell" animation (LeanTween) for immediate visual feedback. Depleted power-up buttons provide negative feedback (Visual Shake). |
| Organic Audio | Virus squish sound effects are randomized in pitch (`0.85f` to `1.15f`) in the `AudioManager` for an organic feel and varying impact. Dedicated audio feedback exists for all power-up activations. |
| Aesthetic Design | Kenarlardaki pastel pembe gradyan dokusu, oyuncuya mikroskop altında bir damar içindeymiş hissi vererek temayı güçlendirir. |

## Credits & References

| Asset Type | Source | Creator / Platform |
| :--- | :--- | :--- |
| Background Music | [Electronic Stealthy Spy...](https://pixabay.com/music/electronic-stealthy-spy-background-music-with-soft-snare-drum-rolls-526348/) | Pixabay |
| Virus Squish SFX | [Cartoon Splat](https://pixabay.com/sound-effects/film-special-effects-cartoon-splat-6086/) / [Squirt](https://pixabay.com/sound-effects/film-special-effects-squirt-86215/) | Pixabay |
| Penalty SFX | [Friendly Item Penalty](https://freesound.org/people/duskstep/sounds/160868/) | Freesound / duskstep |
| Game Over SFX | [Game Over Screen](https://freesound.org/people/TaranP/sounds/362206/) | Freesound / TaranP |
| Win SFX | [Win Screen](https://freesound.org/people/guillermochicasonido/sounds/691655/) | Freesound / guillermochicasonido |
| Power-up SFX | [Shield Power-Up](https://freesound.org/people/LilMati/sounds/523655/) | Freesound / LilMati |

<br />

| Unity Asset | Asset Store Link |
| :--- | :--- |
| Hyper-Casual UI Pack | [Asset Store](https://assetstore.unity.com/packages/2d/gui/hyper-casual-ui-pack-375832) |
| Viruses Free Edition 3D | [Asset Store](https://assetstore.unity.com/packages/3d/characters/viruses-free-edition-3d-273462) |
| Sci-Fi Pistol 1 | [Asset Store](https://assetstore.unity.com/packages/3d/props/guns/sci-fi-pistol-1-141442) |
| Simple Gems and Items | [Asset Store](https://assetstore.unity.com/packages/3d/props/simple-gems-and-items-ultimate-animated-customizable-pack-73764) |
| Cartoon FX Remaster | [Asset Store](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565) |
| LeanTween | [Asset Store](https://assetstore.unity.com/packages/tools/animation/leantween-3595) |

---
Developed by Furkan Tuç
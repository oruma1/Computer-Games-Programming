# Matatu Coin Rush — Unity C# Script Pack

Copy these `.cs` files into a Unity project under `Assets/Scripts/`.

## Scene setup

1. Create a 3D scene with a road or plane running along the Z axis.
2. Add your player object, such as a matatu model, near `(0, 0, -5)`.
3. Add `LaneRunnerPlayer` to the player.
4. Add a trigger collider to the player.
5. Create an empty object named `GameManager` and add `GameManager.cs`.
6. Create an empty object named `Spawner` and add `RoadSpawner.cs`.

## Prefabs

Create these prefabs and assign them on `RoadSpawner`:

- `coinPrefab`: a coin pickup with `Pickup`, set `type = Coin`, and a trigger collider.
- `factPowerUpPrefab`: a badge/power-up with `Pickup`, set `type = NairobiFact`, and a trigger collider.
- `obstaclePrefabs`: traffic cones, barriers, buses, potholes, or other Nairobi road hazards with `Obstacle` and trigger colliders.

Objects spawned by `RoadSpawner` automatically get `RunnerObject`, which moves them toward the player and destroys them when they pass behind the camera.

## UI setup

Use TextMeshPro text fields and assign them to `GameManager`:

- `scoreText`
- `coinText`
- `bestText`
- `factText`
- `boostText`
- `gameOverText`

Create a start panel and game-over panel. Add `StartButton` to any button helper object, then wire button `OnClick` events:

- Start button → `StartButton.StartRun()`
- Retry button → `StartButton.RestartRun()`

## Controls

- Desktop: Left/Right arrows or A/D.
- Mobile: swipe left or right.

## Gameplay notes

- Fact power-ups display a random Nairobi fact, protect the player briefly, and double coin value.
- Coin streaks raise score multipliers.
- Speed ramps up over time through `GameManager` balance values.
- Best score is saved with `PlayerPrefs`.

## Suggested Unity feel

Use a portrait camera looking down the road, colorful matatu art, warm Nairobi sunset lighting, and KICC/city silhouettes in the background.

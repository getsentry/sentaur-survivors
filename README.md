# Sentaur Survivors

Requires Unity 2022.3.7f1.

## Folders

### Assets

All your work lives here.

### Assets/Prefabs

These are prefab game objects from which in-game game objects are generated (anything that's duplicated). Enemies, Projectiles, etc. 

From inside the Unity Editor, you can drag these prefab objects into the scene (make sure you're in Scene view) and they'll become game objects in the game.

### Assets/Scenes

This is where the main scene files are located. Right now we just have a single scene, the `BattleScene` (where the player fights enemies).

### Assets/Scripts

All the component C# scripts (read: the code for the game).

## Tips

* Make sure you save your Scene in the Unity Editor (CMD + S) before committing code. If you don't, all the scene data (e.g. the game objects, their components, and those components' properties) won't get persisted.
* When you add a new Script component inside Unity, it gets placed in the top-level `Assets` folder. You have to manually move the newly-created file to `Assets/Scripts`.
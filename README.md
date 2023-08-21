# Sentaur Survivors

Requires Unity 2022.3.7f1.

## Coding Style

We mostly follow standard [Google's C# style guide](https://google.github.io/styleguide/csharp-style.html), because it's the most concise guide.

The key things to know:

* `PascalCase` for classes, methods, public fields, etc.
* `camelCase` for local variables and parameters
* `_camelCase` for private and protected class properties
* 4 space indentation (vs 2 in Google's guide)

Don't worry about being pedantic around whitespace rules, where braces go, method ordering, etc.

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

### Assets/Sprites

Sprites, art assets, etc.

### Assets/Sounds

Sound files.

## Tips

* Make sure you save your Scene in the Unity Editor (CMD + S) before committing code. If you don't, all the scene data (e.g. the game objects, their components, and those components' properties) won't get persisted.
* When you add a new Script component inside Unity, it gets placed in the top-level `Assets` folder. You have to manually move the newly-created file to `Assets/Scripts`.
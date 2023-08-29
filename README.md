# Sentaur Survivors

An Sentry-themed Vampire Survivors clone written for Unity in C# featuring:
* 3 weapons with their own unique behavior and upgrade paths
* 5 different enemies
* 3 tactical item pickups

![](https://github.com/getsentry/sentaur-survivors/blob/main/Media/gameplay.gif?raw=true)

## Dependencies

Requires Unity 2022.3.7f1.


## Contributing

### Coding Style

We mostly follow standard [Google's C# style guide](https://google.github.io/styleguide/csharp-style.html), because it's the most concise guide.

The key things to know:

* `PascalCase` for classes, methods, public fields, etc.
* `camelCase` for local variables and parameters
* `_camelCase` for private and protected class properties
* 4 space indentation (vs 2 in Google's guide)

Don't worry about being pedantic around whitespace rules, where braces go, method ordering, etc.

### Folders

#### Assets/Prefabs

These are prefab game objects from which in-game game objects are generated (anything that's duplicated). Enemies, Projectiles, etc. 

From inside the Unity Editor, you can drag these prefab objects into the scene (make sure you're in Scene view) and they'll become game objects in the game.

#### Assets/Scenes

This is where the main scene files are located. There are just two scenes:

* `TitleScene` - displays the title when the game launches
* `BattleScene` - the primary in-game scene (where the player fights enemies)

#### Assets/Scripts

All the component C# scripts (read: the code for the game).

#### Assets/Graphics

Sprites, art assets, tiles, materials, etc.

#### Assets/Sounds

Music and sound effects.

### Tips for Contributing

* Make sure you save your Scene in the Unity Editor (CMD + S) before committing code. If you don't, all the scene data (e.g. the game objects, their components, and those components' properties) won't get persisted.
* When you add a new Script component inside Unity, it gets placed in the top-level `Assets` folder. You have to manually move the newly-created file to `Assets/Scripts`.
* When renaming or moving files, move/rename the files inside Unity Editor, _not_ VS Code. Unity Editor will ensure any scripts that were attached to game objects also get renamed/mapped to the new location.
  * _If you perform the move/rename in VS Code, you'll have to manually add all the script references back inside Unity._

## Credits

Sentaur Survivors was originally developed in a single week as part of @getsentry's 2023 Hack Week event. The development team was:

* Michelle Fu ([@mifu67](https://github.com/mifu67)) - art + animation for title screen, player character, pickups, icons + programming
* Olivier Williams ([@olivier-w](https://github.com/olivier-w)) - UI design and sound effects + programming
* Isabella Enriquez ([@isabellaenriquez)](https://github.com/isabellaenriquez) - weapon and projectile systems
* Daniel Cardozo - art for level tileset, Sentaur art + animation
* Ben Vinegar ([@benvinegar](https://github.com/benvinegar)) - general game programming

## License

Game source code, art assets, and sound effects are licensed under Apache 2.0 (see LICENSE).

The in-game music track, ["37 ohmperios" by Rolemusic](https://freemusicarchive.org/music/Rolemusic/single/37-ohmperios/), is licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/).

DOTween is distributed under [DOTween's Artistic License](https://dotween.demigiant.com/license.php).

# Character Profile Management API (CPMAPI)
#### Originally Developed by Austin Renner (@austephner)

## Summary
This is a small and simple API which allows for quick and dirty random character profile generation in Unity. It can be extended to include additional randomization points. Content is maintained directly through a custom Unity Editor window. This API can be used in a multitude of different game genres and mechanics.

## Features
- Configure character details such as:
  - Attributes
  - Traits
  - Species
  - Classes
- Custom editor window
- Super stable serializable data structure
- Custom availability rules and trait negation
- GUID based system
- Extendable C# API
- Light weight

## Notes & Disclaimers
- Editor uses C# Reflection due to Unity's horrible handling of complex object serialization
  - This is more of a performance concern for editor serialization speed, it has no other negative implications
  - Serialization works completely fine, no data is lost when saving or editing
- Dev-Heavy, a lot of custom code is needed to actually use the randomly generated characters
- Character leveling / experience is not covered in this API
- Character abilities are no longer supported (this may come back in the future)

## Todo
- Complete documentation
- Improve extensibility
- Improve UI for character testing
- Improve custom editor window stability
- Settings
- C# `enum` and `class` generation tools

## Contributions
Push/pull requests are welcome but subject to code review!

## Licensing and Usage
This API is licensed under the MIT license. Consider supporting the developer(s) in your game's credits.

## Credits
- Austin Renner ([website](https://www.austephner.com))

## Getting Started
1. Create a new `GameObject` in the current scene and name it "CharacterProfileManager"
2. Add the `CharacterProfileManager` component
3. Use the Inspector or `Tools->Character Profile Manager` menu to open the editor window
4. Ensure that the `GameObject` from step 1 is assigned to the `CharacterProfileManager` field in the editor window
5. Begin creating/editing content. Don't forget to save your work!

## How To: Configure Stuff
### Classes
TODO

### Classes
TODO

### Attributes
TODO

### Traits
TODO

### Attribute Effectors (Traits)
TODO

### Availability Rules
TODO

### Trait Negations
TODO

## How To: Roll New Characters
The easiest way is to use the `CharacterProfileManager`'s non-static function. You are not limited to using this method, it just is the fastest/easiest to get things going.
```c#
var randomCharacter = CharacterProfileManager.Instance.RollNewCharacter();
```

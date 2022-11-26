# Character Generator

## Summary
This API creates random characters based on configured species, traits, attributes, etc.

![Example](https://i.imgur.com/YouUx3D.gif)

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

## Todo
- UI Rework
- Readme improvements for tutorials

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

## Usage Guides & Information
All tutorials assume you have the CPMAPI configuration window open.

### Classes
Classes are professions or roles given to characters. They can be made exclusive or unavailable to one to many species and other classes.

1. Select the "Classes" window mode
2. Click the "Add New Class" button
3. Give the class a name, icon, description, or availability rules

![Class Example](https://i.imgur.com/hh6kyD4.gif)

### Species
Species are the types of creatures that exist in the world that the character profile represents. They can have unique attributes, traits, classes, etc.

1. Select the "Species" window mode
2. Click the "Add New Species" button
3. Give the species a name, icon, description, base attributes, or availability rules

![Species Example](https://i.imgur.com/6KSRbip.gif)

### Attributes
Attributes are a major component to a character profile. They specify competency in certain aspects of the game. An example would be "Move Speed", "Accuracy", "Charisma", or "Arcane Power".

1. Select the "Attributes" window mode
2. Click the "Add New Attribute" button
3. Give the attribute a name, icon, description, or availability rules

![Attribute Example](https://i.imgur.com/vRDVtlQ.gif)

### Traits
Traits are profile modifications which affect attributes or other gameplay mechanics. An example would be a trait called "Smart" which positively affects an attribute named "Intelligence". For more complicated effects and impacts on mechanics, custom developer work is required to recognize when a profile has a certain GUID.

1. Select the "Traits" window mode
2. Click the "Add New Trait" button
3. Give the trait a name, icon, description, attribute effects, negations, or availability rules

![Trait Example](https://i.imgur.com/ZYgM2ef.gif)

#### Attribute Effects
These objects determine the actual effect a trait has on a character profile's attribute.

#### Trait Negations
These objects help prevent the random trait assignment system from adding two traits to a character's profile which contradict one another. For example, a "Smart" trait positively effects an "Intelligence" attribute while a "Dumb" trait negatively effects the same attribute. It would be contradicting for a character profile to have both the "Smart" and "Dumb" trait. When configured, the "Smart" trait could have the "Dumb" trait listed as a negation which would ultimately prevent the two from both occurring on the same character profile.

### Availability Rules
Availability rules help restrict certain classes, traits, or attributes to either be exclusive to or unavailable to another species or class. For example, a class called "G'Hork" may be designed to only appear for a species called "Orc" and not a species called "Human". The class's availability rules can be set to ensure the class is "Exclusive To" the given species "Orc".

### Name Builders
Name builders are object references that can be assigned to a species. They help "build a name" for the character given the implementation's rules. The CPMAPI comes with two by default which are accessible via the scene context menu.

#### Developing New Name Builders
1. Create a new C# class and inherit from `CharacterNameBuilder`
2. Implement `string CreateRandomName()` which should return a randomly generated charater's name
3. Create an instance of the new type within the Unity Editor and assign it to the species of your choice

#### Example Name Builder
This name builder chooses a random name from a predetermined list.
```c#
using System.Collections.Generic;
using CharacterProfileManagement.Utility;
using UnityEngine;

namespace CharacterProfileManagement.NameBuilding
{
    [CreateAssetMenu(menuName = "Custom Name Builder")]
    public class CustomNameBuilder : CharacterNameBuilder
    {
        public List<string> names;
        
        public override string CreateRandomName()
        {
            // the "Random()" function comes from a static extension in "CharacterProfileManagement.Utility"
            return firstNames.Random();
        }
    }
}
```

## How To: Roll New Characters
The easiest way is to use the `CharacterProfileManager`'s non-static function. You are not limited to using this method, it just is the fastest/easiest to get things going.
```c#
var randomCharacter = CharacterProfileManager.Instance.RollNewCharacter();
```

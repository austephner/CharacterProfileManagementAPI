using System;
using System.Collections.Generic;
using CharacterGenerator.NameBuilding;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class CharacterSpeciesConfiguration : CharacterDataConfiguration
    {
        public bool showAttributeEffectsInEditor;
        public CharacterNameBuilder nameBuilder;
        public int minTraitCount = 2, maxTraitCount = 5;
        public List<CharacterAttributeEffectConfiguration> baseAttributeEffects =
            new List<CharacterAttributeEffectConfiguration>();
    }
}
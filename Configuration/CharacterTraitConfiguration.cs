﻿using System;
using System.Collections.Generic;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public class CharacterTraitConfiguration : CharacterDataConfiguration
    {
        public bool showAttributeEffectsInEditor, showTraitNegationsInEditor;
        public List<string> negatedTraits = new List<string>();
        public List<CharacterAttributeEffectConfiguration> attributeEffects =
            new List<CharacterAttributeEffectConfiguration>();

        public bool CheckNegation(CharacterTraitConfiguration characterTraitConfiguration)
        {
            return negatedTraits.Contains(characterTraitConfiguration.guid);
        }
    }
}
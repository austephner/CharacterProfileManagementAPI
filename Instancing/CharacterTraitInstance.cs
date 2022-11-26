using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using Random = UnityEngine.Random;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class CharacterTraitInstance : CharacterDataInstance<CharacterTraitConfiguration>
    {
        public List<CharacterAttributeEffectInstance> attributeEffects = new List<CharacterAttributeEffectInstance>();

        public CharacterTraitInstance(CharacterTraitConfiguration characterTraitConfiguration)
        {
            configurationGuid = characterTraitConfiguration.guid;

            foreach (var attributeEffect in characterTraitConfiguration.attributeEffects)
            {
                attributeEffects.Add(new CharacterAttributeEffectInstance
                {
                    attributeGuid = attributeEffect.attributeGuid,
                    attributeEffectConfigurationGuid = attributeEffect.guid,
                    traitConfigurationGuid = configurationGuid,
                    effectValue = Random.Range(attributeEffect.minAttributeValue, attributeEffect.maxAttributeValue)
                });
            }
        }
    }
}
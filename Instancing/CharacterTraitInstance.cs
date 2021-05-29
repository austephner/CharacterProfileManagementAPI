using System;
using System.Collections.Generic;
using CharacterProfileManagement.Configuration;
using Random = UnityEngine.Random;

namespace CharacterProfileManagement.Instancing
{
    [Serializable]
    public class CharacterTraitInstance : CharacterDataInstance<CharacterTraitConfiguration>
    {
        public List<CharacterAttributeEffectInstance> attributeEffects = new List<CharacterAttributeEffectInstance>();

        public CharacterTraitInstance(CharacterTraitConfiguration characterTraitConfiguration)
        {
            configurationGuid = characterTraitConfiguration.guid;

            foreach (var attributeEffect in characterTraitConfiguration.attributeEffects)
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
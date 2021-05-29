using System;
using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    /// <summary>
    ///     Not to be confused with the <see cref="CharacterAttributeInstance" />, the effect-version of the class simply
    ///     holds a reference to an attribute configuration GUID and a calculated random effect value which must be used
    ///     in calculating a character's profile.
    /// </summary>
    [Serializable]
    public class CharacterAttributeEffectInstance
    {
        // todo: this code is entirely subject to change because it is incredibly inefficient. 

        public int effectValue;

        public string
            attributeGuid,
            attributeEffectConfigurationGuid,
            traitConfigurationGuid;

        public CharacterAttributeEffectConfiguration GetEffectConfiguration()
        {
            return CharacterProfileManager.Instance
                .GetConfiguration<CharacterTraitConfiguration>(traitConfigurationGuid)
                .GetAttributeEffectConfiguration(attributeEffectConfigurationGuid);
        }

        public CharacterAttributeConfiguration GetAttributeConfiguration()
        {
            return GetEffectConfiguration().GetAttributeConfiguration();
        }
    }
}
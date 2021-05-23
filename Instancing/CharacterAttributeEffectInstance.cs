using System;
using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    /// <summary>
    /// Not to be confused with the <see cref="CharacterAttributeInstance"/>, the effect-version of the class simply
    /// holds a reference to an attribute configuration GUID and a calculated random effect value which must be used
    /// in calculating a character's profile.
    /// </summary>
    [Serializable]
    public class CharacterAttributeEffectInstance : CharacterDataInstance<CharacterAttributeEffectConfiguration>
    {
        public int effectValue;
    }
}
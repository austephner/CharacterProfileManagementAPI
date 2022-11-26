using System;

namespace CharacterGenerator.Old.Configuration
{
    /// <summary>
    ///     A configuration owned by a <see cref="CharacterTraitConfiguration" /> which specifies the effect a trait has
    ///     on the given attribute.
    /// </summary>
    [Serializable]
    public class CharacterAttributeEffectConfiguration
    {
        public string guid = Guid.NewGuid().ToString(), attributeGuid;
        public int minAttributeValue, maxAttributeValue;
        public bool stackable;

        public CharacterAttributeConfiguration GetAttributeConfiguration()
        {
            return Behaviours.CharacterGeneratorBehaviour.instance.GetConfiguration<CharacterAttributeConfiguration>(attributeGuid);
        }
    }
}
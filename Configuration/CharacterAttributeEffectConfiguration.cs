using System;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public class CharacterAttributeEffectConfiguration : CharacterDataConfiguration
    {
        public string attributeGuid;
        public int minAttributeValue, maxAttributeValue;
        public bool stackable;
    }
}
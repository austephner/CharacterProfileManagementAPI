using System;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public class CharacterAttributeEffectConfiguration
    {
        public string attributeGuid;
        public int minAttributeValue, maxAttributeValue;
    }
}
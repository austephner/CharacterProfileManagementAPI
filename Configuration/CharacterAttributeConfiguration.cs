using System;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public class CharacterAttributeConfiguration : CharacterDataConfiguration
    {
        public bool stackable;
        public int minValue, maxValue;
    }
}
using System;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public class CharacterAttributeConfiguration : CharacterDataConfiguration
    {
        public int minValue, maxValue;
    }
}
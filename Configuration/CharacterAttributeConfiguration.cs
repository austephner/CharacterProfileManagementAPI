using System;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class CharacterAttributeConfiguration : CharacterDataConfiguration
    {
        public int minValue, maxValue;
    }
}
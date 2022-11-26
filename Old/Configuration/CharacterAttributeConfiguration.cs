using System;

namespace CharacterGenerator.Old.Configuration
{
    [Serializable]
    public class CharacterAttributeConfiguration : CharacterDataConfiguration
    {
        public int minValue, maxValue;
    }
}
using System;
using CharacterGenerator.Configuration;

namespace CharacterGenerator.DefaultCharacters.Modules.Attributes
{
    [Serializable]
    public class AttributeConfiguration : EntityConfiguration
    {
        public int 
            minLevel, 
            maxLevel,
            minStartingLevel,
            maxStartingLevel;
    }
}
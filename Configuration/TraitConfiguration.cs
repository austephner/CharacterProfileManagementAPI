using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.DefaultCharacters.Modules.Traits;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class TraitConfiguration : AvailabilityConfiguration
    {
        public List<AttributeAffectConfiguration> affectedAttributes = new List<AttributeAffectConfiguration>();

        public TraitInstance CreateRandomInstance(AttributeModule attributeModule)
        {
            return new TraitInstance()
            {
                guid = guid,
                name = name,
                affectedAttributes = affectedAttributes
                    .Select(a => a.CreateRandomInstance(attributeModule))
                    .ToList()
            };
        }
    }
}
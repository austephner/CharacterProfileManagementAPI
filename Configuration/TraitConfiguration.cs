using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.DefaultCharacters.Modules.Traits;
using CharacterGenerator.Instancing;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class TraitConfiguration : AvailabilityConfiguration, IRandomInstanceFactory<TraitInstance>
    {
        public List<AttributeAffectConfiguration> affectedAttributes = new List<AttributeAffectConfiguration>();

        public TraitInstance CreateRandomInstance()
        {
            return new TraitInstance()
            {
                guid = guid,
                affectedAttributes = affectedAttributes.Select(a => a.CreateRandomInstance()).ToList()
            };
        }
    }
}
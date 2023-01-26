using System;
using System.Collections.Generic;
using CharacterGenerator.Instancing;

namespace CharacterGenerator.DefaultCharacters.Modules.Traits
{
    [Serializable]
    public class TraitInstance : EntityInstance
    {
        public List<AttributeAffectInstance> affectedAttributes = new List<AttributeAffectInstance>();

        // public TraitConfiguration GetTraitConfiguration(CharacterGeneratorConfiguration configuration)
        // {
        //     return configuration.traits.FirstOrDefault(t => t.guid == guid);
        // }
    }
}
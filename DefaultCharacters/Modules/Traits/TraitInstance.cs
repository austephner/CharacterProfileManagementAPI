using System;
using System.Collections.Generic;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.Instancing;

namespace CharacterGenerator.DefaultCharacters.Modules.Traits
{
    [Serializable]
    public class TraitInstance : EntityInstance
    {
        public string name;
        
        public List<AttributeAffectInstance> affectedAttributes = new List<AttributeAffectInstance>();
    }
}
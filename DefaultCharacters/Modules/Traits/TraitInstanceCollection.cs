using System;
using System.Collections.Generic;
using System.Linq;

namespace CharacterGenerator.DefaultCharacters.Modules.Traits
{
    [Serializable]
    public class TraitInstanceCollection
    {
        public List<TraitInstance> traits = new List<TraitInstance>();

        public TraitInstanceCollection(List<TraitInstance> traits)
        {
            this.traits = traits.Clone();
        }
    }
}
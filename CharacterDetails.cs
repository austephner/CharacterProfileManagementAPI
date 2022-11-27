using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;

namespace CharacterGenerator
{
    /// <summary>
    /// Set of details that represents a character.
    /// </summary>
    [Serializable]
    public class CharacterDetails
    {
        public string name;

        public CharacterGeneratorConfiguration configuration = null;

        public PrimaryClass primaryClass = null;
        
        public Species species = null;

        public List<Trait> traits = new List<Trait>();

        public CharacterDetails()
        {
            
        }

        public CharacterDetails(CharacterGeneratorConfiguration configuration)
        {
            
        }
    }
}
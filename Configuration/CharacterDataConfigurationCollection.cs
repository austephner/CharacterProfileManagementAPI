using System;
using System.Collections.Generic;

namespace CharacterProfileManagement.Configuration
{
    [Serializable]
    public abstract class CharacterDataConfigurationCollection<T> where T : CharacterDataConfiguration
    {
        public List<T> items = new List<T>();
    }
    
    /* -------------------------------------------------------------------------------------------------------------- */
    
    [Serializable] public class CharacterTraitConfigurationCollection : CharacterDataConfigurationCollection<CharacterTraitConfiguration> { }
    [Serializable] public class CharacterClassConfigurationCollection : CharacterDataConfigurationCollection<CharacterClassConfiguration> { }
    [Serializable] public class CharacterSpeciesConfigurationCollection : CharacterDataConfigurationCollection<CharacterSpeciesConfiguration> { }
    [Serializable] public class CharacterAttributeConfigurationCollection : CharacterDataConfigurationCollection<CharacterAttributeConfiguration> { }
    
    /* -------------------------------------------------------------------------------------------------------------- */
}
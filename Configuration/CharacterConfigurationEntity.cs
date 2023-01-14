using System;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public abstract class CharacterConfigurationEntity
    {
        public string 
            name = "Species", 
            description = "", 
            guid = Guid.NewGuid().ToString();
    }
}
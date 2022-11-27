using System;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public abstract class CharacterConfigurationEntity
    {
        public string name = "";
        
        public string guid = Guid.NewGuid().ToString();
    }
}
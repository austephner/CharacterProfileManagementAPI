using System;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public abstract class EntityConfiguration
    {
        public string 
            name = "Entity", 
            description = "", 
            guid = Guid.NewGuid().ToString();

        public float rarity = 0.5f; 
        
#if UNITY_EDITOR
        public bool expandedInEditor;
#endif
    }
}
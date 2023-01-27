using System;
using CharacterGenerator.Instancing;

namespace CharacterGenerator.DefaultCharacters.Modules.Attributes
{
    [Serializable]
    public class AttributeInstance : EntityInstance
    {
        public string name;
        
        public int level;
    }
}
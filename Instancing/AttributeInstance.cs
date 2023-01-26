using System;
using System.Linq;
using CharacterGenerator.Configuration;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class AttributeInstance : EntityInstance
    {
        public int level;

        // public AttributeConfiguration GetAttributeConfiguration(CharacterGeneratorConfiguration configuration)
        // {
        //     return configuration.attributes.FirstOrDefault(a => a.guid == guid);
        // }
    }
}
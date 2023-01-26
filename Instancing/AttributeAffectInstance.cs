using System;
using System.Linq;
using CharacterGenerator.Configuration;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class AttributeAffectInstance
    {
        public string attributeGuid;

        public float affectAmount;

        // public AttributeConfiguration GetAttribute(CharacterGeneratorConfiguration configuration)
        // {
        //     return configuration.attributes.FirstOrDefault(a => a.guid == attributeGuid);
        // }
    }
}
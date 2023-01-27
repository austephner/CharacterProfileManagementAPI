using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace CharacterGenerator.DefaultCharacters.Modules.Attributes
{
    [Serializable]
    public class AttributeAffectConfiguration
    {
        public string attributeGuid;

        public float minAffect, maxAffect;

        public AttributeAffectInstance CreateRandomInstance(AttributeModule attributeModule)
        {
            return new AttributeAffectInstance()
            {
                attributeName = attributeModule.attributes.FirstOrDefault(a => a.guid == attributeGuid)?.name ?? "Unnamed Attribute",
                affectAmount = Random.Range(minAffect, maxAffect + 1),
                attributeGuid = attributeGuid
            };
        }
    }
}
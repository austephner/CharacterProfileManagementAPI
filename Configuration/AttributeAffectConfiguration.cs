using System;
using CharacterGenerator.Instancing;
using Random = UnityEngine.Random;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class AttributeAffectConfiguration : IRandomInstanceFactory<AttributeAffectInstance>
    {
        public string attributeGuid;

        public float minAffect, maxAffect;

        public AttributeAffectInstance CreateRandomInstance()
        {
            return new AttributeAffectInstance()
            {
                affectAmount = Random.Range(minAffect, maxAffect),
                attributeGuid = attributeGuid
            };
        }
    }
}
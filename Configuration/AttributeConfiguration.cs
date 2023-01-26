using System;
using CharacterGenerator.Instancing;
using Random = UnityEngine.Random;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class AttributeConfiguration : EntityConfiguration, IRandomInstanceFactory<AttributeInstance>
    {
        public int min, max;

        public AttributeInstance CreateRandomInstance()
        {
            return new AttributeInstance()
            {
                guid = guid,
                level = Random.Range(min, max + 1)
            };
        }
    }
}
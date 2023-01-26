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
    }
}
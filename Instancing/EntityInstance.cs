using System;
using UnityEngine.Serialization;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class EntityInstance
    {
        [FormerlySerializedAs("guid")]
        public string instanceGuid = Guid.NewGuid().ToString();

        public string configurationGuid;
    }
}
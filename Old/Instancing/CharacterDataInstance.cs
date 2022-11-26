using System;
using CharacterGenerator.Old.Configuration;

namespace CharacterGenerator.Old.Instancing
{
    [Serializable]
    public abstract class CharacterDataInstance<T> where T : CharacterDataConfiguration
    {
        public string guid = Guid.NewGuid().ToString();
        public string configurationGuid;

        public T GetConfiguration() => Behaviours.CharacterGeneratorBehaviour.instance.GetConfiguration<T>(configurationGuid);
    }
}
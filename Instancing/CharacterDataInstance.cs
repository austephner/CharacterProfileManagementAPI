using System;
using CharacterGenerator.Configuration;
using CharacterGenerator.Behaviours;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public abstract class CharacterDataInstance<T> where T : CharacterDataConfiguration
    {
        public string guid = Guid.NewGuid().ToString();
        public string configurationGuid;

        public T GetConfiguration() => Behaviours.CharacterGeneratorBehaviour.instance.GetConfiguration<T>(configurationGuid);
    }
}
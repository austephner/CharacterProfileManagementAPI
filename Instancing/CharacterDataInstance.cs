using System;
using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    [Serializable]
    public abstract class CharacterDataInstance<T> where T : CharacterDataConfiguration
    {
        public string guid = Guid.NewGuid().ToString();
        public string configurationGuid;

        public T GetConfiguration() => CharacterProfileManager.Instance.GetConfiguration<T>(configurationGuid);
    }
}
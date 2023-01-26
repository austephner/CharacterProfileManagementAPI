using System;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class CharacterData : Stringtionary
    {
        public string guid = Guid.NewGuid().ToString();
    }
}
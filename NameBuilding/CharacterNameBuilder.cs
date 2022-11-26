using UnityEngine;

namespace CharacterGenerator.NameBuilding
{
    public abstract class CharacterNameBuilder : ScriptableObject
    {
        public abstract string CreateRandomName();
    }
}
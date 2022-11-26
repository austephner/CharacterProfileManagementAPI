using UnityEngine;

namespace CharacterGenerator.Old.NameBuilding
{
    public abstract class CharacterNameBuilder : ScriptableObject
    {
        public abstract string CreateRandomName();
    }
}
using UnityEngine;

namespace CharacterProfileManagement.NameBuilding
{
    public abstract class CharacterNameBuilder : ScriptableObject
    {
        public abstract string CreateRandomName();
    }
}
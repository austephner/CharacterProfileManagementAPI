using UnityEngine;

namespace CharacterGenerator.Configuration
{
    public abstract class NameBuilder : ScriptableObject
    {
        public abstract string GenerateName();
    }
}
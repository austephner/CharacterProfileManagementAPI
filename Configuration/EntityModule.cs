using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.Configuration
{
    public abstract partial class EntityModule: ScriptableObject
    {
        public virtual string displayName { get; protected set; } = "New Module";
        
        public abstract void Generate(CharacterData characterData, uint? seed = null);

        public virtual void HandleGuidChange(string oldGuid, string newGuid) { }
    }
}
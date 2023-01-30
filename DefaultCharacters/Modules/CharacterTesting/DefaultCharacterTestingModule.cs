using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.DefaultCharacters.Modules.CharacterTesting
{
    [CreateAssetMenu(menuName = "Character Generator/Default Modules/Default Testing")]
    public class DefaultCharacterTestingModule : EntityModule
    {
        public override string displayName => "Default Testing";
        
        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            
        }
    }
}
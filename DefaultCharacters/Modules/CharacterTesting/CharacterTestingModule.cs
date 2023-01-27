using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.DefaultCharacters.Modules.CharacterTesting
{
    [CreateAssetMenu(menuName = "Character Generator/Default Modules/Testing")]
    public class CharacterTestingModule : EntityModule
    {
        public override string displayName => "Testing";
        
        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            
        }
    }
}
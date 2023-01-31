using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.DefaultCharacters.Modules
{
    [CreateAssetMenu(menuName = "Test Module")]
    public class TestModule : EntityModule
    {
        public string testField = "";
        
        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            
        }
    }
}
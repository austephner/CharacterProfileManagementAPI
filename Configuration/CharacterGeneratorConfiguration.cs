using System;
using System.Collections.Generic;
using CharacterGenerator.DefaultCharacters.Modules;
using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.Configuration
{
    [CreateAssetMenu(menuName = "Character Generator/Configuration")]
    public class CharacterGeneratorConfiguration : ScriptableObject
    {
        public TestModule testModule;
        
        public List<EntityModule> modules = new List<EntityModule>();

        public CharacterData CreateRandom(uint? seed = null)
        {
            var result = new CharacterData();

            foreach (var module in modules)
            {
                try
                {
                    module.Generate(result, seed);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Module \"{module?.name ?? "NULL"}\" failed to generate new character data. Exception: {exception.Message}");
                }
            }

            return result;
        }
    }
}
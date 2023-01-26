using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using Random = Unity.Mathematics.Random;

namespace CharacterGenerator.DefaultCharacters.Modules.Traits
{
    public class TraitsModule : EntityModule
    {
        public int minTraits = 0, maxTraits = 3;

        public List<TraitConfiguration> traits = new List<TraitConfiguration>();
        
        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            var traitCount = Random
                .CreateFromIndex(seed ?? (uint)UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue))
                .NextInt(minTraits, maxTraits);

            var resultTraits = new List<TraitInstance>();
            var availableTraits = traits.Clone();

            for (int i = 0; i < traitCount && availableTraits.Count > 0; i++)
            {
                var trait = availableTraits.Random();
                resultTraits.Add(trait.CreateRandomInstance());
                availableTraits.Remove(trait);
            }
            
            characterData.SetOrAdd(
                DefaultCharacterDataConstants.TRAITS,
                new TraitInstanceCollection(resultTraits).ToJson());
        }
    }
}
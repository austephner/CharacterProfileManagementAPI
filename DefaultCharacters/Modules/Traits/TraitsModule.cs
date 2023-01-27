using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.Instancing;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace CharacterGenerator.DefaultCharacters.Modules.Traits
{
    [CreateAssetMenu(menuName = "Character Generator/Default Modules/Traits")]
    public class TraitsModule : EntityModule
    {
        public override string displayName => "Traits";

        public AttributeModule attributeModule;
        
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
                var trait = seed != null
                    ? availableTraits.RandomByWeight(
                        seed.Value,
                        trait => trait.rarity)
                    : availableTraits.RandomByWeight(
                        trait => trait.rarity);
                
                resultTraits.Add(trait.CreateRandomInstance(attributeModule));
                availableTraits.Remove(trait);
            }
            
            characterData.SetOrAdd(
                DefaultCharacterDataConstants.TRAITS,
                new TraitInstanceCollection(resultTraits).ToJson());
        }

        public override void HandleGuidChange(string oldGuid, string newGuid)
        {
            base.HandleGuidChange(oldGuid, newGuid);

            foreach (var trait in traits)
            {
                foreach (var attributeAffect in trait.affectedAttributes)
                {
                    if (attributeAffect.attributeGuid == oldGuid)
                    {
                        attributeAffect.attributeGuid = newGuid;
                    }
                }
            }
        }
    }
}
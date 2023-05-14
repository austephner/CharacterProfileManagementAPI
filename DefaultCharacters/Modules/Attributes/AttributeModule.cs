using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace CharacterGenerator.DefaultCharacters.Modules.Attributes
{
    [CreateAssetMenu(menuName = "Character Generator/Default Modules/Attributes")]
    public class AttributeModule : EntityModule
    {
        public override string displayName => "Attributes";

        public List<AttributeConfiguration> attributes = new List<AttributeConfiguration>();

        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            var attributes = new List<AttributeInstance>(); 
            
            // todo: exclusion rules go here

            foreach (var attribute in this.attributes)
            {
                attributes.Add(new AttributeInstance()
                {
                    instanceGuid = attribute.guid,
                    name = attribute.name,
                    level = seed != null
                        ? new Random(seed.Value).NextInt(attribute.minStartingLevel, attribute.maxStartingLevel + 1)
                        : UnityEngine.Random.Range(attribute.minStartingLevel, attribute.maxStartingLevel + 1)
                });
            }
            
            characterData.SetOrAdd(
                DefaultCharacterDataConstants.ATTRIBUTES,
                new AttributeInstanceCollection() { attributes = attributes }.ToJson());
        }
    }
}
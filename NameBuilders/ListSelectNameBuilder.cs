using System.Collections.Generic;
using CharacterGenerator.Configuration;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterGenerator.NameBuilders
{
    [CreateAssetMenu(menuName = "Character Generator/Name Builders/List Select")]
    public class ListSelectNameBuilder : NameBuilder
    {
        [Range(0.0f, 1.0f)]
        public float
            firstNameChance = 1.0f,
            middleNameChance = 0.25f,
            secondMiddleNameChance = 0.25f,
            lastNameChance = 0.99f,
            hyphenateMiddleNamesChance = 0.15f;
        
        public List<string>
            firstNames = new List<string>(),
            middleNames = new List<string>(),
            secondMiddleNames = new List<string>(),
            lastNames = new List<string>();

        public override string GenerateName()
        {
            var result = "";

            if (GetRandom() < lastNameChance)
            {
                result = lastNames.Random(); 
            }

            if (GetRandom() < middleNameChance)
            {
                if (GetRandom() < secondMiddleNameChance)
                {
                    var middleNamesSeparator = GetRandom() < hyphenateMiddleNamesChance ? "-" : " ";
                    result = $"{middleNames.Random()}{middleNamesSeparator}{secondMiddleNames.Random()} {result}"; 
                }
                else
                {
                    result = $"{middleNames.Random()} {result}";
                }
            }

            if (string.IsNullOrEmpty(result) || GetRandom() < firstNameChance)
            {
                result = $"{firstNames.Random()} {result}"; 
            }

            return result.Trim();
        }

        private float GetRandom() => Random.Range(0f, 1f);
    }
}
using System.Collections.Generic;
using CharacterGenerator.Utility;
using UnityEngine;

namespace CharacterGenerator.NameBuilding
{
    [CreateAssetMenu(menuName = "Character Management/Name Builders/List Based")]
    public class ListBasedNameBuilder : CharacterNameBuilder
    {
        public List<string> firstNames, lastNames;
        
        public override string CreateRandomName()
        {
            return $"{firstNames.Random()} {lastNames.Random()}";
        }
    }
}
using System.Collections.Generic;
using CharacterGenerator.Old.Utility;
using UnityEngine;

namespace CharacterGenerator.Old.NameBuilding
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
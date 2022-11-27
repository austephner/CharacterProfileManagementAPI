using System.Collections.Generic;
using UnityEngine;

namespace CharacterGenerator.Configuration
{
    [CreateAssetMenu(menuName = "Character Generator")]
    public class CharacterGeneratorConfiguration : ScriptableObject
    {
        public List<Species> species = new List<Species>();
        
        public List<Trait> traits = new List<Trait>();
    }
}
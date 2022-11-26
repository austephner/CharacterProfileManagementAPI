using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration.Interfaces;
using JetBrains.Annotations;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class SpeciesConfiguration : INameBuilder
    {
        public string guid = Guid.NewGuid().ToString();

        public string name = "Species";
        
        /// <summary>
        /// All name builders available for characters of this species.
        /// </summary>
        public List<INameBuilder> characterNameBuilders = new List<INameBuilder>();

        public string GenerateName() => characterNameBuilders.Random()?.GenerateName();
    }
}
using System;
using System.Collections.Generic;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class Species : CharacterConfigurationEntity
    {
        /// <summary>
        /// All name builders available for characters of this species.
        /// </summary>
        public List<NameBuilder> characterNameBuilders = new List<NameBuilder>();

        public string GenerateName() => characterNameBuilders.Random()?.CreateName();
    }
}
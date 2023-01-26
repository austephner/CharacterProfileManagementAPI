using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;

namespace CharacterGenerator.DefaultCharacters.Modules.Race
{
    [Serializable]
    public class RaceConfiguration : EntityConfiguration
    {
        /// <summary>
        /// All name builders available for characters of this species.
        /// </summary>
        public List<NameBuilder> characterNameBuilders = new List<NameBuilder>();

        public string GenerateName() => characterNameBuilders.Random()?.GenerateName();

        public RaceInstance CreateInstance()
        {
            return new RaceInstance()
            {
                guid = guid,
                name = name
            };
        }
    }
}
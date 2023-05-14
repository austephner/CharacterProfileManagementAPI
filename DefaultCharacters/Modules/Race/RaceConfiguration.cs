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

        // todo: list of attribute affects
        
        public string GenerateName() => characterNameBuilders.Random()?.GenerateName();

        public RaceInstance CreateInstance()
        {
            return new RaceInstance()
            {
                instanceGuid = guid,
                name = name
            };
        }
    }
}
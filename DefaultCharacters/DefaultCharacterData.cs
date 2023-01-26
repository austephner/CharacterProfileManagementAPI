using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Race;
using CharacterGenerator.DefaultCharacters.Modules.Traits;
using CharacterGenerator.Instancing;

namespace CharacterGenerator.DefaultCharacters
{
    /// <summary>
    /// Custom character class that utilizes the default modules which come with this package. 
    /// </summary>
    [Serializable]
    public class DefaultCharacterData : CharacterData
    {
        public string name = "";

        public RaceInstance race = new RaceInstance();

        public List<TraitInstance> traits = new List<TraitInstance>();

        public DefaultCharacterData(CharacterData characterData)
        {
            guid = characterData.guid;

            if (characterData.ContainsKey(DefaultCharacterDataConstants.NAME))
            {
                name = characterData[DefaultCharacterDataConstants.NAME];
                characterData.Remove(DefaultCharacterDataConstants.NAME);
            }

            if (characterData.ContainsKey(DefaultCharacterDataConstants.RACE))
            {
                race = characterData.GetFromJson<RaceInstance>(DefaultCharacterDataConstants.RACE);
                characterData.Remove(DefaultCharacterDataConstants.RACE);
            }

            if (characterData.ContainsKey(DefaultCharacterDataConstants.TRAITS))
            {
                traits = characterData.GetFromJson<TraitInstanceCollection>(DefaultCharacterDataConstants.TRAITS).traits;
                characterData.Remove(DefaultCharacterDataConstants.TRAITS);
            }

            foreach (var key in characterData.Keys)
            {
                SetOrAdd(key, characterData[key]);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.DefaultCharacters.Modules.Race;
using CharacterGenerator.DefaultCharacters.Modules.Traits;
using CharacterGenerator.Instancing;
using UnityEngine;

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

        public List<AttributeInstance> attributes = new List<AttributeInstance>();

        public DefaultCharacterData() { }
        
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

            if (characterData.ContainsKey(DefaultCharacterDataConstants.ATTRIBUTES))
            {
                attributes = characterData.GetFromJson<AttributeInstanceCollection>(DefaultCharacterDataConstants.ATTRIBUTES).attributes;
                characterData.Remove(DefaultCharacterDataConstants.ATTRIBUTES);
            }

            foreach (var key in characterData.Keys)
            {
                SetOrAdd(key, characterData[key]);
            }
        }

        public int GetTotalAttributeAffectsFromTraits(string attributeGuid)
        {
            var result = 0f;

            for (int i = 0; i < traits.Count; i++)
            {
                for (int j = 0; j < traits[i].affectedAttributes.Count; j++)
                {
                    if (traits[i].affectedAttributes[j].attributeGuid == attributeGuid)
                    {
                        result += traits[i].affectedAttributes[j].affectAmount; 
                    }
                }
            }
            
            return Mathf.RoundToInt(result);
        }
    }
}
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using UnityEngine;

namespace CharacterGenerator.DefaultCharacters.Modules.Race
{
    [CreateAssetMenu(menuName = "Character Generator/Default Modules/Race")]
    public class RaceModule : EntityModule
    {
        public List<RaceConfiguration> races = new List<RaceConfiguration>();

        public override string displayName => "Races";

        public override void Generate(CharacterData characterData, uint? seed = null)
        {
            var randomRace = seed != null
                ? races.RandomByWeight(
                    seed.Value,
                    race => race.rarity)
                : races.RandomByWeight(
                    race => race.rarity);

            if (randomRace == null)
            {
                return; 
            }
            
            characterData.SetOrAdd(DefaultCharacterDataConstants.RACE, randomRace.CreateInstance().ToJson());
            characterData.SetOrAdd(DefaultCharacterDataConstants.NAME, randomRace.GenerateName());
        }
    }
}
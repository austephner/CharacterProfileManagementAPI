using CharacterGenerator.Utility;
using UnityEngine;

namespace CharacterGenerator.NameBuilding
{
    [CreateAssetMenu(menuName = "Character Management/Name Builders/Random String Name")]
    public class RandomStringNameBuilder : CharacterNameBuilder
    {
        public string 
            format = "AAA-00000000",
            validLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            validNumbers = "0123456789";

        public string[] invalidCombinations = new string[0];

        public override string CreateRandomName()
        {
            var result = "";

            foreach (var character in format)
            {
                result += 
                    character.IsLetter()
                        ? validLetters.Random() 
                        : character.IsDigit()
                            ? validNumbers.Random() 
                            : character;
            }

            foreach (var invalidCombination in invalidCombinations)
            {
                if (result.Contains(invalidCombination))
                {
                    // todo: refactor this
                    return CreateRandomName();
                }
            }
            
            return result;
        }
    }
}
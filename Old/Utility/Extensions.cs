using System.Collections.Generic;
using System.Linq;

namespace CharacterGenerator.Old.Utility
{
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            var index = UnityEngine.Random.Range(0, array.Count());
            return array[index];
        }
        
        public static bool IsLetter(this char character)
        {
            return char.IsLetter(character);
        }

        public static bool IsDigit(this char character)
        {
            return char.IsDigit(character);
        }
        
        public static bool IsNullOrWhiteSpace(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }
    }
}
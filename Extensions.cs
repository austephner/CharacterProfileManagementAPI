using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace CharacterGenerator
{
    public static class Extensions
    {
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static T Random<T>(this List<T> list, uint seed)
        {
            return list[new Random(seed).NextInt(0, list.Count)];
        }

        public static T FromJson<T>(this string value)
        {
            return JsonUtility.FromJson<T>(value);
        }

        public static string ToJson(this object value)
        {
            return JsonUtility.ToJson(value);
        }

        public static List<T> Clone<T>(this List<T> list)
        {
            var result = new List<T>();

            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i]); 
            }

            return result;
        }
    }
}
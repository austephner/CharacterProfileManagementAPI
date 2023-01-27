using System;
using System.Collections.Generic;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class AvailabilityRule
    {
        /// <summary>
        /// All GUID items that this content is exclusive to. Otherwise, availability is globally assumed.
        /// </summary>
        public List<string> exclusiveTo = new List<string>();

        /// <summary>
        /// All GUID items that this content is unavailable to. Otherwise, availability is globally assumed. 
        /// </summary>
        public List<string> unavailableTo = new List<string>();

        /// <summary>
        /// Checks whether the given GUID is contained in <see cref="exclusiveTo"/>. If it is, <c>true</c> is returned.
        /// If it's not, checks whether or not the GUID is contained in <see cref="unavailableTo"/>. If it is,
        /// <c>false</c> is returned. Otherwise, this function will return <c>true</c>.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool IsAvailableTo(string guid)
        {
            if (exclusiveTo.Contains(guid)) return true;
            if (unavailableTo.Contains(guid)) return false;
            return true;
        }
    }
}
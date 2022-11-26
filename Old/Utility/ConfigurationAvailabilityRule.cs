using System;
using System.Collections.Generic;

namespace CharacterGenerator.Old.Utility
{
    [Serializable]
    public class ConfigurationAvailabilityRule
    {
        public AvailabilityType availabilityType;
        public DataAvailabilityType dataAvailabilityType;
        public List<string> guids = new List<string>();
    }

    public enum AvailabilityType
    {
        None,
        /// <summary>
        /// This data configuration object is exclusive to the given GUIDs.
        /// </summary>
        Exclusive,
        /// <summary>
        /// This data configuration object is unavailable to the given GUIDs.
        /// </summary>
        Unavailable
    }

    public enum DataAvailabilityType
    {
        None,
        Species,
        Class
    }
}
using System;
using System.Collections.Generic;
using CharacterGenerator.Utility;
using UnityEngine;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class CharacterDataConfiguration
    {
        public string guid = Guid.NewGuid().ToString();
        public Texture icon;
        public string name, description;
        public bool showInEditor, showAvailabilityRulesInEditor;
        public List<ConfigurationAvailabilityRule> availabilityRules = new List<ConfigurationAvailabilityRule>();

        /// <summary>
        /// Determines if the given <see cref="CharacterDataConfiguration"/> item is compatible.
        /// </summary>
        /// <param name="characterDataConfiguration"></param>
        /// <returns></returns>
        public bool CheckCompatibility(CharacterDataConfiguration characterDataConfiguration)
        {
            for (int i = 0; i < availabilityRules.Count; i++)
            {
                switch (availabilityRules[i].availabilityType)
                {
                    case AvailabilityType.Exclusive:
                        return availabilityRules[i].guids.Contains(characterDataConfiguration.guid);
                    case AvailabilityType.Unavailable:
                        return !availabilityRules[i].guids.Contains(characterDataConfiguration.guid);
                }
            }

            return true;
        }

        public virtual void HandleGuidReplacement(string oldGuid, string newGuid)
        {
            if (guid == oldGuid)
            {
                guid = newGuid;
            }
            
            foreach (var rule in availabilityRules)
            {
                if (rule.guids.Contains(oldGuid))
                {
                    var oldGuidIndex = rule.guids.IndexOf(oldGuid);
                    rule.guids[oldGuidIndex] = newGuid;
                }
            }
        }
    }
}
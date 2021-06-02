using System;
using System.Collections.Generic;
using System.Linq;
using CharacterProfileManagement.Instancing;

namespace CharacterProfileManagement.Utility
{
    [Serializable]
    public class CharacterProfile
    {
        public string name;
        public List<CharacterTraitInstance> traits = new List<CharacterTraitInstance>();
        public List<CharacterAttributeInstance> attributes = new List<CharacterAttributeInstance>();
        public CharacterClassInstance mainClass;

        public CharacterSpeciesInstance species;

        public void RecalculateTraitAttributes()
        {
            foreach (var attribute in attributes)
            {
                attribute.effects.Clear();
            }

            foreach (var trait in traits)
            {
                foreach (var attributeEffect in trait.attributeEffects)
                {
                    var attribute = GetAttribute(attributeEffect.attributeGuid);

                    if (attribute == null)
                    {
                        var newAttribute = new CharacterAttributeInstance
                        {
                            configurationGuid = attributeEffect.attributeGuid
                        };

                        newAttribute.effects.Add(attributeEffect);

                        attributes.Add(newAttribute);
                    }
                    else
                    {
                        attribute.effects.Add(attributeEffect);
                    }
                }
            }

            foreach (var attribute in attributes)
            {
                attribute.RecalculateValue();
            }
        }

        public CharacterAttributeInstance GetAttribute(string attributeConfigurationGuid)
        {
            return attributes.FirstOrDefault(a => a.configurationGuid == attributeConfigurationGuid);
        }
    }
}
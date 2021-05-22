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

        public CharacterSpeciesInstance species;
        public CharacterClassInstance mainClass;
        public List<CharacterAttributeInstance> attributes = new List<CharacterAttributeInstance>();
        public List<CharacterTraitInstance> traits = new List<CharacterTraitInstance>();

        public void RecalculateTraitAttributes()
        {
            foreach (var baseAttribute in attributes)
            {
                baseAttribute.effects.Clear();
            }
            
            foreach (var trait in traits)
            {
                foreach (var attributeEffect in trait.attributeEffects)
                {
                    var baseAttribute = GetBaseAttribute(attributeEffect.configurationGuid);

                    if (baseAttribute == null)
                    {
                        var newAttribute = new CharacterAttributeInstance()
                        {
                            configurationGuid = attributeEffect.configurationGuid
                        };

                        newAttribute.effects.Add(attributeEffect);
                        
                        attributes.Add(newAttribute);
                    }
                    else
                    {
                        baseAttribute.effects.Add(attributeEffect);
                    }
                }
            }
            
            foreach (var baseAttribute in attributes)
            {
                baseAttribute.RecalculateValue();
            }
        }

        public CharacterAttributeInstance GetBaseAttribute(string attributeConfigurationGuid)
        {
            return attributes.FirstOrDefault(a => a.configurationGuid == attributeConfigurationGuid);
        }
    }
}
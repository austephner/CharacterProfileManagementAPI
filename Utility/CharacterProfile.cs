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
            foreach (var baseAttribute in attributes) baseAttribute.effects.Clear();

            foreach (var trait in traits)
            foreach (var attributeEffect in trait.attributeEffects)
            {
                var baseAttribute = GetBaseAttribute(attributeEffect.attributeGuid);

                if (baseAttribute == null)
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
                    baseAttribute.effects.Add(attributeEffect);
                }
            }

            foreach (var baseAttribute in attributes) baseAttribute.RecalculateValue();
        }

        public CharacterAttributeInstance GetBaseAttribute(string attributeConfigurationGuid)
        {
            return attributes.FirstOrDefault(a => a.configurationGuid == attributeConfigurationGuid);
        }
    }
}
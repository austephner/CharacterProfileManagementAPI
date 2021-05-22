using System.Collections.Generic;
using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    /// <summary>
    /// Represents an actual instance of an attribute, given a base value, additive values, and calculated value for
    /// easy access.
    /// </summary>
    public class CharacterAttributeInstance : CharacterDataInstance<CharacterAttributeConfiguration>
    {
        public int baseValue;
        
        public int calculatedValue { get; private set; }
        
        public int totalValue { get; private set; }

        public List<CharacterAttributeEffectInstance> effects = new List<CharacterAttributeEffectInstance>();

        public void RecalculateValue()
        {
            var stackedEffects = new List<string>();
            calculatedValue = 0;
            
            foreach (var effect in effects)
            {
                if (effect.GetConfiguration().stackable)
                {
                    if (stackedEffects.Contains(effect.configurationGuid))
                    {
                        continue;
                    }

                    stackedEffects.Add(effect.configurationGuid);
                }
                
                calculatedValue += effect.effectValue;
            }

            totalValue = baseValue + calculatedValue;
        }
    }
}
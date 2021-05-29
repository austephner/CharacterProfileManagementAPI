using System.Collections.Generic;
using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    /// <summary>
    ///     Represents an actual instance of an attribute, given a base value, additive values, and calculated value for
    ///     easy access.
    /// </summary>
    public class CharacterAttributeInstance : CharacterDataInstance<CharacterAttributeConfiguration>
    {
        /// <summary>
        ///     The base value of this attribute, unaffected by the <see cref="effects" />.
        /// </summary>
        public int baseValue;

        public List<CharacterAttributeEffectInstance> effects = new List<CharacterAttributeEffectInstance>();

        /// <summary>
        ///     The value that was calculated by adding all values from <see cref="effects" />.
        /// </summary>
        public int calculatedValue { get; private set; }

        /// <summary>
        ///     The base value plus the calculated value.
        /// </summary>
        public int totalValue { get; private set; }

        public void RecalculateValue()
        {
            var stackedEffects = new List<string>();
            calculatedValue = 0;

            foreach (var effect in effects)
            {
                if (effect.GetEffectConfiguration().stackable)
                {
                    if (stackedEffects.Contains(effect.attributeGuid)) continue;

                    stackedEffects.Add(effect.attributeGuid);
                }

                calculatedValue += effect.effectValue;
            }

            totalValue = baseValue + calculatedValue;
        }
    }
}
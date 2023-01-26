using System.Collections.Generic;

namespace CharacterGenerator.Configuration
{
    public abstract class AvailabilityConfiguration : EntityConfiguration
    {
        public List<AvailabilityRule> availabilityRules = new List<AvailabilityRule>();

        public virtual bool IsAvailableTo(string guid)
        {
            for (int i = 0; i < availabilityRules.Count; i++)
            {
                if (!availabilityRules[i].IsAvailableTo(guid))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
using System.Reflection;
using CharacterGenerator.Configuration;
using UnityEditor;

namespace CharacterGenerator.Editor
{
    [CustomEditor(typeof(CharacterGeneratorConfiguration))]
    public class CharacterGeneratorConfigurationEditor : UnityEditor.Editor
    {
        public new CharacterGeneratorConfiguration target => base.target as CharacterGeneratorConfiguration;
        
        private FieldInfo GetFieldInfo(string fieldName)
        {
            return typeof(CharacterGeneratorConfiguration).GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void ChangeGuid(string from, string to)
        {
            foreach (var species in target.species)
            {
                if (species.guid == from)
                {
                    species.guid = to;
                }
            }

            foreach (var trait in target.traits)
            {
                if (trait.guid == from)
                {
                    trait.guid = to;
                }
            }
        }
    }
}
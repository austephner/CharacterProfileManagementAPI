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

        public void UpdateEntityGuidAcrossAllModules(string from, string to)
        {
            foreach (var module in target.modules)
            {
                module.HandleGuidChange(from, to);
            }
        }
    }
}
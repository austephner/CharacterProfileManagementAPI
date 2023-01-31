using System.Reflection;
using CharacterGenerator.Configuration;
using UnityEditor;

namespace CharacterGenerator.Editor
{
    [CustomEditor(typeof(CharacterGeneratorConfiguration))]
    public class CharacterGeneratorConfigurationEditor : UnityEditor.Editor
    {
        private FieldInfo GetFieldInfo(string fieldName)
        {
            return typeof(CharacterGeneratorConfiguration).GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void UpdateEntityGuidAcrossAllModules(string from, string to)
        {
            var castedTarget = (CharacterGeneratorConfiguration)target;
            
            foreach (var module in castedTarget.modules)
            {
                module.HandleGuidChange(from, to);
            }
        }
    }
}
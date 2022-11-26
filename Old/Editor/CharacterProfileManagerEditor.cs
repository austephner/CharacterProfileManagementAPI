using System.Reflection;
using CharacterGenerator.Old.Configuration;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Old.Editor
{
    [CustomEditor(typeof(Behaviours.CharacterGeneratorBehaviour))]
    public class CharacterProfileManagerEditor : UnityEditor.Editor
    {
        public Behaviours.CharacterGeneratorBehaviour CharacterGeneratorBehaviour => target as Behaviours.CharacterGeneratorBehaviour;
        
        public CharacterClassConfigurationCollection characterClassConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_classConfigurationCollection")?.GetValue(CharacterGeneratorBehaviour) as CharacterClassConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_classConfigurationCollection")?.SetValue(CharacterGeneratorBehaviour, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterSpeciesConfigurationCollection characterSpeciesConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_speciesConfigurationCollection")?.GetValue(CharacterGeneratorBehaviour) as CharacterSpeciesConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_speciesConfigurationCollection")?.SetValue(CharacterGeneratorBehaviour, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterTraitConfigurationCollection characterTraitConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_traitConfigurationCollection")?.GetValue(CharacterGeneratorBehaviour) as CharacterTraitConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_traitConfigurationCollection")?.SetValue(CharacterGeneratorBehaviour, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterAttributeConfigurationCollection characterAttributeConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_attributeConfigurationCollection")?.GetValue(CharacterGeneratorBehaviour) as CharacterAttributeConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_attributeConfigurationCollection")?.SetValue(CharacterGeneratorBehaviour, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private FieldInfo GetFieldInfo(string fieldName)
        {
            return typeof(Behaviours.CharacterGeneratorBehaviour).GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Character Configuration Window", GUILayout.Height(30)))
            {
                CharacterConfigurationWindow.Open(this);
            }
            
            base.OnInspectorGUI();
        }
    }
}
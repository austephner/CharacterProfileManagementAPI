using System.Reflection;
using CharacterProfileManagement.Configuration;
using UnityEditor;
using UnityEngine;

namespace CharacterProfileManagement.Editor
{
    [CustomEditor(typeof(CharacterProfileManager))]
    public class CharacterProfileManagerEditor : UnityEditor.Editor
    {
        public CharacterProfileManager characterProfileManager => target as CharacterProfileManager;
        
        public CharacterClassConfigurationCollection characterClassConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_classConfigurationCollection")?.GetValue(characterProfileManager) as CharacterClassConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_classConfigurationCollection")?.SetValue(characterProfileManager, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterSpeciesConfigurationCollection characterSpeciesConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_speciesConfigurationCollection")?.GetValue(characterProfileManager) as CharacterSpeciesConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_speciesConfigurationCollection")?.SetValue(characterProfileManager, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterTraitConfigurationCollection characterTraitConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_traitConfigurationCollection")?.GetValue(characterProfileManager) as CharacterTraitConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_traitConfigurationCollection")?.SetValue(characterProfileManager, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        public CharacterAttributeConfigurationCollection characterAttributeConfigurationCollection
        {
            get
            {
                return GetFieldInfo("_attributeConfigurationCollection")?.GetValue(characterProfileManager) as CharacterAttributeConfigurationCollection;
            }
            set
            {
                serializedObject.Update();
                GetFieldInfo("_attributeConfigurationCollection")?.SetValue(characterProfileManager, value);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private FieldInfo GetFieldInfo(string fieldName)
        {
            return typeof(CharacterProfileManager).GetField(
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
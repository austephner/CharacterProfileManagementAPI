using System;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters;
using CharacterGenerator.DefaultCharacters.Modules.CharacterTesting;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class DefaultCharacterTestingModuleDrawer : EntityModuleDrawer
    {
        public override Type moduleType => typeof(DefaultCharacterTestingModule);

        public override int order => int.MaxValue;

        private DefaultCharacterData _data;

        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action setDirty,
            Action<string> recordChange,
            Action<string, string> handleGuidChange)
        {
            if (GUILayout.Button("Roll New Character"))
            {
                _data = new DefaultCharacterData(characterGeneratorConfiguration.CreateRandom());
            }

            if (_data != null)
            {
                using (new GUILayout.VerticalScope(GUI.skin.box))
                {
                    GUILayout.Label("Details", EditorStyles.boldLabel);
                    GUILayout.Label($"Name: {_data.name}");
                    GUILayout.Label($"Race: {_data.race.name}");
                    GUILayout.Space(15);
                    
                    GUILayout.Label("Attributes", EditorStyles.boldLabel);
                    if (_data.attributes.Count == 0) GUILayout.Label("None");
                    foreach (var attribute in _data.attributes)
                    {
                        var affect = _data.GetTotalAttributeAffectsFromTraits(attribute.guid);
                        var affectText = affect > 0 ? $"(+{affect})" : affect < 0 ? $"(-{affect})" : "";
                        GUILayout.Label($"{attribute.name}: {attribute.level} {affectText}");
                    }

                    GUILayout.Space(15);
                    GUILayout.Label("Traits", EditorStyles.boldLabel);
                    if (_data.traits.Count == 0) GUILayout.Label("None");
                    foreach (var trait in _data.traits)
                    {
                        GUILayout.Label($"{trait.name}");
                        if (trait.affectedAttributes.Count > 0)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Space(15);
                                using (new GUILayout.VerticalScope())
                                {
                                    foreach (var affect in trait.affectedAttributes)
                                    {
                                        GUILayout.Label($"Affects {affect.attributeName} by {Mathf.RoundToInt(affect.affectAmount)}");
                                    }
                                }
                            }
                        }
                    }

                    GUILayout.Space(15);

                    DrawExtraDataDetails(_data);
                }
            }
        }

        protected virtual void DrawExtraDataDetails(DefaultCharacterData data)
        {
            
        }
    }
}
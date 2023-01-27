using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.DefaultCharacters.Modules.Traits;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class TraitEntityModuleEditor : EntityModuleEditor
    {
        public override Type moduleType => typeof(TraitsModule);

        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfigurationEditor characterGeneratorConfigurationEditor,
            Action<string> setDirty)
        {
            var traitsModule = module as TraitsModule;

            if (traitsModule == null)
            {
                return; 
            }
            
            CharacterGeneratorGUIUtility.DrawTitle("Traits", "Manage all available traits for characters.");

            var nextAttributesModule = (AttributeModule) EditorGUILayout.ObjectField(
                "Attributes Module",
                traitsModule.attributeModule,
                typeof(AttributeModule),
                false);

            if (nextAttributesModule != traitsModule.attributeModule)
            {
                traitsModule.attributeModule = nextAttributesModule;
                setDirty("Changed trait module's attribute module");
            }

            GUILayout.Space(15);
            
            for (var i = 0; i < traitsModule.traits.Count; i++)
            {
                var unmodifiedIndex = i;
                var trait = traitsModule.traits[i];

                CharacterGeneratorGUIUtility.DrawEntity(
                    trait,
                    characterGeneratorConfigurationEditor,
                    setDirty,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Attribute Affectors");
                        GUILayout.Label("Traits can optionally affect attribute values for characters.", EditorStyles.wordWrappedMiniLabel);
                        
                        DrawAttributeAffectList(
                            configuration.affectedAttributes,
                            traitsModule.attributeModule,
                            setDirty);
                    },
                    configuration =>
                    {
                        traitsModule.traits.Remove(configuration);
                        setDirty("Removed trait");
                    },
                    configuration =>
                    {
                        traitsModule.traits.Insert(
                            unmodifiedIndex,
                            new TraitConfiguration()
                            {
                                description = configuration.description,
                                guid = Guid.NewGuid().ToString(),
                                name = configuration.name + " (CLONE)",
                                affectedAttributes = configuration.affectedAttributes.Clone(),
                                expandedInEditor = false
                            });
                    });
            }

            if (traitsModule.traits.Count != 0)
            {
                GUILayout.Space(15);
            }

            if (GUILayout.Button("Add New Trait"))
            {
                var nextTraitNumber = traitsModule.traits.Count + 1;

                while (traitsModule.traits.Any(r => r.name == $"Trait {nextTraitNumber}"))
                {
                    nextTraitNumber++;
                }

                traitsModule.traits.Add(new TraitConfiguration()
                {
                    name = $"Trait {nextTraitNumber}"
                });
                setDirty("Added new trait");
            }
        }
        
        private static void DrawAttributeAffectList(List<AttributeAffectConfiguration> list, AttributeModule attributeModule, Action<string> setDirty) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                var attributeAffect = list[i];
                
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        var nextAttributeGuid = CharacterGeneratorGUIUtility.DrawEntitySelect(
                            new GUIContent("Attribute", "The attribute which will be affected by this trait."),
                            attributeAffect.attributeGuid,
                            attributeModule.attributes);

                        if (nextAttributeGuid != attributeAffect.attributeGuid)
                        {
                            attributeAffect.attributeGuid = nextAttributeGuid;
                            setDirty("Changed attribute affect's target guid");
                        }

                        var nextMin = EditorGUILayout.FloatField("Min Affect", attributeAffect.minAffect);

                        if (nextMin != attributeAffect.minAffect)
                        {
                            attributeAffect.minAffect = nextMin;
                            setDirty("Updated attribute affect's min value");
                        }
                        
                        var nextMax = EditorGUILayout.FloatField("Max Affect", attributeAffect.maxAffect);

                        if (nextMax != attributeAffect.maxAffect)
                        {
                            attributeAffect.maxAffect = nextMax;
                            setDirty("Updated attribute affect's max value");
                        }
                    }

                    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                    {
                        setDirty("Removed name builder");
                        list.RemoveAt(i);
                        break;
                    }
                }
            }

            if (GUILayout.Button("Add Attribute Affect"))
            {
                list.Add(new AttributeAffectConfiguration());
                setDirty("Added name builder");
            }
        }
    }
}
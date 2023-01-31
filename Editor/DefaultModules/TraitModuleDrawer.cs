using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.DefaultCharacters.Modules.Traits;
using CharacterGenerator.Utilities;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class TraitModuleDrawer : EntityModuleDrawer
    {
        public override Type moduleType => typeof(TraitsModule);

        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action setDirty,
            Action<string> recordChange,
            Action<string, string> handleGuidChange)
        {
            var traitsModule = module as TraitsModule;

            if (traitsModule == null)
            {
                return; 
            }
            
            ModuleGUILayout.DrawTitle("Traits", "Manage all available traits for characters.");

            var nextAttributesModule = (AttributeModule) EditorGUILayout.ObjectField(
                "Attributes Module",
                traitsModule.attributeModule,
                typeof(AttributeModule),
                false);

            if (nextAttributesModule != traitsModule.attributeModule)
            {
                recordChange("Changed attribute module");
                traitsModule.attributeModule = nextAttributesModule;
                setDirty();
            }

            GUILayout.Space(15);
            
            for (var i = 0; i < traitsModule.traits.Count; i++)
            {
                var unmodifiedIndex = i;
                var trait = traitsModule.traits[i];

                ModuleGUILayout.DrawEntityFoldoutGroup(
                    trait,
                    setDirty,
                    recordChange,
                    handleGuidChange,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Attribute Affectors");
                        GUILayout.Label("Traits can optionally affect attribute values for characters.", EditorStyles.wordWrappedMiniLabel);
                        
                        DrawAttributeAffectList(
                            configuration.affectedAttributes,
                            traitsModule.attributeModule,
                            setDirty,
                            recordChange);
                    },
                    configuration =>
                    {
                        recordChange("Removed trait");
                        traitsModule.traits.Remove(configuration);
                        setDirty();
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

                recordChange("Added new trait");
                traitsModule.traits.Add(new TraitConfiguration() { name = $"Trait {nextTraitNumber}" });
                setDirty();
            }
        }
        
        private static void DrawAttributeAffectList(
            List<AttributeAffectConfiguration> list, 
            AttributeModule attributeModule, 
            Action setDirty,
            Action<string> recordChange) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                var attributeAffect = list[i];
                
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        var nextAttributeGuid = ModuleGUILayout.DrawEntitySelect(
                            new GUIContent("Attribute", "The attribute which will be affected by this trait."),
                            attributeAffect.attributeGuid,
                            attributeModule.attributes);

                        if (nextAttributeGuid != attributeAffect.attributeGuid)
                        {
                            recordChange("Changed attribute affect's target guid");
                            attributeAffect.attributeGuid = nextAttributeGuid;
                            setDirty();
                        }

                        var nextMin = EditorGUILayout.FloatField("Min Affect", attributeAffect.minAffect);

                        if (nextMin != attributeAffect.minAffect)
                        {
                            recordChange("Changed attribute affect's min value");
                            attributeAffect.minAffect = nextMin;
                            setDirty();
                        }
                        
                        var nextMax = EditorGUILayout.FloatField("Max Affect", attributeAffect.maxAffect);

                        if (nextMax != attributeAffect.maxAffect)
                        {
                            recordChange("Changed attribute affect's max value");
                            attributeAffect.maxAffect = nextMax;
                            setDirty();
                        }
                    }

                    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                    {
                        recordChange("Removed attribute affect");
                        list.RemoveAt(i);
                        setDirty();
                        break;
                    }
                }
            }

            if (GUILayout.Button("Add Attribute Affect"))
            {
                recordChange("Added attribute affect");
                list.Add(new AttributeAffectConfiguration());
                setDirty();
            }
        }
    }
}
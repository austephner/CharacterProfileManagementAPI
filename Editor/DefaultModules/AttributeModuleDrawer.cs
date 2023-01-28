using System;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class AttributeModuleDrawer : EntityModuleDrawer
    {
        public override Type moduleType => typeof(AttributeModule);
        
        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action<string> setDirty,
            Action<string, string> handleGuidChange)
        {
            var attributesModule = module as AttributeModule;

            if (attributesModule == null)
            {
                return; 
            }
            
            CharacterGeneratorGUIUtility.DrawTitle("Attributes", "Manage all available attributes for characters.");

            for (var i = 0; i < attributesModule.attributes.Count; i++)
            {
                var unmodifiedIndex = i;
                var attribute = attributesModule.attributes[i];

                CharacterGeneratorGUIUtility.DrawEntity(
                    attribute,
                    setDirty,
                    handleGuidChange,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Attribute Level Range");
                        CharacterGeneratorGUIUtility.DrawIntField(
                            new GUIContent("Min Level", "The lowest value this attribute's level can be."),
                            ref configuration.minLevel,
                            () => setDirty("Changed attribute's min level"));
                        CharacterGeneratorGUIUtility.DrawIntField(
                            new GUIContent("Max Level", "The lowest value this attribute's level can be."),
                            ref configuration.maxLevel,
                            () => setDirty("Changed attribute's max level"));
                        GUILayout.Space(15);
                        GUILayout.Label("Starting Levels");
                        CharacterGeneratorGUIUtility.DrawIntField(
                            new GUIContent("Min Starting Level", "The lowest value this attribute's level can start at."),
                            ref configuration.minStartingLevel,
                            () => setDirty("Changed attribute's min starting level"));
                        CharacterGeneratorGUIUtility.DrawIntField(
                            new GUIContent("Max Starting Level", "The highest value this attribute's level can start at."),
                            ref configuration.maxStartingLevel,
                            () => setDirty("Changed attribute's max starting level"));
                    },
                    configuration =>
                    {
                        attributesModule.attributes.Remove(configuration);
                        setDirty("Removed attribute");
                    },
                    configuration =>
                    {
                        attributesModule.attributes.Insert(
                            unmodifiedIndex,
                            new AttributeConfiguration()
                            {
                                description = configuration.description,
                                guid = Guid.NewGuid().ToString(),
                                name = configuration.name + " (CLONE)",
                                expandedInEditor = false,
                                minLevel = configuration.minLevel,
                                maxLevel = configuration.maxLevel,
                                minStartingLevel = configuration.minStartingLevel,
                                maxStartingLevel = configuration.maxStartingLevel
                                
                            });
                    },
                    false);
            }

            if (attributesModule.attributes.Count != 0)
            {
                GUILayout.Space(15);
            }

            if (GUILayout.Button("Add New Attribute"))
            {
                var nextAttributeNumber = attributesModule.attributes.Count + 1;

                while (attributesModule.attributes.Any(r => r.name == $"attribute {nextAttributeNumber}"))
                {
                    nextAttributeNumber++;
                }

                attributesModule.attributes.Add(new AttributeConfiguration()
                {
                    name = $"attribute {nextAttributeNumber}"
                });
                setDirty("Added new attribute");
            }
        }
    }
}
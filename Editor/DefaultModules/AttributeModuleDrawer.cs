using System;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Attributes;
using CharacterGenerator.Utilities;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class AttributeModuleDrawer : EntityModuleDrawer
    {
        public override Type moduleType => typeof(AttributeModule);
        
        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action setDirty,
            Action<string> recordChange,
            Action<string, string> handleGuidChange)
        {
            var attributesModule = module as AttributeModule;

            if (attributesModule == null)
            {
                return; 
            }
            
            ModuleGUILayout.DrawTitle("Attributes", "Manage all available attributes for characters.");

            for (var i = 0; i < attributesModule.attributes.Count; i++)
            {
                var unmodifiedIndex = i;
                var attribute = attributesModule.attributes[i];

                ModuleGUILayout.DrawEntityFoldoutGroup(
                    attribute,
                    setDirty,
                    recordChange,
                    handleGuidChange,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Attribute Level Range");
                        ModuleGUILayout.DrawIntField(
                            new GUIContent("Min Level", "The lowest value this attribute's level can be."),
                            ref configuration.minLevel,
                            (from, to) => recordChange("Changed attribute value"),
                            (from, to) => setDirty());
                        ModuleGUILayout.DrawIntField(
                            new GUIContent("Max Level", "The lowest value this attribute's level can be."),
                            ref configuration.maxLevel,
                            (from, to) => recordChange("Changed attribute value"),
                            (from, to) => setDirty());
                        GUILayout.Space(15);
                        GUILayout.Label("Starting Levels");
                        ModuleGUILayout.DrawIntField(
                            new GUIContent("Min Starting Level", "The lowest value this attribute's level can start at."),
                            ref configuration.minStartingLevel,
                            (from, to) => recordChange("Changed attribute value"),
                            (from, to) => setDirty());
                        ModuleGUILayout.DrawIntField(
                            new GUIContent("Max Starting Level", "The highest value this attribute's level can start at."),
                            ref configuration.maxStartingLevel,
                            (from, to) => recordChange("Changed attribute value"),
                            (from, to) => setDirty());
                    },
                    configuration =>
                    {
                        recordChange("Removed attribute");
                        attributesModule.attributes.Remove(configuration);
                        setDirty();
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

                recordChange("Added attribute");
                attributesModule.attributes.Add(new AttributeConfiguration() { name = $"attribute {nextAttributeNumber}" });
                setDirty();
            }
        }
    }
}
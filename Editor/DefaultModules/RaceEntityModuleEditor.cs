using System;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Race;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class RaceEntityModuleEditor : EntityModuleEditor
    {
        public override Type moduleType => typeof(RaceModule);
        
        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfigurationEditor characterGeneratorConfigurationEditor,
            Action<string> setDirty)
        {
            var raceModule = module as RaceModule;

            if (raceModule == null)
            {
                return; 
            }
            
            CharacterGeneratorGUIUtility.DrawTitle("Races", "Manage all available races for characters.");
            
            for (var i = 0; i < raceModule.races.Count; i++)
            {
                var unmodifiedIndex = i;
                var race = raceModule.races[i];

                CharacterGeneratorGUIUtility.DrawEntity(
                    race,
                    characterGeneratorConfigurationEditor,
                    setDirty,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Name Builders");
                        GUILayout.Label(
                            "Use these name builders to help determine the random names for members of this race.",
                            EditorStyles.wordWrappedMiniLabel);
                        GUILayout.Space(5);
                        CharacterGeneratorGUIUtility.DrawNameBuilderList(configuration.characterNameBuilders,
                            setDirty);
                        GUILayout.Space(15);
                    },
                    configuration =>
                    {
                        raceModule.races.Remove(configuration);
                        setDirty("Removed race");
                    },
                    configuration =>
                    {
                        raceModule.races.Insert(
                            unmodifiedIndex,
                            new RaceConfiguration()
                            {
                                description = configuration.description,
                                guid = Guid.NewGuid().ToString(),
                                name = configuration.name + " (CLONE)",
                                characterNameBuilders = configuration.characterNameBuilders.ToList(),
                                expandedInEditor = false
                            });
                    });
            }

            if (raceModule.races.Count != 0)
            {
                GUILayout.Space(15);
            }

            if (GUILayout.Button("Add New Race"))
            {
                var nextRaceNumber = raceModule.races.Count + 1;

                while (raceModule.races.Any(r => r.name == $"Race {nextRaceNumber}"))
                {
                    nextRaceNumber++;
                }

                raceModule.races.Add(new RaceConfiguration()
                {
                    name = $"Race {nextRaceNumber}"
                });
                setDirty("Added new race");
            }
        }
    }
}
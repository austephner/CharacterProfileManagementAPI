using System;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.DefaultCharacters.Modules.Race;
using CharacterGenerator.Utilities;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor.DefaultModules
{
    public class RaceModuleDrawer : EntityModuleDrawer
    {
        public override Type moduleType => typeof(RaceModule);
        
        public override void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action setDirty,
            Action<string> recordChange,
            Action<string, string> handleGuidChange)
        {
            var raceModule = module as RaceModule;

            if (raceModule == null)
            {
                return; 
            }
            
            ModuleGUILayout.DrawTitle("Races", "Manage all available races for characters.");
            
            for (var i = 0; i < raceModule.races.Count; i++)
            {
                var unmodifiedIndex = i;
                var race = raceModule.races[i];

                ModuleGUILayout.DrawEntityFoldoutGroup(
                    race,
                    setDirty,
                    recordChange,
                    handleGuidChange,
                    configuration =>
                    {
                        GUILayout.Space(15);
                        GUILayout.Label("Name Builders");
                        GUILayout.Label(
                            "Use these name builders to help determine the random names for members of this race.",
                            EditorStyles.wordWrappedMiniLabel);
                        GUILayout.Space(5);
                        ModuleGUILayout.DrawNameBuilderList(
                            configuration.characterNameBuilders,
                            setDirty,
                            recordChange);
                        GUILayout.Space(15);
                    },
                    configuration => raceModule.races.Remove(configuration),
                    configuration => raceModule.races.Insert(
                        unmodifiedIndex,
                        new RaceConfiguration()
                        {
                            description = configuration.description,
                            guid = Guid.NewGuid().ToString(),
                            name = configuration.name + " (CLONE)",
                            characterNameBuilders = configuration.characterNameBuilders.ToList(),
                            expandedInEditor = false
                        }));
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

                recordChange?.Invoke("Added new race");
                raceModule.races.Add(new RaceConfiguration() { name = $"Race {nextRaceNumber}" });
                setDirty?.Invoke();
            }
        }
    }
}
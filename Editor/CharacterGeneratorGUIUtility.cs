using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using UnityEditor;
using UnityEngine;
using Guid = Pathfinding.Util.Guid;

namespace CharacterGenerator.Editor
{
    public static class CharacterGeneratorGUIUtility
    {
        public static void DrawFullyFlexibleLabel(string label, GUIStyle style, params GUILayoutOption[] labelOptions)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(label, style, labelOptions);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();
            }
        }

        public static void DrawFullyFlexibleLabel(string label, params GUILayoutOption[] labelOptions)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(label, labelOptions);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();
            }
        }

        public static string DrawFilterBar(string filter, GUIStyle filterFieldStyle)
        {
            return EditorGUILayout.TextField(new GUIContent(), filter, filterFieldStyle);
        }
        
        public static string DrawGuidField(string guid, bool showRandomButton)
        {
            using (new GUILayout.HorizontalScope())
            {
                guid = EditorGUILayout.TextField("GUID", guid);
                
                if (showRandomButton)
                {
                    if (GUILayout.Button(new GUIContent("R", "Randomize"), GUILayout.ExpandWidth(false)))
                    {
                        guid = Guid.NewGuid().ToString();
                    }
                }
            }

            return guid;
        }

        public static void DrawTitle(string title, string subtitle)
        {
            GUILayout.Label(title, EditorStyles.boldLabel); 
            GUILayout.Label(subtitle, EditorStyles.wordWrappedMiniLabel);
            GUILayout.Space(15);
        }

        public static void DrawEntity<T>(
            T entityConfiguration,
            CharacterGeneratorConfigurationEditor characterGeneratorConfigurationEditor,
            Action<string> setDirty,
            Action<T> drawCustomContent,
            Action<T> onDeleteButtonClicked = null,
            Action<T> onDuplicateButtonClicked = null) where T : EntityConfiguration
        {
            
            var foldoutDisplayName = string.IsNullOrWhiteSpace(entityConfiguration.name)
                ? "Unnamed"
                : entityConfiguration.name;

            using (new EditorGUILayout.HorizontalScope())
            {
                entityConfiguration.expandedInEditor = EditorGUILayout.BeginFoldoutHeaderGroup(
                    entityConfiguration.expandedInEditor,
                    foldoutDisplayName);

                if (onDeleteButtonClicked != null &&
                    GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                {
                    onDeleteButtonClicked?.Invoke(entityConfiguration);
                    throw new ExitGUIException();
                }

                if (onDuplicateButtonClicked != null &&
                    GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Duplicate"), GUILayout.ExpandWidth(false)))
                {
                    onDuplicateButtonClicked?.Invoke(entityConfiguration);
                    throw new ExitGUIException();
                }
            }

            if (entityConfiguration.expandedInEditor)
            {
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    GUILayout.Space(15); 
                    
                    using (new GUILayout.VerticalScope())
                    {
                        var nextName = EditorGUILayout.TextField("Name", entityConfiguration.name);

                        if (nextName != entityConfiguration.name)
                        {
                            entityConfiguration.name = nextName;
                            setDirty($"Changed {nextName} name");
                        }

                        var nextGuid = DrawGuidField(entityConfiguration.guid, true);

                        if (nextGuid != entityConfiguration.guid)
                        {
                            characterGeneratorConfigurationEditor.UpdateEntityGuidAcrossAllModules(entityConfiguration.guid, nextGuid);
                            setDirty($"Changed {nextName} GUID");
                        }

                        GUILayout.Space(15);
                        GUILayout.Label("Description");

                        var nextDescription =
                            EditorGUILayout.TextArea(entityConfiguration.description, GUILayout.Height(50));

                        if (nextDescription != entityConfiguration.description)
                        {
                            entityConfiguration.description = nextDescription;
                            setDirty($"Changed {nextName} description");
                        }

                        GUILayout.Space(15);
                        GUILayout.Label("Rarity");

                        var nextRarity = EditorGUILayout.Slider(entityConfiguration.rarity, 0.0f, 1.0f);

                        if (nextRarity != entityConfiguration.rarity)
                        {
                            entityConfiguration.rarity = nextRarity;
                            setDirty($"Changed {nextName} rarity");
                        }

                        GUILayout.Space(15);
                        drawCustomContent?.Invoke(entityConfiguration);
                    }

                    GUILayout.Space(15);
                }

                GUILayout.Space(15);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        public static void DrawNameBuilderList(List<NameBuilder> list, Action<string> setDirty) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var next = (NameBuilder)EditorGUILayout.ObjectField(list[i], typeof(NameBuilder), false);

                    if (next != list[i])
                    {
                        setDirty("Changed name builder");
                    }

                    list[i] = next;

                    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                    {
                        setDirty("Removed name builder");
                        list.RemoveAt(i);
                        break;
                    }
                }
            }

            if (GUILayout.Button("Add Name Builder"))
            {
                list.Add(null);
                setDirty("Added name builder");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using UnityEngine;
using Guid = Pathfinding.Util.Guid;
using Object = UnityEngine.Object;

namespace CharacterGenerator.Utilities
{
    public static class ModuleGUILayout
    {
        public static void DrawObjectField<T>(GUIContent content, ref T obj, Action onChanged = null) where T : Object
        {
            #if UNITY_EDITOR
            var nextObjValue = (T)UnityEditor.EditorGUILayout.ObjectField(content, obj, typeof(T), false);
            if (nextObjValue != obj)
            {
                onChanged?.Invoke();
                obj = nextObjValue;
            }
            #endif
        }
        
        public static void DrawObjectField<T>(ref T obj, Action onChanged = null) where T : Object
        {
            #if UNITY_EDITOR
            var nextObjValue = (T)UnityEditor.EditorGUILayout.ObjectField(obj, typeof(T), false);
            if (nextObjValue != obj)
            {
                onChanged?.Invoke();
                obj = nextObjValue;
            }
            #endif
        }
        
        public static void DrawIntField(GUIContent content, ref int value, Action onChanged = null)
        {
            #if UNITY_EDITOR
            var nextIntValue = UnityEditor.EditorGUILayout.IntField(content, value);

            if (nextIntValue != value)
            {
                onChanged?.Invoke();
                value = nextIntValue;
            }
            #endif
        }
        
        public static void DrawIntField(string label, ref int value, Action onChanged = null)
        {
            DrawIntField(new GUIContent(label), ref value, onChanged);
        }

        public static void DrawFloatField(GUIContent content, ref float value, Action onChanged = null, float min = 0.0f, float max = 1.0f)
        {
            #if UNITY_EDITOR
            var nextFloat = UnityEditor.EditorGUILayout.Slider(content, value, min, max);

            if (nextFloat != value)
            {
                onChanged?.Invoke();
                value = nextFloat;
            }
            #endif
        }

        public static void DrawFloatField(string label, ref float value, Action onChanged = null, float min = 0f, float max = 1f)
        {
            DrawFloatField(new GUIContent(label), ref value, onChanged, min, max);
        }
        
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
            #if UNITY_EDITOR
            return UnityEditor.EditorGUILayout.TextField(new GUIContent(), filter, filterFieldStyle);
            #endif
        }
        
        // todo: format this function to behave like the DrawInt and DrawObject functions above
        public static string DrawGuidField(string guid, bool showRandomButton)
        {
            #if UNITY_EDITOR
            using (new GUILayout.HorizontalScope())
            {
                guid = UnityEditor.EditorGUILayout.TextField("GUID", guid);
                
                if (showRandomButton)
                {
                    if (GUILayout.Button(new GUIContent("R", "Randomize"), GUILayout.ExpandWidth(false)))
                    {
                        guid = Guid.NewGuid().ToString();
                    }
                }
            }

            return guid;
            #endif
        }

        public static void DrawTitle(string title, string subtitle)
        {
            #if UNITY_EDITOR
            GUILayout.Label(title, UnityEditor.EditorStyles.boldLabel); 
            GUILayout.Label(subtitle, UnityEditor.EditorStyles.wordWrappedMiniLabel);
            GUILayout.Space(15);
            #endif
        }

        public static void DrawEntityFoldoutGroup<T>(
            T entityConfiguration,
            Action<string> setDirty,
            Action<string, string> handleEntityGuidChange,
            Action<T> drawCustomContent,
            Action<T> onDeleteButtonClicked = null,
            Action<T> onDuplicateButtonClicked = null,
            bool showRarity = true) where T : EntityConfiguration
        {
            #if UNITY_EDITOR
            var foldoutDisplayName = string.IsNullOrWhiteSpace(entityConfiguration.name)
                ? "Unnamed"
                : entityConfiguration.name;

            using (new GUILayout.HorizontalScope())
            {
                entityConfiguration.expandedInEditor = UnityEditor.EditorGUILayout.BeginFoldoutHeaderGroup(
                    entityConfiguration.expandedInEditor,
                    foldoutDisplayName);

                if (onDeleteButtonClicked != null &&
                    GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                {
                    onDeleteButtonClicked?.Invoke(entityConfiguration);
                    throw new ExitGUIException();
                }

                if (onDuplicateButtonClicked != null &&
                    GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Duplicate"), GUILayout.ExpandWidth(false)))
                {
                    onDuplicateButtonClicked?.Invoke(entityConfiguration);
                    throw new ExitGUIException();
                }
            }

            if (entityConfiguration.expandedInEditor)
            {
                using (new GUILayout.HorizontalScope(GUI.skin.box))
                {
                    GUILayout.Space(15); 
                    
                    using (new GUILayout.VerticalScope())
                    {
                        var nextName = UnityEditor.EditorGUILayout.TextField("Name", entityConfiguration.name);

                        if (nextName != entityConfiguration.name)
                        {
                            entityConfiguration.name = nextName;
                            setDirty($"Changed {nextName} name");
                        }

                        var nextGuid = DrawGuidField(entityConfiguration.guid, true);

                        if (nextGuid != entityConfiguration.guid)
                        {
                            handleEntityGuidChange?.Invoke(entityConfiguration.guid, nextGuid);
                            setDirty($"Changed {nextName} GUID");
                        }

                        GUILayout.Space(15);
                        GUILayout.Label("Description");

                        var nextDescription = UnityEditor.EditorGUILayout.TextArea(entityConfiguration.description, GUILayout.Height(50));

                        if (nextDescription != entityConfiguration.description)
                        {
                            entityConfiguration.description = nextDescription;
                            setDirty($"Changed {nextName} description");
                        }

                        if (showRarity)
                        {
                            GUILayout.Space(15);
                            GUILayout.Label("Rarity");

                            var nextRarity = UnityEditor.EditorGUILayout.Slider(entityConfiguration.rarity, 0.0f, 1.0f);

                            if (nextRarity != entityConfiguration.rarity)
                            {
                                entityConfiguration.rarity = nextRarity;
                                setDirty($"Changed {nextName} rarity");
                            }
                        }

                        GUILayout.Space(15);
                        drawCustomContent?.Invoke(entityConfiguration);
                    }

                    GUILayout.Space(15);
                }

                GUILayout.Space(15);
            }

            UnityEditor.EditorGUILayout.EndFoldoutHeaderGroup();
            #endif
        }
        
        public static void DrawEntityBox<T>(
            T entityConfiguration,
            Action<string> setDirty,
            Action<string, string> handleEntityGuidChange,
            Action<T> drawCustomContent,
            Action<T> onDeleteButtonClicked = null,
            Action<T> onDuplicateButtonClicked = null,
            bool showRarity = true) where T : EntityConfiguration
        {
            #if UNITY_EDITOR
            var displayName = string.IsNullOrWhiteSpace(entityConfiguration.name)
                ? "Unnamed"
                : entityConfiguration.name;

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(displayName, UnityEditor.EditorStyles.boldLabel);

                    if (onDeleteButtonClicked != null &&
                        GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Trash"),
                            GUILayout.ExpandWidth(false)))
                    {
                        onDeleteButtonClicked?.Invoke(entityConfiguration);
                        throw new ExitGUIException();
                    }

                    if (onDuplicateButtonClicked != null &&
                        GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Duplicate"),
                            GUILayout.ExpandWidth(false)))
                    {
                        onDuplicateButtonClicked?.Invoke(entityConfiguration);
                        throw new ExitGUIException();
                    }
                }

                GUILayout.Space(15);

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(15);

                    using (new GUILayout.VerticalScope())
                    {
                        var nextName = UnityEditor.EditorGUILayout.TextField("Name", entityConfiguration.name);

                        if (nextName != entityConfiguration.name)
                        {
                            entityConfiguration.name = nextName;
                            setDirty($"Changed {nextName} name");
                        }

                        var nextGuid = DrawGuidField(entityConfiguration.guid, true);

                        if (nextGuid != entityConfiguration.guid)
                        {
                            handleEntityGuidChange?.Invoke(entityConfiguration.guid, nextGuid);
                            setDirty($"Changed {nextName} GUID");
                        }

                        GUILayout.Space(15);
                        GUILayout.Label("Description");

                        var nextDescription =
                            UnityEditor.EditorGUILayout.TextArea(entityConfiguration.description, GUILayout.Height(50));

                        if (nextDescription != entityConfiguration.description)
                        {
                            entityConfiguration.description = nextDescription;
                            setDirty($"Changed {nextName} description");
                        }

                        if (showRarity)
                        {
                            GUILayout.Space(15);
                            GUILayout.Label("Rarity");

                            var nextRarity = UnityEditor.EditorGUILayout.Slider(entityConfiguration.rarity, 0.0f, 1.0f);

                            if (nextRarity != entityConfiguration.rarity)
                            {
                                entityConfiguration.rarity = nextRarity;
                                setDirty($"Changed {nextName} rarity");
                            }
                        }

                        GUILayout.Space(15);
                        drawCustomContent?.Invoke(entityConfiguration);
                    }

                    GUILayout.Space(15);
                }
            }

            GUILayout.Space(15);
            #endif
        }

        public static void DrawList<T>(
            List<T> list,
            Action<T> onDraw,
            Action<T> onRemove,
            Action<T> onDuplicate,
            bool showMoveButtons = true)
        {
            var priorGuiState = GUI.enabled;
            
            #if UNITY_EDITOR
            for (int i = 0; i < list.Count; i++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.HorizontalScope(GUILayout.Width(25)))
                    {
                        GUILayout.Label($"#{i + 1}", UnityEditor.EditorStyles.boldLabel);
                        GUILayout.FlexibleSpace();
                    }

                    using (new GUILayout.VerticalScope(GUI.skin.box))
                    {
                        onDraw?.Invoke(list[i]);
                    }

                    using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(35)))
                    {
                        if (onRemove != null &&
                            GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Trash")))
                        {
                            onRemove?.Invoke(list[i]);
                        }

                        if (onDuplicate != null && 
                            GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Duplicate")))
                        {
                            onDuplicate?.Invoke(list[i]);
                        }

                        if (showMoveButtons)
                        {
                            GUI.enabled = i != 0;
                            if (GUILayout.Button("UP")) { Debug.Log("Not Yet Implemented!"); }
                            if (GUILayout.Button("DN")) { Debug.Log("Not Yet Implemented!"); }
                            GUI.enabled = i != list.Count - 1;
                        }
                    }
                }
            }
            #endif
            
            GUI.enabled = priorGuiState;
        }
        
        public static void DrawNameBuilderList(List<NameBuilder> list, Action<string> setDirty) 
        {
            #if UNITY_EDITOR
            for (int i = 0; i < list.Count; i++)
            {
                using (new UnityEditor.EditorGUILayout.HorizontalScope())
                {
                    var next = (NameBuilder)UnityEditor.EditorGUILayout.ObjectField(list[i], typeof(NameBuilder), false);

                    if (next != list[i])
                    {
                        setDirty("Changed name builder");
                    }

                    list[i] = next;

                    if (GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
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
            #endif
        }

        public static string DrawEntitySelect<T>(GUIContent label, string selectedGuid, List<T> options) where T : EntityConfiguration
        {
            #if UNITY_EDITOR
            var guids = new List<string>() { "" };
            var names = new List<string>() { "None" };

            for (int i = 0; i < options.Count; i++)
            {
                guids.Add(options[i].guid);
                names.Add(options[i].name);
            }

            var selectedIndex = guids.IndexOf(selectedGuid);

            if (selectedIndex == -1)
            {
                selectedIndex = 0;
            }
            
            var nextSelectedIndex = 
                label != null 
                ? UnityEditor.EditorGUILayout.Popup(label, selectedIndex, names.ToArray())
                : UnityEditor.EditorGUILayout.Popup(selectedIndex, names.ToArray());

            return nextSelectedIndex != selectedIndex 
                ? guids[nextSelectedIndex] 
                : selectedGuid;
            #endif
        }

        public static bool DrawButtonLeft(string text, params GUILayoutOption[] options)
        {
            using (new GUILayout.HorizontalScope())
            {
                var result = GUILayout.Button(text, options);
                GUILayout.FlexibleSpace();
                return result;
            }
        }
        
        public static bool DrawButtonRight(string text, params GUILayoutOption[] options)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                return GUILayout.Button(text, options);
            }
        }
    }
}
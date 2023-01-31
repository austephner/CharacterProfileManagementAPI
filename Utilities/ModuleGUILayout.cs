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
        public static void DrawObjectField<T>(
            GUIContent content, 
            ref T obj, 
            Action<T,T> beforeChangeFromTo = null,
            Action<T,T> afterChangeFromTo = null) where T : Object
        {
            #if UNITY_EDITOR
            var nextObjValue = (T)UnityEditor.EditorGUILayout.ObjectField(content, obj, typeof(T), false);
            if (nextObjValue != obj)
            {
                var priorObjValue = obj;
                beforeChangeFromTo?.Invoke(obj, nextObjValue);
                obj = nextObjValue;
                afterChangeFromTo?.Invoke(priorObjValue, obj);
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
        
        public static void DrawIntField(GUIContent content, ref int value, Action<int, int> beforeChangeFromTo = null, Action<int, int> afterChangeFromTo = null)
        {
#if UNITY_EDITOR
            var nextIntValue = UnityEditor.EditorGUILayout.IntField(content, value);

            if (nextIntValue != value)
            {
                var priorValue = value;
                beforeChangeFromTo?.Invoke(value, nextIntValue);
                value = nextIntValue;
                afterChangeFromTo?.Invoke(priorValue, value);
            }
#endif
        }
        
        public static void DrawIntField(string label, ref int value, Action<int, int> beforeChangeFromTo = null, Action<int, int> afterChangeFromTo = null)
        {
            DrawIntField(new GUIContent(label), ref value, beforeChangeFromTo, afterChangeFromTo);
        }

        public static void DrawFloatField(GUIContent content, ref float value, Action<float, float> beforeChangeFromTo, Action<float, float> afterChangeFromTo, float min = 0.0f, float max = 1.0f)
        {
#if UNITY_EDITOR
            var nextFloat = UnityEditor.EditorGUILayout.Slider(content, value, min, max);

            if (nextFloat != value)
            {
                var priorValue = value;
                beforeChangeFromTo?.Invoke(value, nextFloat);
                value = nextFloat;
                afterChangeFromTo?.Invoke(priorValue, value);
            }
#endif
        }

        public static void DrawFloatField(string label, ref float value, Action<float, float> beforeChangeFromTo, Action<float, float> afterChangeFromTo, float min = 0f, float max = 1f)
        {
            DrawFloatField(new GUIContent(label), ref value, beforeChangeFromTo, afterChangeFromTo, min, max);
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
            return "";
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
            return "";
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
            Action setDirty,
            Action<string> recordChange,
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
                    recordChange?.Invoke("Deleted entity");
                    onDeleteButtonClicked?.Invoke(entityConfiguration);
                    setDirty?.Invoke();
                    return;
                }

                if (onDuplicateButtonClicked != null &&
                    GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Duplicate"), GUILayout.ExpandWidth(false)))
                {
                    recordChange?.Invoke("Duplicated entity");
                    onDuplicateButtonClicked?.Invoke(entityConfiguration);
                    setDirty?.Invoke();
                    return;
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
                            recordChange?.Invoke("Changed entity name");
                            entityConfiguration.name = nextName;
                            setDirty?.Invoke();
                        }

                        var nextGuid = DrawGuidField(entityConfiguration.guid, true);

                        if (nextGuid != entityConfiguration.guid)
                        {
                            recordChange?.Invoke("Changed entity GUID");
                            handleEntityGuidChange?.Invoke(entityConfiguration.guid, nextGuid);
                            setDirty?.Invoke();
                        }

                        GUILayout.Space(15);
                        GUILayout.Label("Description");

                        var nextDescription = UnityEditor.EditorGUILayout.TextArea(entityConfiguration.description, GUILayout.Height(50));

                        if (nextDescription != entityConfiguration.description)
                        {
                            recordChange?.Invoke("Changed entity description");
                            entityConfiguration.description = nextDescription;
                            setDirty?.Invoke();
                        }

                        if (showRarity)
                        {
                            GUILayout.Space(15);
                            GUILayout.Label("Rarity");

                            var nextRarity = UnityEditor.EditorGUILayout.Slider(entityConfiguration.rarity, 0.0f, 1.0f);

                            if (nextRarity != entityConfiguration.rarity)
                            {
                                recordChange?.Invoke("Changed entity rarity");
                                entityConfiguration.rarity = nextRarity;
                                setDirty?.Invoke();
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
        
        public static void DrawNameBuilderList(
            List<NameBuilder> list, 
            Action setDirty,
            Action<string> recordChange) 
        {
            #if UNITY_EDITOR
            for (int i = 0; i < list.Count; i++)
            {
                using (new UnityEditor.EditorGUILayout.HorizontalScope())
                {
                    var next = (NameBuilder)UnityEditor.EditorGUILayout.ObjectField(list[i], typeof(NameBuilder), false);

                    if (next != list[i])
                    {
                        recordChange?.Invoke("Changed name builder");
                        list[i] = next;
                        setDirty?.Invoke();
                    }

                    if (GUILayout.Button(UnityEditor.EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.ExpandWidth(false)))
                    {
                        recordChange?.Invoke("Removed name builder");
                        list.RemoveAt(i);
                        setDirty?.Invoke();
                        break;
                    }
                }
            }

            if (GUILayout.Button("Add Name Builder"))
            {
                recordChange?.Invoke("Added name builder");
                list.Add(null);
                setDirty?.Invoke();
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
            return "";
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
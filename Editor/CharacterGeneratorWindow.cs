using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.Utilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CharacterGenerator.Editor
{
    public class CharacterGeneratorWindow : EditorWindow
    {
        #region Constants
        
        private const string LIST_ITEM_PREFIX = "• ";
        
        #endregion
        
        #region Private

        private CharacterGeneratorConfigurationEditor _characterGeneratorConfigurationEditor;

        private EntityModule _selectedModule;
        
        private Vector2 _categoryViewScrollPosition;

        private static GUIStyle _filterFieldStyle;

        private ModuleTreeView _moduleTreeView;

        [SerializeField]
        private TreeViewState _moduleTreeViewState;

        private static IDictionary<Type, EntityModuleDrawer> _EntityModuleDrawers = new Dictionary<Type, EntityModuleDrawer>();

        #endregion
        
        #region Properties
        
        private static GUIStyle filterFieldStyle
        {
            get
            {
                if (_filterFieldStyle == null)
                {
                    _filterFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField");
                }

                return _filterFieldStyle;
            }
        }
        
        #endregion
        
        #region Events

        private void OnEnable()
        {
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            titleContent = new GUIContent("Character Generator");
            EnsureNonNullViewComponents();
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        private void OnUndoRedoPerformed()
        {
            
        }

        private void OnGUI()
        {
            if (_characterGeneratorConfigurationEditor)
            {
                var castedTarget = (CharacterGeneratorConfiguration)_characterGeneratorConfigurationEditor.target;
                var testModuleProperty = _characterGeneratorConfigurationEditor.serializedObject.FindProperty("testModule");

                // var nextTestField = EditorGUILayout.TextField(castedTarget.testModule.testField);
                //
                // if (nextTestField != castedTarget.testModule.testField)
                // {
                //     RecordUndoRedo("Changed test field");
                //     castedTarget.testModule.testField = nextTestField;
                //     _characterGeneratorConfigurationEditor.serializedObject.ApplyModifiedProperties();
                //     SetDirty();
                // }
            }
            
            EnsureNonNullViewComponents();
            DrawCharacterGeneratorField();
            
            if (!_characterGeneratorConfigurationEditor)
            {
                ModuleGUILayout.DrawFullyFlexibleLabel(
                    "Please select a character details generator configuration at the top of this window.",
                    EditorStyles.wordWrappedLabel);
            }
            else
            {
                DrawMainWindowContent();
            }
        }
        
        #endregion

        #region UI Functions

        private void DrawCharacterGeneratorField()
        {
            var characterGeneratorConfiguration = (CharacterGeneratorConfiguration)EditorGUILayout.ObjectField(
                _characterGeneratorConfigurationEditor?.target,
                typeof(CharacterGeneratorConfiguration),
                false);

            if (characterGeneratorConfiguration != _characterGeneratorConfigurationEditor?.target)
            {
                if (characterGeneratorConfiguration)
                {
                    _characterGeneratorConfigurationEditor =
                        UnityEditor.Editor.CreateEditor(characterGeneratorConfiguration) as
                            CharacterGeneratorConfigurationEditor;
                }
                else
                {
                    _characterGeneratorConfigurationEditor = null;
                }
            }
        }

        private void DrawMainWindowContent()
        {
            using (new GUILayout.HorizontalScope())
            {
                DrawModuleTree();
                DrawSelectedModule();
            }
        }

        private void DrawModuleTree()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(150)))
            {
                GUILayout.FlexibleSpace();
            }

            _moduleTreeView.OnGUI(GUILayoutUtility.GetLastRect());
        }

        private void DrawSelectedModule()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var selectedModuleTreeViewItem = _moduleTreeView.GetSelectedModule();
                
                if (selectedModuleTreeViewItem == null)
                {
                    ModuleGUILayout.DrawFullyFlexibleLabel("No module selected.");
                    return;
                }

                _selectedModule = selectedModuleTreeViewItem.module;
                _categoryViewScrollPosition = GUILayout.BeginScrollView(_categoryViewScrollPosition, null, GUI.skin.verticalScrollbar);

                if (_EntityModuleDrawers.TryGetValue(_selectedModule.GetType(), out var EntityModuleDrawer))
                {
                    var castedTarget = (CharacterGeneratorConfiguration) _characterGeneratorConfigurationEditor.target;
                    
                    EntityModuleDrawer.DrawModule(
                        _selectedModule,
                        castedTarget,
                        SetDirty,
                        RecordUndoRedo,
                        _characterGeneratorConfigurationEditor.UpdateEntityGuidAcrossAllModules);
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("This module doesn't have an editor.");
                        GUILayout.FlexibleSpace();
                    }
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndScrollView();
            }
        }

        #endregion

        #region Private Utilities

        private new void SetDirty()
        {
            EditorUtility.SetDirty(_characterGeneratorConfigurationEditor.target);
            EditorSceneManager.MarkAllScenesDirty();
        }

        private void RecordUndoRedo(string change = "")
        {
            Undo.RecordObject(_characterGeneratorConfigurationEditor.target, change);
        }

        private void EnsureNonNullViewComponents()
        {
            if (_moduleTreeViewState == null)
            {
                _moduleTreeViewState = new TreeViewState();
            }

            var castedTarget = (CharacterGeneratorConfiguration) _characterGeneratorConfigurationEditor?.target;

            if (_moduleTreeView == null || _moduleTreeView.GetRows().Count != (castedTarget?.modules?.Count ?? 0))
            {
                var nextId = 1;
                
                _moduleTreeView = new ModuleTreeView(
                    _moduleTreeViewState,
                    castedTarget?.modules?
                        .Where(module => module && module != null)
                        .OrderBy(module => _EntityModuleDrawers.ContainsKey(module.GetType()) ? _EntityModuleDrawers[module.GetType()].order : int.MaxValue - 1)
                        .Select(module => new ModuleTreeViewItem()
                    {
                        displayName = module.displayName,
                        id = nextId++,
                        depth = 0,
                        module = module
                    })?.ToList() ?? new List<ModuleTreeViewItem>());
            }
        }
        
        [MenuItem("Tools/Character Generator")]
        public static CharacterGeneratorWindow Open()
        {
            return GetWindow<CharacterGeneratorWindow>();
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void Init()
        {
            _EntityModuleDrawers = new Dictionary<Type, EntityModuleDrawer>();
            
            var editors = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => typeof(EntityModuleDrawer).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => (EntityModuleDrawer)Activator.CreateInstance(type))
                .ToList();

            foreach (var editor in editors)
            {
                _EntityModuleDrawers.Add(editor.moduleType, editor);
            }
        }

        #endregion
    }
}
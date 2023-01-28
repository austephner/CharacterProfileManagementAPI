using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CharacterGenerator.Editor
{
    public class CharacterGeneratorWindow : EditorWindow
    {
        #region Constants
        
        private const string LIST_ITEM_PREFIX = "• ";
        
        #endregion
        
        #region Private

        private CharacterGeneratorConfiguration _characterGeneratorConfiguration;

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
            EnsureNonNullViewComponents();
            DrawCharacterGeneratorField();
            
            if (!_characterGeneratorConfiguration)
            {
                CharacterGeneratorGUIUtility.DrawFullyFlexibleLabel(
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
                _characterGeneratorConfiguration,
                typeof(CharacterGeneratorConfiguration),
                false);

            if (characterGeneratorConfiguration != _characterGeneratorConfiguration)
            {
                _characterGeneratorConfiguration = characterGeneratorConfiguration;

                if (characterGeneratorConfiguration)
                {
                    _characterGeneratorConfigurationEditor =
                        UnityEditor.Editor.CreateEditor(_characterGeneratorConfiguration) as
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
                    CharacterGeneratorGUIUtility.DrawFullyFlexibleLabel("No module selected.");
                    return;
                }

                _selectedModule = selectedModuleTreeViewItem.module;
                _categoryViewScrollPosition = GUILayout.BeginScrollView(_categoryViewScrollPosition, null, GUI.skin.verticalScrollbar);

                if (_EntityModuleDrawers.TryGetValue(_selectedModule.GetType(), out var EntityModuleDrawer))
                {
                    EntityModuleDrawer.DrawModule(
                        _selectedModule,
                        _characterGeneratorConfigurationEditor.target,
                        SetDirty,
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

        [Obsolete]
        private void DrawManual()
        {
            GUILayout.Label("The manual is not yet available.");
        }

        [Obsolete]
        private void DrawAttributeEditor()
        {
            // _attributeFilter = DrawFilterBar(_attributeFilter);
            //
            // for (int i = 0; i < _characterGeneratorConfiguration.attributes.Count; i++)
            // {
            //     DrawAttribute(_characterGeneratorConfiguration.attributes[i]);
            // }
            //
            // if (_characterGeneratorConfiguration.attributes.Count > 0)
            // {
            //     GUILayout.Space(15);
            // }
            //
            // if (GUILayout.Button("Add Attribute"))
            // {
            //     Undo.RecordObject(_characterGeneratorConfiguration, "Added Attribute");
            //     _characterGeneratorConfigurationEditor.target.attributes.Add(new AttributeConfiguration());
            //     SetDirty();
            // }
        }

        [Obsolete]
        private void DrawRaceEditor()
        {
            // _raceFilter = DrawFilterBar(_raceFilter);
            //
            // for (int i = 0; i < _characterGeneratorConfiguration.races.Count; i++)
            // {
            //     var race = _characterGeneratorConfiguration.races[i];
            //     var foldoutDisplayName = string.IsNullOrWhiteSpace(race.name) ? "Unnamed Race" : race.name;
            //     
            //     race.expandedInEditor = EditorGUILayout.BeginFoldoutHeaderGroup(
            //         race.expandedInEditor,
            //         foldoutDisplayName);
            //
            //     if (race.expandedInEditor)
            //     {
            //         using (new GUILayout.VerticalScope(GUI.skin.box))
            //         {
            //             DrawRace(race);
            //         }
            //     }
            //
            //     EditorGUILayout.EndFoldoutHeaderGroup();
            // }
            //
            // if (_characterGeneratorConfiguration.races.Count > 0)
            // {
            //     GUILayout.Space(15);
            // }
            //
            // if (GUILayout.Button("Add Race"))
            // {
            //     Undo.RecordObject(_characterGeneratorConfiguration, "Added Race");
            //     _characterGeneratorConfigurationEditor.target.races.Add(new RaceConfiguration());
            //     SetDirty();
            // }
        }

        #endregion

        #region Private Utilities

        private void SetDirty(string dirtyMessage = "Change was made")
        {
            SetDirty(_characterGeneratorConfigurationEditor.target, dirtyMessage);
        }

        private new void SetDirty(Object dirty, string dirtyMessage = "Change was made")
        {
            EditorUtility.SetDirty(_characterGeneratorConfigurationEditor.target);

            if (!dirty)
            {
                Undo.RecordObject(dirty, dirtyMessage);
            }
        }

        // private void DrawEntity(EntityModuleDrawer module, EntityConfiguration entityConfiguration, string name)
        // {
        //     
        // }
        //
        // [Obsolete]
        // private void DrawRace(RaceConfiguration race)
        // {
        //     // DrawEntity(race, "race");
        //     //
        //     // GUILayout.Space(15); 
        //     // GUILayout.Label("Name Builders");
        //     // GUILayout.Label("Name builders are what generate names for new characters using this race.", EditorStyles.miniLabel);
        //     //
        //     // using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        //     // {
        //     //     DrawNameBuilderList(race.characterNameBuilders);
        //     // }
        // }
        //
        // [Obsolete]
        // private void DrawAttribute(AttributeConfiguration attribute)
        // {
        //     // DrawEntity(attribute, "attribute");
        //     //
        //     // GUILayout.Space(15);
        //     //
        //     // var nextMin = EditorGUILayout.IntField(
        //     //     new GUIContent("Min", "The lowest value this attribute can be."),
        //     //     attribute.min);
        //     //
        //     // if (nextMin != attribute.min)
        //     // {
        //     //     attribute.min = nextMin;
        //     //     SetDirty("Changed attribute min value");
        //     // }
        //     //     
        //     // var nextMax = EditorGUILayout.IntField(
        //     //     new GUIContent("Max", "The lowest value this attribute can be."),
        //     //     attribute.max);
        //     //
        //     // if (nextMax != attribute.max)
        //     // {
        //     //     attribute.max = nextMax;
        //     //     SetDirty("Changed attribute max value");
        //     // }
        // }

        private void EnsureNonNullViewComponents()
        {
            if (_moduleTreeViewState == null)
            {
                _moduleTreeViewState = new TreeViewState();
            }

            if (_moduleTreeView == null || _moduleTreeView.GetRows().Count != (_characterGeneratorConfiguration?.modules?.Count ?? 0))
            {
                var nextId = 1;
                
                _moduleTreeView = new ModuleTreeView(
                    _moduleTreeViewState,
                    _characterGeneratorConfiguration?.modules?.Select(module => new ModuleTreeViewItem()
                    {
                        displayName = module.displayName,
                        id = nextId++,
                        depth = 0,
                        module = module
                    })?.ToList() ?? new List<ModuleTreeViewItem>());
                    /*new List<ModuleTreeViewItem>()
                    {
                        new ModuleTreeViewItem()
                        {
                            displayName = "Manual",
                            id = 1,
                            depth = 0
                        },
                        new ModuleTreeViewItem()
                        {
                            displayName = "Attributes",
                            id = 5,
                            depth = 0,
                        },
                        new ModuleTreeViewItem()
                        {
                            displayName = "Races",
                            id = 2,
                            depth = 0,
                        },
                        new ModuleTreeViewItem()
                        {
                            displayName = "Primary Classes",
                            id = 3,
                            depth = 0,
                        },
                        new ModuleTreeViewItem()
                        {
                            displayName = "Sub Classes",
                            id = 4,
                            depth = 0,
                        },
                        new ModuleTreeViewItem()
                        {
                            displayName = "Traits",
                            id = 6,
                            depth = 0,
                        }
                    }*/
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
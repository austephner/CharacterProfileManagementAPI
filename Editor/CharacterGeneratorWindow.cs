using System;
using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.Old.NameBuilding;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        
        private Vector2
            _categoryViewScrollPosition,
            _classEditorScrollPosition,
            _speciesEditorScrollPosition,
            _traitEditorScrollPosition,
            _attributeEditorScrollPosition;

        private string
            _classFilter = "",
            _speciesFilter = "",
            _traitFilter = "",
            _attributeFilter = "";

        private static GUIStyle _filterFieldStyle;

        private bool
            _showSpeciesHelp,
            _showClassHelp,
            _showTraitHelp,
            _showAttributesHelp;

        private CategoryTreeView _categoryTreeView;

        [SerializeField]
        private TreeViewState _categoryTreeViewState;

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
                DrawFullyFlexibleLabel("Please select a character details generator configuration at the top of this window.", EditorStyles.wordWrappedLabel);
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
                DrawCategoryTree();
                DrawCurrentlySelectedCategory();
            }
        }

        private void DrawCategoryTree()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(150)))
            {
                GUILayout.FlexibleSpace();
            }

            _categoryTreeView.OnGUI(GUILayoutUtility.GetLastRect());
        }

        private void DrawCurrentlySelectedCategory()
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var selectedCategory = _categoryTreeView.GetCurrentCategory();

                if (selectedCategory == null)
                {
                    GUILayout.FlexibleSpace();
                    return;
                }
                
                _categoryViewScrollPosition = GUILayout.BeginScrollView(_categoryViewScrollPosition, null, GUI.skin.verticalScrollbar);

                GUILayout.Label(selectedCategory.displayName, EditorStyles.largeLabel);

                switch (selectedCategory.displayName)
                {
                    case "Manual":
                        DrawManual();
                        break;
                    
                    case "Species Editor":
                        DrawSpeciesEditor();
                        break;
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.EndScrollView();
            }
        }

        private void DrawManual()
        {
            GUILayout.Label("The manual is not yet available.");
        }

        private void DrawSpeciesEditor()
        {
            _speciesFilter = DrawFilterBar(_speciesFilter);

            for (int i = 0; i < _characterGeneratorConfiguration.species.Count; i++)
            {
                var species = _characterGeneratorConfiguration.species[i];
                var foldoutDisplayName = string.IsNullOrWhiteSpace(species.name) ? "Unnamed Species" : species.name;
                
                species.expandedInEditor = EditorGUILayout.BeginFoldoutHeaderGroup(
                    species.expandedInEditor,
                    foldoutDisplayName);

                if (species.expandedInEditor)
                {
                    using (new GUILayout.VerticalScope(GUI.skin.box))
                    {
                        DrawSpecies(species);
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (_characterGeneratorConfiguration.species.Count > 0)
            {
                GUILayout.Space(15);
            }

            if (GUILayout.Button("Add Species"))
            {
                Undo.RecordObject(_characterGeneratorConfiguration, "Added Species");
                _characterGeneratorConfigurationEditor.target.species.Add(new Species());
                SetDirty();
            }
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

        private void DrawSpecies(Species species)
        {
            var nextName = EditorGUILayout.TextField("Name", species.name);

            if (nextName != species.name)
            {
                species.name = nextName;
                SetDirty("Changed species name");
            }

            var nextGuid = DrawGuidField(species.guid, true);

            if (nextGuid != species.guid)
            {
                _characterGeneratorConfigurationEditor.ChangeGuid(species.guid, nextGuid);
                SetDirty("Changed species GUID");
            }

            GUILayout.Space(15);
            GUILayout.Label("Description");
            
            var nextDescription = EditorGUILayout.TextArea(species.description, GUILayout.Height(50));

            if (nextDescription != species.description)
            {
                species.description = nextDescription;
                SetDirty("Changed species description");
            }

            GUILayout.Space(15); 
            GUILayout.Label("Name Builders");
            GUILayout.Label("Name builders are what generate names for new characters using this species.", EditorStyles.miniLabel);

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                DrawNameBuilderList(species.characterNameBuilders);
            }
        }

        private void DrawNameBuilderList(List<NameBuilder> list) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var next = (NameBuilder)EditorGUILayout.ObjectField(list[i], typeof(NameBuilder), false);

                    if (next != list[i])
                    {
                        SetDirty("Changed name builder");
                    }

                    list[i] = next;

                    if (GUILayout.Button(new GUIContent("X", "Remove"), GUILayout.ExpandWidth(false)))
                    {
                        SetDirty("Removed name builder");
                        list.RemoveAt(i);
                        break;
                    }
                }
            }

            if (GUILayout.Button("Add Name Builder"))
            {
                list.Add(null);
                SetDirty();
            }
        }

        private string DrawGuidField(string guid, bool showRandomButton)
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

        private void EnsureNonNullViewComponents()
        {
            if (_categoryTreeViewState == null)
            {
                _categoryTreeViewState = new TreeViewState();
            }

            if (_categoryTreeView == null)
            {
                _categoryTreeView = new CategoryTreeView(
                    _categoryTreeViewState,
                    new List<CategoryTreeViewItem>()
                    {
                        new CategoryTreeViewItem()
                        {
                            displayName = "Manual",
                            id = 1,
                            depth = 0
                        },
                        new CategoryTreeViewItem()
                        {
                            displayName = "Species Editor",
                            id = 2,
                            depth = 0,
                        },
                        new CategoryTreeViewItem()
                        {
                            displayName = "Primary Classes",
                            id = 3,
                            depth = 0,
                        },
                        new CategoryTreeViewItem()
                        {
                            displayName = "Sub Classes",
                            id = 4,
                            depth = 0,
                        },
                        new CategoryTreeViewItem()
                        {
                            displayName = "Attributes",
                            id = 5,
                            depth = 0,
                        },
                        new CategoryTreeViewItem()
                        {
                            displayName = "Traits",
                            id = 6,
                            depth = 0,
                        }
                    });
            }
        }
        
        private static void DrawFullyFlexibleLabel(string label, GUIStyle style, params GUILayoutOption[] labelOptions)
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

        private static void DrawFullyFlexibleLabel(string label, params GUILayoutOption[] labelOptions)
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

        private static string DrawFilterBar(string filter)
        {
            return EditorGUILayout.TextField(new GUIContent(), filter, filterFieldStyle);
        }
        
        [MenuItem("Tools/Character Generator")]
        public static CharacterGeneratorWindow Open()
        {
            return GetWindow<CharacterGeneratorWindow>();
        }

        #endregion
    }
}
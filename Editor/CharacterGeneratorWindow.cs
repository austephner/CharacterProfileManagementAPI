using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

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
            _classEditorScrollPosition,
            _speciesEditorScrollPosition,
            _traitEditorScrollPosition,
            _attributeEditorScrollPosition;

        private string
            _classFilter = "",
            _speciesFilter = "",
            _traitFilter = "",
            _attributeFilter = "";

        private GUIStyle _filterFieldStyle;

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
        
        private GUIStyle filterFieldStyle
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
                DrawFullyFlexibleLabel("Please select a character generator configuration.");
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
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
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

                GUILayout.Label(selectedCategory.displayName, EditorStyles.largeLabel);
                
                GUILayout.FlexibleSpace();
            }
        }

        #endregion

        #region Private Utilities

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
                        }
                    });
            }
        }

        private static void DrawFullyFlexibleLabel(string label)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(label);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.FlexibleSpace();
            }
        }
        
        [MenuItem("Tools/Character Generator")]
        public static CharacterGeneratorWindow Open()
        {
            return GetWindow<CharacterGeneratorWindow>();
        }

        #endregion
    }
}
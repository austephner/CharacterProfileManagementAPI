using System;
using System.Collections.Generic;
using CharacterProfileManagement.Configuration;
using CharacterProfileManagement.NameBuilding;
using CharacterProfileManagement.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharacterProfileManagement.Editor
{
    // todo: replace all GUILayout.BeginXLayout() with using statements
    // todo: remaining menus
    public class CharacterConfigurationWindow : EditorWindow
    {
        public const string VERSION = "0.0";

        private const string LIST_ITEM = "•";
        
        #region Private
        
        [SerializeField] private CharacterProfileManagerEditor _characterManagerEditor;

        [SerializeField] private int _windowMode = 0;

        private CharacterProfileManager _characterManager;
        
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

        private CharacterProfile _testCharacterProfile = new CharacterProfile();
        
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

        #region Unity Events

        private void OnEnable()
        {
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            titleContent = new GUIContent("Character Configuration");
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        private void OnGUI()
        {
            _windowMode = GUILayout.SelectionGrid(
                _windowMode, 
                new string[] { "Manual", "Settings", "Tools", "Testing", "Species", "Classes", "Traits", "Attributes" }, 
                4,
                GUILayout.Height(50));
            
            var characterManager = (CharacterProfileManager) EditorGUILayout.ObjectField(
                "Character Manager",
                _characterManager,
                typeof(CharacterProfileManager),
                true);

            if (characterManager != _characterManager)
            {
                _characterManager = characterManager;
                _characterManagerEditor = UnityEditor.Editor.CreateEditor(characterManager) as CharacterProfileManagerEditor;
                Undo.RecordObject(this, "Changed character manager.");
                throw new ExitGUIException();
            }
            
            if (!_characterManagerEditor)
            {
                FlexibleLabel("Please select a character manager.");
                return;
            }
            
            if (!_characterManager || _characterManager != _characterManagerEditor.characterProfileManager)
            {
                _characterManager = _characterManagerEditor.characterProfileManager;
            }
            
            GUILayout.BeginVertical(EditorStyles.helpBox);
            switch (_windowMode)
            {
                case 0: // manual
                    DrawInfoMode();
                    break;
                // case 1: // settings
                //     DrawSettingsMode();
                //     break;
                // case 2: // tools
                //     break;
                case 3: // testing
                    DrawCharacterTestingMode();
                    break;
                case 4: // species
                    DrawCharacterSpeciesMode();
                    break;
                case 5: // classes
                    DrawCharacterClassMode();
                    break;
                case 6: // character traits
                    DrawCharacterTraitMode();
                    break;
                case 7: // atrributes
                    DrawCharacterAttributesMode();
                    break;
                default: 
                    FlexibleLabel("Mode currently not yet supported.");
                    break;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        
        #endregion

        #region Static Utilities

        [MenuItem("Tools/Character Configuration")]
        public static CharacterConfigurationWindow Open()
        {
            return GetWindow<CharacterConfigurationWindow>();
        }

        public static void Open(CharacterProfileManagerEditor characterManagerEditor)
        {
            var window = Open();
            window._characterManagerEditor = characterManagerEditor;
        }

        #endregion

        #region Main Window Modes

        private void DrawInfoMode()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Version: ");
            GUILayout.Label(VERSION, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                _showSpeciesHelp = EditorGUILayout.BeginFoldoutHeaderGroup(_showSpeciesHelp, "Species");
                
                if (_showSpeciesHelp)
                {
                    GUILayout.Label(
                        "Species represent the types of creatures that characters can be. Species can also control " +
                        "what classes, traits, etc. they can or can't have. This adds a dimension of uniqueness to " +
                        "some species.\n\n" +
                        "Examples of species:\n" +
                        $"   {LIST_ITEM} Human\n" +
                        $"   {LIST_ITEM} Elf\n" +
                        $"   {LIST_ITEM} Ork",
                        EditorStyles.wordWrappedLabel);

                    GUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                _showClassHelp = EditorGUILayout.BeginFoldoutHeaderGroup(_showClassHelp, "Classes");
                
                if (_showClassHelp)
                {
                    GUILayout.Label(
                        "Character classes are the \"professions\" or \"roles\" that a character can have. Classes " +
                        "determine what abilities and traits a character can have, as it is the second property that is " +
                        "randomly generated.\n\n" +
                        "Examples of classes:\n" +
                        $"   {LIST_ITEM} Ranger\n" +
                        $"   {LIST_ITEM} Soldier\n" +
                        $"   {LIST_ITEM} Wizard",
                        EditorStyles.wordWrappedLabel);

                    GUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                _showTraitHelp = EditorGUILayout.BeginFoldoutHeaderGroup(_showTraitHelp, "Traits");
                
                if (_showTraitHelp)
                {
                    GUILayout.Label(
                        "Traits are used to modify a character's attributes. They are often represented by a noun, " +
                        "phrase, adjective, or description. A trait's \"severity\" is randomly generated and determines " +
                        "the weight of the trait's effect on the character's mechanics. It also has configuration " +
                        "properties for converting severity to a value that actually impacts an attribute.\n\n" +
                        "Examples of traits:\n" +
                        $"   {LIST_ITEM} Long Legs (+1 speed)\n" +
                        $"   {LIST_ITEM} Tough (+3 fortitude)\n" +
                        $"   {LIST_ITEM} Cursed (boolean)",
                        EditorStyles.wordWrappedLabel);

                    GUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                _showAttributesHelp = EditorGUILayout.BeginFoldoutHeaderGroup(_showAttributesHelp, "Attributes");
                
                if (_showAttributesHelp)
                {
                    GUILayout.Label(
                        "Attributes define a character's profile and determine how they interact within the " +
                        "game world. Traits will typically affect attributes directly.\n\n" +
                        "Examples of traits:\n" +
                        $"   {LIST_ITEM} Movement Speed\n" +
                        $"   {LIST_ITEM} Fortitude\n" +
                        $"   {LIST_ITEM} Holiness",
                        EditorStyles.wordWrappedLabel);

                    GUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        private void DrawSettingsMode()
        {
            
        }
        
        private void DrawCharacterSpeciesMode()
        {
            _speciesFilter =
                EditorGUILayout.TextField(
                    _speciesFilter,
                    filterFieldStyle);

            var collection = _characterManagerEditor.characterSpeciesConfigurationCollection;

            if (DrawCharacterDataCollection(
                collection,
                ref _speciesEditorScrollPosition,
                "Species",
                _speciesFilter,
                (characterData => DrawCharacterSpeciesAdditionalItemDetails(characterData as CharacterSpeciesConfiguration))))
            {
                _characterManagerEditor.characterSpeciesConfigurationCollection = collection;
                SetDirty();
            };
        }

        private void DrawCharacterClassMode()
        {
            _classFilter =
                EditorGUILayout.TextField(
                    _classFilter,
                    filterFieldStyle);

            var collection = _characterManagerEditor.characterClassConfigurationCollection;

            if (DrawCharacterDataCollection(
                collection,
                ref _classEditorScrollPosition,
                "Class",
                _classFilter,
                (characterData => false)))
            {
                _characterManagerEditor.characterClassConfigurationCollection = collection;
                SetDirty();
            };
        }

        private void DrawCharacterTraitMode()
        {
            _traitFilter =
                EditorGUILayout.TextField(
                    _traitFilter,
                    filterFieldStyle);

            var collection = _characterManagerEditor.characterTraitConfigurationCollection;

            if (DrawCharacterDataCollection(
                collection,
                ref _traitEditorScrollPosition,
                "Trait",
                _traitFilter,
                (characterData => DrawCharacterTraitAdditionalItemDetails(characterData as CharacterTraitConfiguration))))
            {
                _characterManagerEditor.characterTraitConfigurationCollection = collection;
                SetDirty();
            };
        }

        private void DrawCharacterAttributesMode()
        {
            _attributeFilter =
                EditorGUILayout.TextField(
                    _attributeFilter,
                    filterFieldStyle);

            var collection = _characterManagerEditor.characterAttributeConfigurationCollection;

            if (DrawCharacterDataCollection(
                collection,
                ref _attributeEditorScrollPosition,
                "Attribute",
                _attributeFilter,
                (characterData => DrawCharacterAttributeAdditionalItemDetails(characterData as CharacterAttributeConfiguration))))
            {
                _characterManagerEditor.characterAttributeConfigurationCollection = collection;
                SetDirty();
            };
        }

        private void DrawCharacterTestingMode()
        {
            if (_testCharacterProfile == null)
            {
                _testCharacterProfile = new CharacterProfile();
            }

            if (!CharacterProfileManager.Instance)
            {
                FlexibleLabel("Non CharacterManager instance. Try running the game.");
                return;
            }

            try
            {

                if (GUILayout.Button("ROLL", GUILayout.Height(50)))
                {
                    _testCharacterProfile = _characterManager.RollNewCharacter();
                }

                GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true));

                GUILayout.Label($"Name: {_testCharacterProfile.name}");
                GUILayout.Label($"Species: {_testCharacterProfile.species?.GetConfiguration()?.name ?? "NONE"}");

                GUILayout.Label("Attributes:");
                using (new EditorGUILayout.VerticalScope())
                {
                    foreach (var attribute in _testCharacterProfile.attributes)
                    {
                        GUILayout.Label($"\t{attribute.GetConfiguration().name}: {attribute.baseValue} + {attribute.calculatedValue} ({attribute.totalValue})");
                    }
                }

                GUILayout.Label("Traits:");
                GUILayout.BeginVertical();
                for (int i = 0; i < _testCharacterProfile.traits.Count; i++)
                {
                    if (i != 0)
                    {
                        GUILayout.Space(15);
                    }

                    var trait = _testCharacterProfile.traits[i];
                    var configuration = trait.GetConfiguration();
                    GUILayout.Label($"\t#{i} - {configuration.name}");
                    foreach (var attributeEffect in trait.attributeEffects)
                    {
                        var attributeConfiguration = attributeEffect.GetConfiguration();
                        var sign = attributeEffect.effectValue >= 0 ? "+" : "";
                        GUILayout.Label($"\t\t{sign}{attributeEffect.effectValue} {attributeConfiguration.name}");
                    }
                }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
            catch (Exception exception)
            {
                if (exception is ExitGUIException)
                {
                    throw;
                }
                
                Debug.LogException(exception);
                _testCharacterProfile = new CharacterProfile();
            }
        }
        
        #endregion
        
        #region Private Utilities

        private void RecordChange(string change)
        {
            Undo.RegisterCompleteObjectUndo(_characterManager, change);
            SetDirty();
        }
        
        private new void SetDirty()
        {
            EditorUtility.SetDirty(_characterManagerEditor.target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        private void ReplaceAllGuidOccurrences(string oldGuid, string newGuid)
        {
            ReplaceGuidInCollection(_characterManagerEditor.characterAttributeConfigurationCollection, oldGuid, newGuid);
            ReplaceGuidInCollection(_characterManagerEditor.characterClassConfigurationCollection, oldGuid, newGuid);
            ReplaceGuidInCollection(_characterManagerEditor.characterSpeciesConfigurationCollection, oldGuid, newGuid);
            ReplaceGuidInCollection(_characterManagerEditor.characterTraitConfigurationCollection, oldGuid, newGuid);
        }

        private void ReplaceGuidInCollection<T>(
            CharacterDataConfigurationCollection<T> collection, 
            string oldGuid,
            string newGuid)
            where T: CharacterDataConfiguration
        {
            foreach (var item in collection.items)
            {
                item.HandleGuidReplacement(oldGuid, newGuid);
            }
        }

        private bool DrawCharacterData(
            CharacterDataConfiguration characterDataConfiguration, 
            Func<CharacterDataConfiguration, bool> customProperties)
        {
            if (characterDataConfiguration == null)
            {
                return false;
            }
            
            var changeWasMade = false;
            var displayName = characterDataConfiguration.name.IsNullOrWhiteSpace()
                ? "UNNAMED ENTRY"
                : characterDataConfiguration.name;

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(true));

            GUILayout.Space(5);

            var icon = (Texture) EditorGUILayout.ObjectField(
                characterDataConfiguration.icon, 
                typeof(Texture), 
                false,
                GUILayout.Height(50),
                GUILayout.Width(50));

            GUILayout.Space(5);

            if (icon != characterDataConfiguration.icon)
            {
                RecordChange("Changed character data configuration icon.");

                characterDataConfiguration.icon = icon;
                changeWasMade = true;
            }
            
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            
            var showInEditor =
                EditorGUILayout.BeginFoldoutHeaderGroup(
                    characterDataConfiguration.showInEditor,
                    displayName);

            if (showInEditor != characterDataConfiguration.showInEditor)
            {
                characterDataConfiguration.showInEditor = showInEditor;
                Repaint();
            }
            
            // GUID

            GUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.TextField("GUID", characterDataConfiguration.guid);
            GUI.enabled = true;
            if (GUILayout.Button(new GUIContent("C", "Copy to clipboard."), GUILayout.ExpandWidth(false)))
            {
                EditorGUIUtility.systemCopyBuffer = characterDataConfiguration.guid;
            }
            if (GUILayout.Button(new GUIContent("R", "Randomize"), GUILayout.ExpandWidth(false)))
            {
                RecordChange("Item received new GUID.");
                
                var newGuid = Guid.NewGuid().ToString();
                var oldGuid = characterDataConfiguration.guid;

                if (oldGuid.IsNullOrWhiteSpace())
                {
                    characterDataConfiguration.guid = newGuid;
                }
                else
                {
                    ReplaceAllGuidOccurrences(oldGuid, newGuid);;
                }

                changeWasMade = true;
            }
            GUILayout.EndHorizontal();

            if (characterDataConfiguration.showInEditor)
            {
                // Name

                var name = EditorGUILayout.TextField("Name", characterDataConfiguration.name);
                if (name != characterDataConfiguration.name)
                {
                    RecordChange("Changed class details.");

                    changeWasMade = true;
                    characterDataConfiguration.name = name;
                }

                // Description

                GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                GUILayout.Label("Description", GUILayout.Width(147));
                var wordWrap = new GUIStyle(EditorStyles.textArea) {wordWrap = true};
                var description = EditorGUILayout.TextArea(characterDataConfiguration.description, wordWrap,
                    GUILayout.Height(50));
                if (description != characterDataConfiguration.description)
                {
                    RecordChange("Changed class details.");

                    changeWasMade = true;
                    characterDataConfiguration.description = description;
                }

                GUILayout.EndHorizontal();

                // Additional Details

                if (customProperties?.Invoke(characterDataConfiguration) == true)
                {
                    changeWasMade = true;
                }
                
                // Availability rules

                GUILayout.Space(15);
                
                characterDataConfiguration.showAvailabilityRulesInEditor =
                    EditorGUILayout.BeginToggleGroup(
                        "Show Availability Rules",
                        characterDataConfiguration.showAvailabilityRulesInEditor);

                if (characterDataConfiguration.showAvailabilityRulesInEditor)
                {
                    if (DrawAvailabilityRules(characterDataConfiguration.availabilityRules))
                    {
                        changeWasMade = true;
                    }
                }

                EditorGUILayout.EndToggleGroup();

                // GUILayout.Space(15);
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(15);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            return changeWasMade;
        }

        private bool DrawAvailabilityRules(List<ConfigurationAvailabilityRule> rules)
        {
            var changeWasMade = false;

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];

                if (i != 0)
                {
                    GUILayout.Space(10);
                }
                
                GUILayout.BeginHorizontal();
                GUILayout.Label($"#{i}", EditorStyles.boldLabel, GUILayout.Width(25));

                GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandHeight(true));
                GUILayout.BeginHorizontal();
                
                // Availability rule editing 
                
                var availability = (AvailabilityType) EditorGUILayout.EnumPopup(rule.availabilityType);
                if (availability != rule.availabilityType)
                {
                    RecordChange("Changed availability rule type.");
                    
                    rule.availabilityType = availability;
                    changeWasMade = true;
                }

                GUILayout.Label(" to ", GUILayout.ExpandWidth(false));

                var dataAvailability = (DataAvailabilityType) EditorGUILayout.EnumPopup(rule.dataAvailabilityType);
                if (dataAvailability != rule.dataAvailabilityType)
                {
                    RecordChange("Changed data availability type");

                    rule.dataAvailabilityType = dataAvailability;
                    changeWasMade = true;
                }
                
                GUILayout.EndHorizontal();
                
                // Availability rule GUIDs
                
                GUILayout.BeginVertical();

                for (int j = 0; j < rule.guids.Count; j++)
                {
                    var guid = rule.guids[j];

                    GUILayout.BeginHorizontal();
                    
                    switch (rule.dataAvailabilityType)
                    {
                        case DataAvailabilityType.None:
                            GUILayout.Label("No Availability Type Selected");
                            break;
                        case DataAvailabilityType.Class:
                            guid = DrawDataTypeDropdown(guid, _characterManagerEditor.characterClassConfigurationCollection);
                            break;
                        case DataAvailabilityType.Species:
                            guid = DrawDataTypeDropdown(guid, _characterManagerEditor.characterSpeciesConfigurationCollection);
                            break;
                    }

                    if (guid != rule.guids[j])
                    {
                        RecordChange("Changed availability rule guid.");

                        rule.guids[j] = guid;
                        changeWasMade = true;
                    }

                    if (GUILayout.Button(new GUIContent("X", "Delete"), GUILayout.ExpandWidth(false)))
                    {
                        RecordChange("Removed guid from availability rule.");

                        rule.guids.RemoveAt(j);
                        changeWasMade = true;
                    }
                    
                    GUILayout.EndHorizontal();
                }
                
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add GUID"))
                {
                    RecordChange("Added guid to availability rule.");

                    rule.guids.Add("");
                    changeWasMade = true;
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();

                GUILayout.Space(10);
                GUILayout.EndVertical();

                // Controls
                
                GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.Width(25));
                if (GUILayout.Button(new GUIContent("X", "Delete")))
                {
                    RecordChange("Removed availability rule.");
                    
                    rules.RemoveAt(i);
                    changeWasMade = true;
                }
                GUILayout.EndVertical();
                
                GUILayout.EndHorizontal();
            }

            if (rules.Count > 0)
            {
                GUILayout.Space(15);
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add New Availability Rule"))
                {
                    RecordChange("Added new availability rule.");

                    rules.Add(new ConfigurationAvailabilityRule());
                    changeWasMade = true;
                }
            }

            return changeWasMade;
        }

        private string DrawDataTypeDropdown<T>(string selectedGuid, CharacterDataConfigurationCollection<T> dataConfigurationCollection) 
            where T : CharacterDataConfiguration
        {
            var options = new List<GUIContent>();
            var values = new List<string>();

            options.Add(new GUIContent("NONE"));
            values.Add("");

            foreach (var item in dataConfigurationCollection.items)
            {
                var name = !item.name.IsNullOrWhiteSpace() ? item.name : "UNNAMED ENTRY";
                options.Add(new GUIContent(name));
                values.Add(item.guid);
            }

            var selectedIndex = values.IndexOf(selectedGuid);

            if (selectedIndex < 0 || selectedIndex >= options.Count)
            {
                selectedIndex = 0;
            }

            selectedIndex = EditorGUILayout.Popup(selectedIndex, options.ToArray());
            return values[selectedIndex];
        }

        /// <summary>
        /// Draws a <see cref="CharacterDataConfigurationCollection{T}"/> and returns whether or not a change was made.
        /// </summary>
        /// <param name="configurationCollection"></param>
        /// <param name="scrollPosition"></param>
        /// <param name="descriptiveClassName">The name of the class for recording change history and adding new items.
        /// </param>
        /// <param name="filter">The filter to use for delimiting items that are drawn.</param>
        /// <param name="drawAdditionalItemDetails"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Whether or not a change was made.</returns>
        private bool DrawCharacterDataCollection<T>(
            CharacterDataConfigurationCollection<T> configurationCollection,
            ref Vector2 scrollPosition,
            string descriptiveClassName,
            string filter,
            Func<CharacterDataConfiguration, bool> drawAdditionalItemDetails) 
            where T : CharacterDataConfiguration
        {
            var items = configurationCollection.items;
            var changeWasMade = false;
            var hasFilter = !filter.IsNullOrWhiteSpace();
            var modifiedFilter = hasFilter ? filter.ToLower().Trim() : null;

            if (GUILayout.Button($"Add New {descriptiveClassName}", GUILayout.Height(30)))
            {
                RecordChange($"Added new {descriptiveClassName}");
                changeWasMade = true;
                items.Add(default(T));
            }

            scrollPosition = GUILayout.BeginScrollView(
                scrollPosition,
                GUIStyle.none,
                GUI.skin.verticalScrollbar,
                GUILayout.ExpandHeight(true));
            
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                
                if (hasFilter && !item.name.IsNullOrWhiteSpace())
                {
                    var modifiedName = item.name.ToLower().Trim();
                    if (!modifiedFilter.Contains(modifiedName) &&
                        !modifiedName.Contains(modifiedFilter) &&
                        modifiedFilter != modifiedName)
                    {
                        continue;
                    }
                }
                
                GUILayout.BeginHorizontal();
                GUILayout.Label($"#{i}", EditorStyles.boldLabel, GUILayout.Width(25));

                if (DrawCharacterData(items[i], drawAdditionalItemDetails))
                {
                    changeWasMade = true;
                }
                
                GUILayout.BeginVertical(GUILayout.Width(25));
                GUILayout.Space(5);
                
                GUI.enabled = i != 0;
                if (GUILayout.Button(new GUIContent("UP", "Move Up")))
                {
                    RecordChange($"Moved {descriptiveClassName} item up.");
                    items.RemoveAt(i);
                    items.Insert(i - 1, item);
                    changeWasMade = true;
                }

                GUI.enabled = i < items.Count - 1;
                if (GUILayout.Button(new GUIContent("DN", "Move Down")))
                {
                    RecordChange($"Moved {descriptiveClassName} item up.");
                    items.RemoveAt(i);
                    items.Insert(i + 1, item);
                    changeWasMade = true;
                }

                GUI.enabled = true;
                if (GUILayout.Button(new GUIContent("X", "Delete")))
                {
                    RecordChange($"Removed {descriptiveClassName} item.");
                    items.RemoveAt(i);
                    i--;
                    changeWasMade = true;
                }
                
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndScrollView();

            if (changeWasMade)
            {
                configurationCollection.items = items;
            }

            return changeWasMade;
        }
        
        private bool DrawCharacterSpeciesAdditionalItemDetails(CharacterSpeciesConfiguration species)
        {
            var changeWasMade = false;
            
            var nameBuilder = 
                (CharacterNameBuilder) EditorGUILayout.ObjectField(
                    "Name Builder",
                    species.nameBuilder,
                    typeof(CharacterNameBuilder), 
                    false);

            if (nameBuilder != species.nameBuilder)
            {
                RecordChange("Changed species name builder.");

                changeWasMade = true;
                species.nameBuilder = nameBuilder;
            }

            species.showAttributeEffectsInEditor = EditorGUILayout.BeginToggleGroup("Show Base Attribute Effects",
                species.showAttributeEffectsInEditor);

            if (species.showAttributeEffectsInEditor && DrawAttributeEffects(species.baseAttributeEffects))
            {
                changeWasMade = true;
            }
            
            EditorGUILayout.EndToggleGroup();

            return changeWasMade;
        }

        private bool DrawCharacterTraitAdditionalItemDetails(CharacterTraitConfiguration trait)
        {
            var changeWasMade = false;

            GUILayout.Space(15);

            trait.showAttributeEffectsInEditor =
                EditorGUILayout.BeginToggleGroup(
                    "Show Attribute Effects", 
                    trait.showAttributeEffectsInEditor);

            if (trait.showAttributeEffectsInEditor)
            {
                if (DrawAttributeEffects(trait.attributeEffects))
                {
                    changeWasMade = true;
                }
            }
            
            EditorGUILayout.EndToggleGroup();
            
            trait.showTraitNegationsInEditor =
                EditorGUILayout.BeginToggleGroup(
                    "Show Negated Traits", 
                    trait.showTraitNegationsInEditor);

            if (trait.showTraitNegationsInEditor)
            {
                if (DrawCharacterTraitNegations(trait.negatedTraits))
                {
                    changeWasMade = true;
                }
            }
            
            EditorGUILayout.EndToggleGroup();

            return changeWasMade;
        }

        private bool DrawCharacterTraitNegations(List<string> negations)
        {
            DrawHelpBox("A negated trait is a trait that can't be randomly selected with this trait in the same character profile.");
            
            var changeWasMade = false;

            for (int i = 0; i < negations.Count; i++)
            {
                var negation = negations[i];
                
                if (i != 0)
                {
                    GUILayout.Space(10);
                }

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"#{i}", EditorStyles.boldLabel, GUILayout.Width(25));

                    using (new GUILayout.HorizontalScope(
                        GUI.skin.box, 
                        GUILayout.ExpandHeight(true)))
                    {
                        var newNegation = DrawDataTypeDropdown(
                            negation, 
                            _characterManagerEditor.characterTraitConfigurationCollection);
                        
                        if (newNegation != negation)
                        {
                            RecordChange("Changed trait negation");
                            
                            negations[i] = newNegation;

                            changeWasMade = true;
                        }
                    }

                    GUILayout.Space(15);
                    
                    if (GUILayout.Button(new GUIContent("X", "Delete"), GUILayout.ExpandWidth(false)))
                    {
                        RecordChange("Removed trait negation.");
                        
                        negations.RemoveAt(i);
                        
                        changeWasMade = true;
                    }
                }
            }

            if (negations.Count > 0)
            {
                GUILayout.Space(5);
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Negation"))
                {
                    RecordChange("Added negation.");

                    negations.Add("");
                    
                    changeWasMade = true;
                }
            }

            return changeWasMade;
        }

        private bool DrawAttributeEffects(List<CharacterAttributeEffectConfiguration> attributeEffects)
        {
            DrawHelpBox("Attribute effects can alter a character's base attributes through traits or provide the actual baseline value for a species.");
            
            var changeWasMade = false;

            for (int i = 0; i < attributeEffects.Count; i++)
            {
                var attributeEffect = attributeEffects[i];
                
                if (i != 0)
                {
                    GUILayout.Space(10);
                }

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"#{i}", EditorStyles.boldLabel, GUILayout.Width(25));

                    using (new GUILayout.VerticalScope(
                        GUI.skin.box, 
                        GUILayout.ExpandHeight(true)))
                    {
                        // Attribute GUID 
                        
                        attributeEffect.attributeGuid = DrawDataTypeDropdown(
                            attributeEffect.attributeGuid, 
                            _characterManagerEditor.characterAttributeConfigurationCollection);
                        
                        // Min Random Effect Value

                        var minRandomAttributeEffect =
                            EditorGUILayout.IntField(
                                "Min Attribute Value",
                                attributeEffect.minAttributeValue);

                        if (minRandomAttributeEffect != attributeEffect.minAttributeValue)
                        {
                            RecordChange("Changed min random attribute effect value.");

                            attributeEffect.minAttributeValue = minRandomAttributeEffect;

                            changeWasMade = true;
                        }
                        
                        // Max Random Effect Value
                        
                        var maxRandomAttributeEffect =
                            EditorGUILayout.IntField(
                                "Max Attribute Value",
                                attributeEffect.maxAttributeValue);

                        if (maxRandomAttributeEffect != attributeEffect.maxAttributeValue)
                        {
                            RecordChange("Changed max random attribute effect value.");

                            attributeEffect.maxAttributeValue = maxRandomAttributeEffect;

                            changeWasMade = true;
                        }
                    }

                    GUILayout.Space(15);
                    
                    if (GUILayout.Button(new GUIContent("X", "Delete"), GUILayout.ExpandWidth(false)))
                    {
                        RecordChange("Removed attribute effect.");
                        attributeEffects.RemoveAt(i);
                        changeWasMade = true;
                    }
                }
            }

            if (attributeEffects.Count > 0)
            {
                GUILayout.Space(5);
            }
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add New Attribute Effect"))
                {
                    RecordChange("Added new attribute effect.");

                    attributeEffects.Add(new CharacterAttributeEffectConfiguration());
                    changeWasMade = true;
                }
            }

            return changeWasMade;
        }

        private bool DrawCharacterAttributeAdditionalItemDetails(
            CharacterAttributeConfiguration characterAttributeConfiguration)
        {
            var changeWasMade = false;
            
            var minAttributeValue =
                EditorGUILayout.IntField("Min Attribute Value", characterAttributeConfiguration.minValue);

            if (minAttributeValue != characterAttributeConfiguration.minValue)
            {
                RecordChange("Changed attribute min value");

                characterAttributeConfiguration.minValue = minAttributeValue;
                changeWasMade = true;
            }
            
            var maxAttributeValue =
                EditorGUILayout.IntField("Max Attribute Value", characterAttributeConfiguration.maxValue);

            if (maxAttributeValue != characterAttributeConfiguration.maxValue)
            {
                RecordChange("Changed attribute max value");

                characterAttributeConfiguration.maxValue = maxAttributeValue;
                changeWasMade = true;
            }

            var stackable = EditorGUILayout.ToggleLeft("Stackable", characterAttributeConfiguration.stackable);

            if (stackable != characterAttributeConfiguration.stackable)
            {
                RecordChange("Changed attribute effect's stackable field");

                characterAttributeConfiguration.stackable = stackable;

                changeWasMade = true;
            }

            return changeWasMade;
        }

        private void DrawHelpBox(string message, float topMargin = 5, float bottomMargin = 5)
        {
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Space(topMargin);
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label(EditorGUIUtility.IconContent("_Help"), GUILayout.ExpandWidth(false));
                    GUILayout.Label(message, EditorStyles.wordWrappedMiniLabel);
                }
                GUILayout.Space(bottomMargin);
            }
        }

        private void FlexibleLabel(string label)
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

        #endregion
        
        #region Events

        private void OnUndoRedoPerformed()
        {
            if (_characterManagerEditor)
            {
                if (_characterManagerEditor.target != _characterManager)
                {
                    _characterManager = _characterManagerEditor.target as CharacterProfileManager;
                }
            }
            else if (_characterManager)
            {
                _characterManager = null;
            }

            if (_characterManagerEditor)
            {
                _characterManagerEditor.serializedObject.Update();
                SetDirty();
            }

            Repaint();
        }
        
        #endregion

        #region Supporting Types

        private enum CharacterDataType
        {
            Class,
            Species,
            Trait,
        }

        #endregion
    }
}
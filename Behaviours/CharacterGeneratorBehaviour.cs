using System.Collections.Generic;
using System.Linq;
using CharacterGenerator.Configuration;
using CharacterGenerator.Instancing;
using CharacterGenerator.Utility;
using UnityEngine;

namespace CharacterGenerator.Behaviours
{
    public class CharacterGeneratorBehaviour : MonoBehaviour
    {
        #region Private

        private readonly IDictionary<string, CharacterDataConfiguration> _allConfigurations =
            new Dictionary<string, CharacterDataConfiguration>();

        #endregion

        #region Static

        public static CharacterGeneratorBehaviour instance { get; private set; }

        #endregion

        #region Initialization

        private void OnEnable()
        {
            if (instance && instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            instance = this;

            if (_dontDestroyOnLoad) DontDestroyOnLoad(this);

            InitializeDictionary();
        }

        #endregion

        #region Private Utilities

        private void InitializeDictionary()
        {
            foreach (var item in _classConfigurationCollection.items) _allConfigurations.Add(item.guid, item);

            foreach (var item in _speciesConfigurationCollection.items) _allConfigurations.Add(item.guid, item);

            foreach (var item in _traitConfigurationCollection.items) _allConfigurations.Add(item.guid, item);

            foreach (var item in _attributeConfigurationCollection.items) _allConfigurations.Add(item.guid, item);
        }

        #endregion

        #region Settings

        [SerializeField] private bool _dontDestroyOnLoad = true;

        [SerializeField]
        private CharacterClassConfigurationCollection _classConfigurationCollection =
            new CharacterClassConfigurationCollection();

        [SerializeField] private CharacterSpeciesConfigurationCollection _speciesConfigurationCollection =
            new CharacterSpeciesConfigurationCollection();

        [SerializeField]
        private CharacterTraitConfigurationCollection _traitConfigurationCollection =
            new CharacterTraitConfigurationCollection();

        [SerializeField] private CharacterAttributeConfigurationCollection _attributeConfigurationCollection =
            new CharacterAttributeConfigurationCollection();

        #endregion

        #region Public Utilities

        public CharacterDataConfiguration GetConfiguration(string guid)
        {
            return _allConfigurations?[guid];
        }

        public T GetConfiguration<T>(string guid) where T : CharacterDataConfiguration
        {
            return GetConfiguration(guid) as T;
        }

        public CharacterSpeciesConfiguration GetRandomSpecies(List<CharacterSpeciesConfiguration> exclude = null)
        {
            return
                (exclude != null
                    ? _speciesConfigurationCollection?.items?
                        .Where(item => !exclude.Contains(item))
                        .ToList()
                    : _speciesConfigurationCollection?.items)
                .Random();
        }

        public CharacterClassConfiguration GetRandomClass(CharacterSpeciesConfiguration species = null)
        {
            return
                _classConfigurationCollection?.items?
                    .Where(item =>
                    {
                        if (species != null)
                            return species.CheckCompatibility(item) &&
                                   item.CheckCompatibility(species);

                        return true;
                    })
                    .Random();
        }

        public CharacterTraitConfiguration GetRandomTrait(
            List<CharacterTraitConfiguration> existingTraits,
            CharacterSpeciesConfiguration existingSpecies)
        {
            var items = _traitConfigurationCollection.items.Where(trait =>
            {
                if (existingTraits != null && existingTraits.Count > 0)
                    for (var i = 0; i < existingTraits.Count; i++)
                        if (existingTraits[i].guid == trait.guid ||
                            existingTraits[i].CheckNegation(trait) ||
                            trait.CheckNegation(existingTraits[i]))
                            return false;

                if (existingSpecies != null &&
                    !existingSpecies.CheckCompatibility(trait) ||
                    !trait.CheckCompatibility(existingSpecies))
                    return false;

                return true;
            });

            return !items.Any() ? null : items.ToList().Random();
        }

        public CharacterProfile RollNewCharacter()
        {
            var character = new CharacterProfile();
            var randomSpecies = GetRandomSpecies();
            var randomClass = GetRandomClass();
            var randomTraitCount = Random.Range(randomSpecies.minTraitCount, randomSpecies.maxTraitCount);
            var randomTraits = new List<CharacterTraitConfiguration>();

            character.name = randomSpecies.nameBuilder.CreateRandomName();
            character.species = new CharacterSpeciesInstance(randomSpecies);
            character.mainClass = new CharacterClassInstance(randomClass);

            for (var i = 0; i < randomSpecies.baseAttributeEffects.Count; i++)
            {
                var baseAttributeEffect = randomSpecies.baseAttributeEffects[i];

                character.attributes.Add(new CharacterAttributeInstance
                {
                    configurationGuid = baseAttributeEffect.attributeGuid,
                    baseValue = Random.Range(baseAttributeEffect.minAttributeValue,
                        baseAttributeEffect.maxAttributeValue)
                });
            }

            for (var i = 0; i < randomTraitCount; i++)
            {
                var randomTrait = GetRandomTrait(randomTraits, randomSpecies);

                if (randomTrait == null) continue;

                randomTraits.Add(randomTrait);
                character.traits.Add(new CharacterTraitInstance(randomTrait));
            }

            character.RecalculateTraitAttributes();

            return character;
        }

        #endregion
    }
}
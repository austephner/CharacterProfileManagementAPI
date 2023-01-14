using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterGenerator.Configuration
{
    [Serializable]
    public class Species : CharacterConfigurationEntity
    {
        /// <summary>
        /// All name builders available for characters of this species.
        /// </summary>
        public List<NameBuilder> characterNameBuilders = new List<NameBuilder>();

        public string GenerateName() => characterNameBuilders.Random()?.GenerateName();

        #if UNITY_EDITOR
        
        [HideInInspector] public bool expandedInEditor = false;
        
        #endif
    }
}
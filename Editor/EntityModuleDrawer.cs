using System;
using System.Collections.Generic;
using CharacterGenerator.Configuration;
using UnityEditor;
using UnityEngine;

namespace CharacterGenerator.Editor
{
    public static class EntityModuleDrawer
    {
        private static string _filter;

        private static IDictionary<Type, Type> _moduleDrawingDetails = new Dictionary<Type, Type>();

        public static void DrawModuleSection(EntityModule entityModule)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label(entityModule.displayName, EditorStyles.largeLabel);
            }
        }
    }
}
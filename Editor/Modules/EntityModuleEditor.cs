using System;
using CharacterGenerator.Configuration;

namespace CharacterGenerator.Editor.Modules
{
    public abstract class EntityModuleEditor
    {
        public abstract Type moduleType { get; }

        public abstract void DrawModule(
            EntityModule module, 
            CharacterGeneratorConfigurationEditor characterGeneratorConfigurationEditor,
            Action<string> setDirty); 
    }
}
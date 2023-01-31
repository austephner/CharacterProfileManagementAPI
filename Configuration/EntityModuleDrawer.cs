using System;

namespace CharacterGenerator.Configuration
{
    [Obsolete("The module drawer class and custom window are currently under revision.")]
    public abstract class EntityModuleDrawer
    {
        public abstract Type moduleType { get; }

        public virtual int order => 1;

        public abstract void DrawModule(
            EntityModule module,
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action setDirty,
            Action<string> recordChange,
            Action<string, string> onEntityGuidChange); 
    }
}
﻿using System;

namespace CharacterGenerator.Configuration
{
    public abstract class EntityModuleDrawer
    {
        public abstract Type moduleType { get; }

        public abstract void DrawModule(
            EntityModule module,
            CharacterGeneratorConfiguration characterGeneratorConfiguration,
            Action<string> setDirty,
            Action<string, string> handleEntityGuidChange); 
    }
}
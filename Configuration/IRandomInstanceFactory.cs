using System;

namespace CharacterGenerator.Configuration
{
    [Obsolete]
    public interface IRandomInstanceFactory<out T>
    {
        public T CreateRandomInstance();
    }
}
namespace CharacterGenerator.Configuration
{
    public interface IRandomInstanceFactory<out T>
    {
        public T CreateRandomInstance();
    }
}
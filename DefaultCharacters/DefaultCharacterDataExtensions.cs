using CharacterGenerator.Instancing;

namespace CharacterGenerator.DefaultCharacters
{
    public static class DefaultCharacterDataExtensions
    {
        public static DefaultCharacterData ToDefaultCharacterData(this CharacterData characterData)
        {
            return new DefaultCharacterData(characterData);
        }
    }
}
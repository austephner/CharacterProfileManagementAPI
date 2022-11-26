using CharacterGenerator.Configuration;

namespace CharacterGenerator.Instancing
{
    public class CharacterClassInstance : CharacterDataInstance<CharacterClassConfiguration>
    {
        public CharacterClassInstance(CharacterClassConfiguration configuration)
        {
            configurationGuid = configuration.guid;
        }
    }
}
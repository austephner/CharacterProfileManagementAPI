using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    public class CharacterClassInstance : CharacterDataInstance<CharacterClassConfiguration>
    {
        public CharacterClassInstance(CharacterClassConfiguration configuration)
        {
            configurationGuid = configuration.guid;
        }
    }
}
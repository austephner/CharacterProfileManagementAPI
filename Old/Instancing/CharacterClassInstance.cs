using CharacterGenerator.Old.Configuration;

namespace CharacterGenerator.Old.Instancing
{
    public class CharacterClassInstance : CharacterDataInstance<CharacterClassConfiguration>
    {
        public CharacterClassInstance(CharacterClassConfiguration configuration)
        {
            configurationGuid = configuration.guid;
        }
    }
}
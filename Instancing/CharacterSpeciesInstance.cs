using CharacterProfileManagement.Configuration;

namespace CharacterProfileManagement.Instancing
{
    public class CharacterSpeciesInstance : CharacterDataInstance<CharacterSpeciesConfiguration>
    {
        public CharacterSpeciesInstance(CharacterSpeciesConfiguration characterSpeciesConfiguration)
        {
            configurationGuid = characterSpeciesConfiguration.guid;
        }
    }
}
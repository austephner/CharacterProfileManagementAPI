using CharacterGenerator.Configuration;

namespace CharacterGenerator.Instancing
{
    public class CharacterSpeciesInstance : CharacterDataInstance<CharacterSpeciesConfiguration>
    {
        public CharacterSpeciesInstance(CharacterSpeciesConfiguration characterSpeciesConfiguration)
        {
            configurationGuid = characterSpeciesConfiguration.guid;
        }
    }
}
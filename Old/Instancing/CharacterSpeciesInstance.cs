using CharacterGenerator.Old.Configuration;

namespace CharacterGenerator.Old.Instancing
{
    public class CharacterSpeciesInstance : CharacterDataInstance<CharacterSpeciesConfiguration>
    {
        public CharacterSpeciesInstance(CharacterSpeciesConfiguration characterSpeciesConfiguration)
        {
            configurationGuid = characterSpeciesConfiguration.guid;
        }
    }
}
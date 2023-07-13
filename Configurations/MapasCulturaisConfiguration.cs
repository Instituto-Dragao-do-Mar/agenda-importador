namespace mapasculturais_service.Configurations;

public class MapasCulturaisConfiguration
{
    #region Endpoints

    public readonly string BaseUrl = "https://mapacultural.secult.ce.gov.br/api/";
    
    public readonly string Agents = "agent/find";
    public readonly string Events = "event/find";

    public readonly string AgentsIds = @"5513,5975,7277,7281,7282,7283,7284,7286,7288,7289,7305,9487,17103,28527,34283,98599,99383,100711,101129,101334,109561,117345,117346,117347,117348,117349,117350,117351,117352,117353,117354,117355,117356,117357";
    public readonly string HighlightedAgentsIds = "5513,7283,9487,17103,28527,34283,98599,99383,99610,100711,101129,101334,117345,117346,117347,117348,117349,117350,117351,117352";
    
    public readonly string Spaces = "space/find";
    public readonly string SpacesSelectParameters = "id, location, name, public, shortDescription, createTimestamp, updateTimestamp," +
                                                    " terms, En_CEP, En_Nome_Logradouro, En_Num, En_Complemento, En_Bairro, En_Municipio, En_Estado," +
                                                    " site, facebook, instagram, horario, endereco, acessibilidade, acessibilidade_fisica, parent";
    public readonly string SpaceTypes = "space/getTypes";
    
    public readonly string Occurences = "event/findOccurrences";

    #endregion
}
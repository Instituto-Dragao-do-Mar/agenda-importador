using System.Data.SqlClient;
using System.Text.RegularExpressions;
using mapasculturais_service.Entities;
using mapasculturais_service.Interfaces;

// ReSharper disable UnusedVariable

namespace mapasculturais_service.Services;

public class DatabaseConnectionService : IDatabaseConnectionService
{
    private IConfiguration _configuration;
    private readonly ILogger<DatabaseConnectionService> _logger;

    public DatabaseConnectionService(IConfiguration configuration, ILogger<DatabaseConnectionService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private async Task SaveEntity(BaseEntity entity, SqlConnection connection, Agent? agent = null)
    {
        var entityCount = await CheckIfEntityIsOnDatabase(connection, entity);

        if (entityCount > 0)
        {
            switch (entity)
            {
                case Occurrence occurrence: return;
            }


            //Comentar para atualizar constantemente

            var isUpdated = await CheckIfEntityIsUpdatedByTimestamp(connection, entity);
            // if (isUpdated > 0)
            // {
            await ExecuteUpdateEntityCommand(connection, entity, agent);

            switch (entity)
            {
                case Space space:
                    await UpdateSpaceImages(connection, space);
                    break;
                case Event @event:
                    await UpdateEventCategories(connection, @event);
                    await UpdateEventImages(connection, @event);
                    await UpdateEventAgeRating(connection, @event);
                    break;
                // case Occurrence ocurrence:
                //     await UpdateOcurrence(connection, ocurrence);
                //     break;
            }
            // }

            return;
        }

        if (entityCount == 0)
        {
            switch (entity)
            {
                case Occurrence occurrence:
                    occurrence.EventId =
                        await GetEventIdBySelect(connection, new Event { Id = occurrence.EventId.Value });
                    occurrence.Space.Id = await GetSpaceIdBySelect(connection, new Space { Id = occurrence.Space.Id });
                    await ExecuteSaveEntityCommand(connection, occurrence);
                    break;
                // switch (entity)
                // {
                case Space space:
                    await ExecuteSaveEntityCommand(connection, space);
                    await SaveSpaceImages(connection, space);
                    break;
                case Event @event:
                    await ExecuteSaveEntityCommand(connection, @event);
                    await UpdateEventCategories(connection, @event);
                    await UpdateEventImages(connection, @event);
                    await UpdateEventAgeRating(connection, @event);
                    break;
                // }
                default:
                    await ExecuteSaveEntityCommand(connection, entity, agent);
                    break;
            }
        }
    }

    private async Task SaveEventAgeRating(SqlConnection connection, Event @event)
    {
        var ageRating = @event.ClassificacaoEtaria;
        if (string.IsNullOrEmpty(ageRating)) return;

        var ageRatingId = await GetAgeRatingIdBySelect(connection, ageRating);
        if (ageRatingId == 0) return;

        var eventId = await GetEventIdBySelect(connection, @event);
        if (eventId == 0) return;

        var insertEventAgeRating =
            $"if not exists (select * from eventosclassificacao where idevento = {eventId} and idclassificacao = {ageRatingId})" +
            $"insert into eventosclassificacao (idevento, idclassificacao) values ({eventId}, {ageRatingId})";

        await ExecuteDbCommand(insertEventAgeRating, connection);
    }

    private async Task UpdateEventAgeRating(SqlConnection connection, Event @event)
    {
        var ageRating = @event.ClassificacaoEtaria;
        if (string.IsNullOrEmpty(ageRating)) return;

        var ageRatingId = await GetAgeRatingIdBySelect(connection, ageRating);
        if (ageRatingId == 0) return;

        var eventId = await GetEventIdBySelect(connection, @event);
        if (eventId == 0) return;

        var updateEventAgeRating =
                // $"begin " +
                $"if exists (select * from eventosclassificacao where idevento = {eventId} and idclassificacao <> {ageRatingId}) " +
                $"begin update eventosclassificacao set idclassificacao = {ageRatingId} where idevento = {eventId} end" +
                // +
                $" else begin insert into eventosclassificacao (idevento, idclassificacao) values ({eventId}, {ageRatingId}) end"
            ;

        await ExecuteDbCommand(updateEventAgeRating, connection);
    }

    private async Task<int> GetAgeRatingIdBySelect(SqlConnection connection, string ageRating)
    {
        var selectEventIdString =
            $"select top(1) c.id from classificacao c where c.nome like '%{ageRating}%'";

        return int.Parse(await ExecuteScalarDBCommand(selectEventIdString, connection));
    }

    private async Task SaveEventCategories(SqlConnection connection, Event @event)
    {
        var linguagens = @event.Terms?.Linguagem;
        if (linguagens == null) return;

        foreach (var linguagem in linguagens)
        {
            var categoryId = await GetCategoryIdBySelect(connection, linguagem);
            if (categoryId == 0) continue;

            var eventId = await GetEventIdBySelect(connection, @event);
            if (eventId == 0) continue;

            var insertEventCategoryString =
                $"if exists (select * from eventoscategorias where idevento = {eventId} and idcategoria <> '{categoryId}') " +
                $"begin update eventoscategorias set idcategoria = '{categoryId}' where idevento = {eventId} end"
                +
                $" else begin insert into eventoscategorias (idevento, idcategoria) values ({eventId}, '{categoryId}') end";

            await ExecuteDbCommand(insertEventCategoryString, connection);
        }
    }

    private async Task SaveEvent(SqlConnection connection, Event @event)
    {
        var linguagens = @event.Terms?.Linguagem;
        if (linguagens == null) return;

        foreach (var linguagem in linguagens)
        {
            var categoryId = await GetCategoryIdBySelect(connection, linguagem);
            if (categoryId == 0) continue;

            var eventId = await GetEventIdBySelect(connection, @event);
            if (eventId == 0) continue;

            var insertEventCategoryString =
                $"if exists (select * from eventoscategorias where idevento = {eventId} and idcategoria <> '{categoryId}') " +
                $"begin update eventoscategorias set idcategoria = '{categoryId}' where idevento = {eventId} end"
                +
                $" else begin insert into eventoscategorias (idevento, idcategoria) values ({eventId}, '{categoryId}') end";

            await ExecuteDbCommand(insertEventCategoryString, connection);
        }
    }

    private async Task UpdateEventCategories(SqlConnection connection, Event @event)
    {
        var linguagens = @event.Terms?.Linguagem;
        if (linguagens == null) return;

        foreach (var linguagem in linguagens)
        {
            var categoryId = await GetCategoryIdBySelect(connection, linguagem);
            if (categoryId == 0) continue;

            var eventId = await GetEventIdBySelect(connection, @event);
            if (eventId == 0) continue;

            var insertEventCategoryString =
                $"if not exists (select * from eventoscategorias where idevento = {eventId} and idcategoria = {categoryId}) " +
                $"insert into eventoscategorias (idevento, idcategoria) values ({eventId}, {categoryId}) " +
                $"else update eventoscategorias set idevento = '{eventId}', idcategoria = '{categoryId}' where idevento = {eventId}";

            await ExecuteDbCommand(insertEventCategoryString, connection);
        }
    }

    private async Task SaveSpaceImages(SqlConnection connection, Space space)
    {
        var imagem = space.FilesAvatar;
        if (imagem == null) return;

        var saveImageString = $@"if not exists(select * from imagens where url = '{imagem.url}') " +
                              $@"begin insert into imagens (url, tipo) values ('{imagem.url}', 'U') end";

        await ExecuteDbCommand(saveImageString, connection);

        var imageId = await GetImageIdBySelect(connection, imagem.url);
        if (string.IsNullOrEmpty(imageId)) return;

        var spaceId = await GetSpaceIdBySelect(connection, space);
        if (spaceId == 0) return;

        var insertSpaceImageString =
            $"if not exists (select * from espacosimagens where idespaco = '{spaceId}' and idimagem = '{imageId}') " +
            $"begin insert into espacosimagens (idespaco, idimagem) values ({spaceId}, '{imageId}') end";

        await ExecuteDbCommand(insertSpaceImageString, connection);
    }

    private async Task UpdateSpaceImages(SqlConnection connection, Space space)
    {
        var imagem = space.FilesAvatar;
        if (imagem == null) return;

        var saveImageString = $@"if not exists(select * from imagens where url = '{imagem.url}') " +
                              $@"begin insert into imagens (url, tipo) values ('{imagem.url}', 'U') end";

        await ExecuteDbCommand(saveImageString, connection);

        var imageId = await GetImageIdBySelect(connection, imagem.url);
        if (string.IsNullOrEmpty(imageId)) return;

        var spaceId = await GetSpaceIdBySelect(connection, space);
        if (spaceId == 0) return;

        var updateSpaceImageString =
                // $"begin" +
                $" if exists (select * from espacosimagens where idespaco = {spaceId} and idimagem = '{imageId}') " +
                $"begin update espacosimagens set idimagem = '{imageId}' where idespaco = {spaceId} end" +
                //     + 
                $" else begin insert into espacosimagens (idespaco, idimagem) values ({spaceId}, '{imageId}') end"
            ;

        await ExecuteDbCommand(updateSpaceImageString, connection);
    }

    private async Task SaveEventImages(SqlConnection connection, Event @event)
    {
        var imagem = @event.FilesAvatar;
        if (imagem == null) return;

        var saveImageString = $@"if not exists (select * from imagens where url = '{imagem.url}') " +
                              $@"begin insert into imagens (url, tipo) values ('{imagem.url}', 'U') end";

        await ExecuteDbCommand(saveImageString, connection);

        var imageId = await GetImageIdBySelect(connection, imagem.url);
        if (string.IsNullOrEmpty(imageId)) return;

        var eventId = await GetEventIdBySelect(connection, @event);
        if (eventId == 0) return;

        var saveEventImageString =
            $"if not exists (select * from eventosimagens where idevento = {eventId} and idimagem = '{imageId}') " +
            $"begin insert into eventosimagens (idevento, idimagem) values ({eventId}, '{imageId}') end";

        await ExecuteDbCommand(saveEventImageString, connection);
    }

    // private async Task UpdateOcurrence(SqlConnection connection, Occurrence occurrence)
    // {
    //     var saveImageString = $@"if not exists (select * from imagens where url = '{imagem.url}') " +
    //                           $@"begin insert into imagens (url, tipo) values ('{imagem.url}', 'U') end";
    //
    //     await ExecuteDbCommand(saveImageString, connection);
    //
    //     var imageId = await GetImageIdBySelect(connection, imagem.url);
    //     if (string.IsNullOrEmpty(imageId)) return;
    //
    //     var eventId = await GetEventIdBySelect(connection, @event);
    //     if (eventId == 0) return;
    //
    //     var updateEventImageString =
    //             // $"begin " +
    //             $"if exists (select * from eventosimagens where idevento = {eventId} and idimagem <> '{imageId}') " +
    //             $"begin update eventosimagens set idimagem = '{imageId}' where idevento = {eventId} end" 
    //         //     +
    //         // $" else insert into eventosimagens (idevento, idimagem) values ({eventId}, '{imageId}') end"
    //         ;
    //
    //     await ExecuteDbCommand(connection, updateEventImageString);
    // }
    private async Task UpdateEventImages(SqlConnection connection, Event @event)
    {
        var imagem = @event.FilesAvatar;
        var imageId = "";
        var saveImageString = "";
        if (imagem == null)
        {
            imageId = await GetCategoryImageId(connection, @event);
        }
        else
        {
            saveImageString = $@"if not exists (select * from imagens where url = '{imagem.url}') " +
                              $@"begin insert into imagens (url, tipo) values ('{imagem.url}', 'U') end";

            await ExecuteDbCommand(saveImageString, connection);
        }

        if (imageId == "" && imagem != null)
            imageId = await GetImageIdBySelect(connection, imagem.url);
        
        if (string.IsNullOrEmpty(imageId)) return;

        var eventId = await GetEventIdBySelect(connection, @event);
        if (eventId == 0) return;

        var updateEventImageString =
                // $"begin " +
                $"if exists (select * from eventosimagens where idevento = {eventId} and idimagem = '{imageId}') " +
                $"begin update eventosimagens set idimagem = '{imageId}' where idevento = {eventId} end"
                +
                $" else begin insert into eventosimagens (idevento, idimagem) values ({eventId}, '{imageId}') end"
            ;

        await ExecuteDbCommand(updateEventImageString, connection);
    }

    private async Task<String> GetCategoryImageId(SqlConnection connection, Event @event)
    {
        var selectEventIdString =
            $"SELECT top(1) idimagem from categorias where nome = '{@event.Terms.Linguagem.FirstOrDefault()}'";

        return await ExecuteScalarDBCommand(selectEventIdString, connection);
    }

    private async Task<int> GetEventIdBySelect(SqlConnection connection, Event @event)
    {
        var selectEventIdString =
            $"SELECT top(1) id from eventos where idexterno = {@event.Id}";

        return int.Parse(await ExecuteScalarDBCommand(selectEventIdString, connection));
    }

    private async Task<int> GetSpaceIdBySelect(SqlConnection connection, Space space)
    {
        var selectSpaceIdString =
            $"SELECT top(1) id from espacos where idexterno = {space.Id}";

        return int.Parse(await ExecuteScalarDBCommand(selectSpaceIdString, connection));
    }

    private async Task<string> GetImageIdBySelect(SqlConnection connection, string url)
    {
        var selectImageIdString =
            $@"SELECT top(1) i.id from imagens i where i.url = '{url}'";

        return await ExecuteScalarDBCommand(selectImageIdString, connection);
    }

    private async Task<int?> GetCategoryIdBySelect(SqlConnection connection, string linguagem)
    {
        var selectCategoryIdString =
            $"SELECT TOP(1) c.id from categoriaslinguagens l inner join categorias c on l.idcategoria = c.id where l.descricao like '%{linguagem}%'";

        var id = await ExecuteScalarDBCommand(selectCategoryIdString, connection);

        return int.Parse(!string.IsNullOrEmpty(id) ? id : "0");
    }

    public int GetSpaceIdByExternalId(SqlConnection connection, int? idexterno)
    {
        if (idexterno is null or 0) return 0;
        var id = "";
        var selectCategoryIdString =
            $"SELECT TOP(1) e.id from espacos e where e.idexterno = {idexterno}";

        id = ExecuteScalarDBCommand(selectCategoryIdString, connection).Result;

        return int.Parse(!string.IsNullOrEmpty(id) ? id : "0");
    }

    // private async Task ExecuteDbCommand(SqlConnection connection, string insertEventCategoryString)
    // {
    //     await ExecuteDbCommand(insertEventCategoryString, connection);
    // }

    private async Task ExecuteUpdateEntityCommand(SqlConnection connection, BaseEntity entity, Agent? agent = null)
    {
        var tableName = GetTableName(entity);
        var columns = GetTableColumns(entity);
        var values = GetTableValues(entity, agent, connection);
        var entityExternalId = GetEntityExternalId(entity);

        var columnsList = columns.Split(",");
        var valuesList = Regex.Split(values, ",(?=(?:[^']*'[^']*')*[^']*$)");

        var setString = "";
        for (var i = 0; i < columnsList.Length; i++)
        {
            var column = columnsList[i];
            var value = valuesList[i];

            setString += $"{column} = {value},";
        }

        setString = setString.Remove(setString.Length - 1, 1);

        var updateCommandString = $@"update {tableName} set {setString} where idexterno = {entityExternalId}";

        await ExecuteDbCommand(updateCommandString, connection);
    }

    private async Task ExecuteSaveEntityCommand(SqlConnection connection, BaseEntity entity, Agent? agent = null)
    {
        var tableName = GetTableName(entity);
        var columns = GetTableColumns(entity);
        var values = GetTableValues(entity, agent, connection);

        var insertCommandString = $@"insert into {tableName} ({columns}) values ({values})";

        await ExecuteDbCommand(insertCommandString, connection);
    }

    private async Task<int> CheckIfEntityIsUpdatedByTimestamp(SqlConnection connection, BaseEntity entity)
    {
        var checkIfEntityIsUpdated = GetCheckIfUpdatedEntityString(entity);
        if (checkIfEntityIsUpdated == "") return 0;

        return int.Parse(await ExecuteScalarDBCommand(checkIfEntityIsUpdated, connection));
    }

    private async Task<int> CheckIfEntityIsOnDatabase(SqlConnection connection, BaseEntity entity)
    {
        var checkIfExistsCommandString = GetCheckIfExistsString(entity);
        return int.Parse(await ExecuteScalarDBCommand(checkIfExistsCommandString, connection));
    }

    public async Task SaveSpaceListToDatabase(List<Space> entities, Agent? agent = null)
    {
        try
        {
            _logger.LogInformation("Iniciando importação para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            var connection = new SqlConnection(connectionString: _configuration.GetConnectionString("Default"));
            await connection.OpenAsync();

            foreach (var entity in entities)
            {
                // if (entity.Id > 6732)
                    await SaveEntity(entity, connection, agent);
            }

            _logger.LogInformation("Importação concluída para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            ShowExceptionLog(e);
            throw;
        }
    }

    private async Task ExecuteDbCommand(string commandString, SqlConnection connection)
    {
        await using var sqlCommand = new SqlCommand(commandString, connection);
        try
        {
            _logger.LogInformation("Executando comando \'{SqlCommand}\': {Now}\n", commandString,
                DateTimeOffset.Now);
            await sqlCommand.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogInformation("Erro ao executar o comando \'{SqlCommand}\': {Now}", commandString,
                DateTimeOffset.Now);
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<string> ExecuteScalarDBCommand(string commandString, SqlConnection connection)
    {
        await using var sqlCommand = new SqlCommand(commandString, connection);
        try
        {
            _logger.LogInformation("Executando comando \'{SqlCommand}\': {Now}\n", commandString,
                DateTimeOffset.Now);
            var sqlReturn = (await sqlCommand.ExecuteScalarAsync())?.ToString();
            return sqlReturn ?? "";
        }
        catch (Exception e)
        {
            _logger.LogInformation("Erro ao executar o comando \'{SqlCommand}\': {Now}", commandString,
                DateTimeOffset.Now);
            Console.WriteLine(e);
            throw;
        }
    }

    private void ShowExceptionLog(Exception exception)
    {
        _logger.LogInformation("Ocorreu um erro na importação: {ExceptionMessage}", exception.Message);
        Console.WriteLine(exception.Message);
    }

    public async Task SaveEventListToDatabase(List<Event> entities, Agent? agent = null)
    {
        try
        {
            _logger.LogInformation("Iniciando importação para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            var connection = new SqlConnection(connectionString: _configuration.GetConnectionString("Default"));
            await connection.OpenAsync();

            // var eventsIds = "(" + string.Join(",", entities.Select(e => e.Id.ToString()).ToList()) + ")";

            var range = Enumerable.Range(1, entities.Select(e => e.Id).Max());
            var idsToExclude = "(" + string.Join(",", range.Where(e => !entities.Select(en => en.Id).Contains(e))) +
                               ")";

            var excludeCommandString = $@"update eventos set excluido = 1 where idexterno in {idsToExclude}";

            await ExecuteDbCommand(excludeCommandString, connection);

            foreach (var entity in entities)
            {
                if (entity.Id > 9860)
                    await SaveEntity(entity, connection, agent);
            }

            _logger.LogInformation("Importação concluída para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            ShowExceptionLog(e);
            throw;
        }
    }

    public async Task SaveOccurrenceListToDatabase(List<Occurrence> entities)
    {
        try
        {
            _logger.LogInformation("Iniciando importação para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            var connection = new SqlConnection(connectionString: _configuration.GetConnectionString("Default"));
            await connection.OpenAsync();

            foreach (var entity in entities)
            {
                await SaveEntity(entity, connection);
            }

            _logger.LogInformation("Importação concluída para a tabela \'{TableName}\': {Now}",
                GetTableName(entities[0]), DateTimeOffset.Now);
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            ShowExceptionLog(e);
            throw;
        }
    }

    // public Task UpdateSpacesSetVerifiedByAgent(List<int> spacesIdsLists)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task UpdateEventsSetVerifiedByAgent(List<int> eventsIdsLists)
    // {
    //     throw new NotImplementedException();
    // }

    private static string GetCheckIfExistsString(BaseEntity entity)
    {
        var tableName = GetTableName(entity);
        try
        {
            switch (entity)
            {
                case Space space:
                    return $@"select count(*) from {tableName} where idexterno={space.Id}";
                case Event @event:
                    return
                        $@"select count(*) from {tableName} where idexterno={@event.Id}";
                case Occurrence occurrence:
                    return
                        $@"select count(*) from {tableName} where idexterno={occurrence.OccurrenceId} and datahora='{occurrence.StartsOn} {occurrence.StartsAt}'";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return "";
    }


    private static string GetCheckIfUpdatedEntityString(BaseEntity entity)
    {
        var tableName = GetTableName(entity);
        try
        {
            switch (entity)
            {
                case Space space:
                    return
                        $@"select count(*) from {tableName} where idexterno={space.Id} and dataatualizacao != '{space.UpdateTimestamp?.Date}'";
                case Event @event:
                    return
                        $@"select count(*) from {tableName} where idexterno={@event.Id} and dataatualizacao != '{@event.UpdateTimestamp?.Date}'";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return "";
    }

    private static string GetTableName(BaseEntity entity)
    {
        try
        {
            return entity switch
            {
                SpaceType spaceType => "tiposespacos",
                Space space => "espacos",
                Event @event => "eventos",
                Occurrence occurrence => "eventosdatas",
                _ => ""
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static long GetEntityExternalId(BaseEntity entity)
    {
        try
        {
            return entity switch
            {
                Space space => space.Id,
                Event @event => @event.Id,
                Occurrence occurrence => occurrence.Id,
                _ => 0
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string GetTableColumns(BaseEntity entity)
    {
        try
        {
            return entity switch
            {
                SpaceType spaceType => "idexterno,nome",
                Space space =>
                    "idexterno,nome,detalhe,detalhelongo,publico,aprovado,latitude,longitude,cep,logradouro,numero,complemento," +
                    "bairro,cidade,uf,datacriacao,dataatualizacao,area,tags,site,facebook,instagram,horario,endereco,acessibilidade,acessibilidade_fisica,urlavatar,idespacoprincipal",
                Event @event =>
                    "idexterno,nome,detalhe,detalhelongo,destaque,importado,aprovado,urlentrada," +
                    "ativo,datahora,telefone,linguagem,datacriacao,dataatualizacao,classificacaoetaria,urlavatar,excluido",
                Occurrence occurrence =>
                    "idexterno,idevento,datahora,idespaco,nome,detalhe,horafim,frequencia," +
                    " datainicio,datafim,diasemana,preco,urlavatar",
                _ => ""
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string GetTableValues(BaseEntity entity, Agent? agent = null, SqlConnection connection = null)
    {
        try
        {
            return entity switch
            {
                SpaceType spaceType => $@"'{spaceType.Id}', '{spaceType.Name}'",

                Space space =>
                    $@"'{space.Id}','{(space.Name ?? "").Replace("'", " ")}',
                    '{(space.ShortDescription ?? "").Replace("'", "''")}',
                    '{(space.LongDescription ?? "").Replace("'", "''")}', {Convert.ToInt32(space.Public)},
                    {Convert.ToInt32(agent != null ? agent.SpacesIds.Contains(space.Id) : 0)}, '{space.Location?.Latitude}', '{space.Location?.Longitude}','{space.EnCep ?? ""}'," +
                    $@"'{space.EnNomeLogradouro?.Replace("'", " ") ?? ""}',
                    '{space.EnNum ?? ""}','{(space.EnComplemento ?? "").Replace("'", " ")}',
                    '{(space.EnBairro ?? "")?.Replace("'", " ") ?? ""}','{space.EnMunicipio ?? ""}'," +
                    $@"'{space.EnEstado ?? ""}','{space.CreateTimestamp?.Date}', '{(space.UpdateTimestamp != null ? space.UpdateTimestamp.Date : "")}',
                    '{string.Join(",", space.Terms?.Area?.ToArray() ?? Array.Empty<string>())}'," +
                    $@"'{string.Join(",", space.Terms?.Tag?.ToArray() ?? Array.Empty<string>())}',
                    '{(space.Site ?? "").Replace("\\", "")}','{(space.Facebook ?? "").Replace("\\", "")}',
                    '{(space.Instagram ?? "").Replace("\\", "")}','{space.Horario?.Replace("'", "''").Replace("\\", "") ?? ""}',
                    '{(space.Endereco ?? "").Replace("'", "''").Replace("\\", "") ?? ""}','{space.Acessibilidade ?? ""}','{space.AcessibilidadeFisica ?? ""}',
                    '{space.FilesAvatar?.url}', '{GetSpaceIdByExternalId(connection, space.Parent ?? 0)}'",

                Event @event =>
                    $@"'{@event.Id}', '{@event.Name?.Replace("'", " ") ?? ""}',
                    '{(@event.ShortDescription ?? "").Replace("'", " ")}',
                    '{(@event.LongDescription ?? "").Replace("'", " ")}',
                    {Convert.ToInt32(agent != null ? agent.HighlightedEventsIds.Contains(@event.Id) : 0)},'1',{Convert.ToInt32(agent != null ? agent!.EventsIds.Contains(@event.Id) : 0)},'{@event.Site}'," +
                    $"{Convert.ToInt32(@event.Status)}, '{@event.CreateTimestamp?.Date}'," +
                    $"'{@event.TelefonePublico?.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")}'," +
                    $"'{string.Join(",", @event.Terms?.Linguagem?.ToArray() ?? Array.Empty<string>())}','{@event.CreateTimestamp?.Date}','{@event.UpdateTimestamp?.Date}','{@event.ClassificacaoEtaria}'," +
                    $"'{@event.FilesAvatar?.url}','0'",

                Occurrence occurrence =>
                    $@"'{occurrence.OccurrenceId}','{occurrence.EventId}','{occurrence.StartsOn} {occurrence.StartsAt}','{occurrence.Space?.Id}','','{occurrence.Rule?.Description.Replace("'", "''")}','{occurrence.EndsAt}','{occurrence.Rule?.Frequency}'," +
                    $@"'{occurrence.Rule?.StartsOn}','{occurrence.Rule?.Until}','','{occurrence.Rule?.Price}','{occurrence.FilesAvatar?.url}'",

                _ => ""
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
using System.Data;
using Dapper;
using RinhaBackend2023Q3.Entities;

namespace RinhaBackend2023Q3.Repositories;

public class PersonRepository
{
    private readonly IDbConnection _dbConnection;

    public PersonRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Person>> ListAsync(string query)
    {
        return await _dbConnection.QueryAsync<Person>(
            $@"SELECT id::varchar, nickname, name, birthdate, stack FROM people
                WHERE search ILIKE '%' || @Query || '%'
                LIMIT 50", new
            {
                Query = query?.Replace(' ', '%')
            });
    }

    public async Task CreateAsync(CreatePerson createPerson)
    {
        await _dbConnection.ExecuteAsync(
            "INSERT INTO people (id, nickname, name, birthdate, stack) VALUES (@Id::uuid, @Nickname, @Name, @BirthDate, @Stack)",
            createPerson);
    }

    public async Task<int> CountAsync()
    {
        return await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM people");
    }

    public async Task<Person> FindByIdAsync(string id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Person>(
            "SELECT id::varchar, nickname, name, birthdate, stack FROM people WHERE id = @Id::uuid", new { Id = id });
    }
}
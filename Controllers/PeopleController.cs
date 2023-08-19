using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using RinhaBackend2023Q3.Entities;
using RinhaBackend2023Q3.Repositories;

namespace RinhaBackend2023Q3.Controllers;

[ApiController]
[Produces("application/json")]
[Route("pessoas")]
public class PeopleController : ControllerBase
{
    private readonly PersonRepository _personRepository;

    public PeopleController(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<Person>> List([FromQuery(Name = "t")] string query)
    {
        return await _personRepository.ListAsync(query);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePerson createPerson,
        [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
    {
        try
        {
            await _personRepository.CreateAsync(createPerson);

            return CreatedAtAction(nameof(Get), new { id = createPerson.Id }, createPerson);
        }
        catch (PostgresException postgresException) when (postgresException.SqlState ==
                                                          PostgresErrorCodes.UniqueViolation)
        {
            ModelState.AddModelError(nameof(createPerson.Nickname), "Nickname already exists");
            return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> Get(string id)
    {
        var personDto = await _personRepository.FindByIdAsync(id);

        if (personDto is null)
        {
            return NotFound();
        }

        return personDto;
    }

    [HttpGet("contagem-pessoas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<int> Count()
    {
        return await _personRepository.CountAsync();
    }

    [HttpGet("config")]
    public IEnumerable<KeyValuePair<string, string?>> Config([FromServices] IConfiguration configuration)
    {
        return configuration.AsEnumerable();
    }
}
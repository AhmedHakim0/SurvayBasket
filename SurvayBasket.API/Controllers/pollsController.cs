namespace SurvayBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class pollsController(IPollService PollService) : ControllerBase
{
   private readonly IPollService _PollService = PollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_PollService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var poll=_PollService.Get(id);

        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Add(Poll poll)
    {
        _PollService.Add(poll);
        return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll);
    }

    [HttpPut("{id}")]
 
    public IActionResult Update(int id, Poll poll)
    {
        if(!_PollService.Update(id,poll))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_PollService.Delete(id))
            return NotFound();
        return NoContent();
    }
}

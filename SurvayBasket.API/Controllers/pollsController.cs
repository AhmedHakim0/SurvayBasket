namespace SurvayBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class pollsController(IPollService PollService) : ControllerBase
{
   private readonly IPollService _PollService = PollService;

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var Polls = _PollService.GetAll();
        return Ok(Polls.Adapt<IEnumerable<PollResponse>>());
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute]int id)
    {
        var poll=_PollService.Get(id);

        return poll is null ? NotFound() : Ok(poll.Adapt<PollResponse>());
    }

    [HttpPost("")]
    public IActionResult Add([FromBody]PollRequest Request)
    {
       var newPoll= _PollService.Add(Request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("{id}")]
 
    public IActionResult Update([FromRoute]int id,[FromBody] PollRequest request)
    {
        if (!_PollService.Update(id, request.Adapt<Poll>()))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (!_PollService.Delete(id))
            return NotFound();
        return NoContent();
    }
}

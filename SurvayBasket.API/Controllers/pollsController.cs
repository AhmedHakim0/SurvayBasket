using Microsoft.AspNetCore.Authorization;
using SurvayBasket.API.Contracts.Polls;

namespace SurvayBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class pollsController(IPollService PollService) : ControllerBase
{
   private readonly IPollService _PollService = PollService;

    [HttpGet("")]
   
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var Polls = await _PollService.GetAllAsync(cancellationToken);
        return Ok(Polls.Adapt<IEnumerable<PollResponse>>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await _PollService.GetAsync(id, cancellationToken);

        return poll is null ? NotFound() : Ok(poll.Adapt<PollResponse>());
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest Request, CancellationToken cancellationToken)
    {
        var newPoll = await _PollService.AddAsync(Request.Adapt<Poll>(), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll.Adapt<PollResponse>());
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,CancellationToken cancellationToken)
    {
        if (!await _PollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken))
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
    {
        if (!await _PollService.DeleteAsync(id, cancellationToken))
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}/ToggelPublish")]
    public async Task<IActionResult> ToggelPublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (!await _PollService.ToggelPublishStatusAsync(id, cancellationToken))
            return NotFound();

        return NoContent();
    }

}

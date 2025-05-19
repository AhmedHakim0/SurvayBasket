using Microsoft.AspNetCore.Authorization;

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
        return Ok(Polls);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _PollService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.Description);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest Request, CancellationToken cancellationToken)
    {
        var newPoll = await _PollService.AddAsync(Request, cancellationToken);
        var createdPoll = newPoll.Adapt<Poll>();
        return CreatedAtAction(nameof(Get), new { id = createdPoll.Id }, newPoll);

    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _PollService.UpdateAsync(id, request, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            :Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.Description);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _PollService.DeleteAsync(id, cancellationToken);
          return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.Description);
    }

    [HttpPut("{id}/ToggelPublish")]
    public async Task<IActionResult> ToggelPublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _PollService.ToggelPublishStatusAsync(id, cancellationToken);
         
        return result.IsSuccess
            ? NoContent()
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.code, detail: result.Error.Description);
    }

}

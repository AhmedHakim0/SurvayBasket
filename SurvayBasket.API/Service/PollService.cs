
using SurvayBasket.API.Abstractions;
using SurvayBasket.API.Errors;

namespace SurvayBasket.API.Service;
public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context=context;

    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var polls = await _context.Polls.ToListAsync(cancellationToken);
        return polls.Adapt<IEnumerable<PollResponse>>();
    }

    public async Task<Result<PollResponse>> GetAsync(int id,CancellationToken cancellationToken=default)
    {
        var poll= await _context.Polls.FindAsync(id,cancellationToken);
        
        return poll is not null
            ?Result.Success(poll.Adapt<PollResponse>())
            :Result.Failure<PollResponse>(PollErrors.PollNotFound);
    }
     


    public async Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken)
    {
       await _context.Polls.AddAsync(request.Adapt<Poll>());
       await _context.SaveChangesAsync();  
       return request.Adapt<PollResponse>();
    }

    public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
    {
        var current_poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (current_poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        current_poll.Title = poll.Title;
        current_poll.Summary = poll.Summary;
        current_poll.StartsAt = poll.StartsAt;
        current_poll.EndsAt = poll.EndsAt;

        _context.Polls.Update(current_poll);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }

    public async  Task<Result> DeleteAsync(int id, CancellationToken cancellationToken )
    {
        var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);
        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        _context.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result> ToggelPublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);
        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;
        _context.Polls.Update(poll);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

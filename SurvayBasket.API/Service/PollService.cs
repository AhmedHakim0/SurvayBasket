
namespace SurvayBasket.API.Service;
public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context=context;

    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)=> await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<Poll?> GetAsync(int id,CancellationToken cancellationToken)=> await _context.Polls.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);


    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken)
    {
       await _context.Polls.AddAsync(poll);
       await _context.SaveChangesAsync();  
       return poll;
    }

    public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken)
    {
        var current_poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);
        if (current_poll is null)
            return false;
        current_poll.Title = poll.Title;
        current_poll.Summary = poll.Summary;
        current_poll.IsPublished = poll.IsPublished;
        current_poll.StartsAt = poll.StartsAt;
        current_poll.EndsAt = poll.EndsAt;

        _context.Polls.Update(current_poll);
        await _context.SaveChangesAsync(cancellationToken);
        return true;

    }

    public async  Task<bool> DeleteAsync(int id, CancellationToken cancellationToken )
    {
        var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);
        if (poll is null)
            return false;
       _context.Remove(poll);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ToggelPublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.SingleOrDefaultAsync(p => p.Id == id);
        if (poll is null)
            return false;
        poll.IsPublished = !poll.IsPublished;
        _context.Polls.Update(poll);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

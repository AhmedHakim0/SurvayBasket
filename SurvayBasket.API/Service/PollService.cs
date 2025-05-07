namespace SurvayBasket.API.Service;
public class PollService : IPollService
{
    private static List<Poll> polls = [
       new Poll{
            Id=1,
            Title="Poll 1",
            Description="Description for Poll 1",
        }
       ];
    public IEnumerable<Poll> GetAll()
    {
        return polls;
    }
    public Poll? Get(int id)
    {
        return polls.SingleOrDefault(p => p.Id == id);
    }

    public Poll Add(Poll poll)
    {
        poll.Id = polls.Count + 1;
        polls.Add(poll);
        return poll;
    }

    public bool Update(int id, Poll poll)
    {
        var current_poll = polls.SingleOrDefault(p => p.Id == id);
        if (current_poll is null)
            return false;
        current_poll.Title = poll.Title;
        current_poll.Description = poll.Description;
        return true;

    }

    public bool Delete(int id)
    {
        var poll = polls.SingleOrDefault(p => p.Id == id);
        if (poll is null)
            return false;
        polls.Remove(poll);
         return true;
    } 
}

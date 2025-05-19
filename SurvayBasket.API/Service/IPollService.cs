using SurvayBasket.API.Abstractions;

namespace SurvayBasket.API.Service;

public interface IPollService
{
    Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<PollResponse> AddAsync(PollRequest poll, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> ToggelPublishStatusAsync(int id, CancellationToken cancellationToken = default);
}

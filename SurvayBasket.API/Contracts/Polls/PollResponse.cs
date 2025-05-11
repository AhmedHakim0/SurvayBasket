namespace SurvayBasket.API.Contracts.Polls;

public record PollResponse(

     string Title,
     string Summary,
     bool IsPublished,
     DateOnly StartsAt,
     DateOnly EndsAt

    );

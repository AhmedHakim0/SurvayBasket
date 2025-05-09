namespace SurvayBasket.API.Contracts.Response;

public record PollResponse(

     string Title,
     string Summary,
     bool IsPublished,
     DateOnly StartsAt,
     DateOnly EndsAt

    );

namespace SurvayBasket.API.Contracts.Request;

public record PollRequest(

     string Title,
     string Summary,
     DateOnly StartsAt,
     DateOnly EndsAt
);

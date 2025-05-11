﻿namespace SurvayBasket.API.Contracts.Request;

public record PollRequest(

     string Title,
     string Summary,
     bool IsPublished,
     DateOnly StartsAt,
     DateOnly EndsAt
);

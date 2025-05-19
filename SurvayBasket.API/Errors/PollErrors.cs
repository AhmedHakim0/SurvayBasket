using SurvayBasket.API.Abstractions;

namespace SurvayBasket.API.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = 
        new("Poll.NotFound", "No Poll was found With the given id");
}

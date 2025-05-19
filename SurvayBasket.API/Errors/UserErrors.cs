using SurvayBasket.API.Abstractions;
namespace SurvayBasket.API.Errors;

public static class UserErrors
{
    public static  readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid User name Or Password");
    public static  readonly Error InvalidToken = new("Token.Invalid", "Invalid Token Generate Another One!");
}

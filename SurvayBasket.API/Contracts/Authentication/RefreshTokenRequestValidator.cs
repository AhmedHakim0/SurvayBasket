namespace SurvayBasket.API.Contracts.Authentication;

public class RefreshTokenRequestValidator:AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.token)
            .NotEmpty()
            .WithMessage("Token is required.");
        RuleFor(x => x.refreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}


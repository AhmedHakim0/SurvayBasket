namespace SurvayBasket.API.Validation;

public class PollRequestValidator: AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Length(3, 100);



    }
}

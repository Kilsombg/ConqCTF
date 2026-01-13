namespace ConqCTF.Application.Challenges.Commands.CreateChallenge
{
    public class CreateChallengeCommandValidator : AbstractValidator<CreateChallengeCommand>
    {
        public CreateChallengeCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Points)
                .GreaterThan(0);

            RuleFor(x => x.Flag)
                .NotEmpty();
        }
    }
}

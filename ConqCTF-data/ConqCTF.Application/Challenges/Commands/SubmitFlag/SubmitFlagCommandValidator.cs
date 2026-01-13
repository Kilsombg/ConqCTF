namespace ConqCTF.Application.Challenges.Commands.SubmitFlag
{
    public class SubmitFlagCommandValidator : AbstractValidator<SubmitFlagCommand>
    {
        public SubmitFlagCommandValidator()
        {
            RuleFor(x => x.Flag)
                .NotEmpty();
        }
    }
}

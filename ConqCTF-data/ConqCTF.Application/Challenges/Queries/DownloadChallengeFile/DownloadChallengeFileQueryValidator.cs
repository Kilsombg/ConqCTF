namespace ConqCTF.Application.Challenges.Queries.DownloadChallengeFile
{
    public class DownloadChallengeFileQueryValidator : AbstractValidator<DownloadChallengeFileQuery>
    {
        public DownloadChallengeFileQueryValidator()
        {
            RuleFor(x => x.ChallengeId)
                .NotEmpty();

            RuleFor(x => x.FileName)
                .NotEmpty();
        }
    }
}

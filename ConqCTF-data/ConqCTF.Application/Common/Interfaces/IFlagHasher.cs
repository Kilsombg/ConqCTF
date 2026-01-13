namespace ConqCTF.Application.Common.Interfaces
{
    public interface IFlagHasher
    {
        string Hash(string flag);

        bool Verify(string flag, string hash);
    }
}

namespace PartialZ.Api.Services.Interfaces
{
    public interface ILoginService
    {
        Task<string> Login(string emailID, string password);
    }
}

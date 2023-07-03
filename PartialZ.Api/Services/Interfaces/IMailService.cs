namespace PartialZ.Api.Services.Interfaces
{
    public interface IMailService
    {
        void SendVerificationMail(string toMailID);
    }
}

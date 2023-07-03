namespace PartialZ.Api.Services.Interfaces
{
    public interface ICryptographyService
    {
        string Encrypt(string Text);
        string Decrypt(string Text);
    }
}

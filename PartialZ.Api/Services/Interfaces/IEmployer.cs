using PartialZ.Api.Dtos;

namespace PartialZ.Api.Services.Interfaces
{
    public interface IEmployer
    {
        Task<int> RegsregisterEmployer(string eanNumber, string feinNumber);
        Task<int> AffidavitRegistration(AffidavitDto affidavitDto);
    }
}

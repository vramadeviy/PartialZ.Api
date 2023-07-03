using PartialZ.Api.Dtos;

namespace PartialZ.Api.Services.Interfaces
{
    public interface IEmployee
    {
        Task<EmployeeDto> GetEmployee(string EmailID);
        Task<int> RegsregisterEmployee(string emailID, string password);
        Task<int> VerifyEmployee(string emailID);
        Task<int> EmplpyeeDetailRegistration(AffidavitDto affidavitDto);
    }
}

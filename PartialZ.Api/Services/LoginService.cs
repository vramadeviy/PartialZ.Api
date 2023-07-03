using Microsoft.EntityFrameworkCore;
using PartialZ.Api.Services.Interfaces;
using PartialZ.DataAccess.PartialZDB;

namespace PartialZ.Api.Services
{
    public class LoginService: ILoginService
    {
        private PartialZContext _PartialZContext;
        private ICryptographyService _cryptographyService;
        public LoginService(PartialZContext PartialZContext,
            ICryptographyService cryptographyService)
        {
            this._PartialZContext = PartialZContext;
            this._cryptographyService = cryptographyService;
        }
        public async Task<string> Login(string emailID, string password)
        {
            try
            {
                password = this._cryptographyService.Encrypt(password);
                if (this._PartialZContext.Employees.Where(e => e.Email == emailID && e.Password== password && e.IsVerified==1).Any())
                {
                    return "logged in successfully";
                }
                else if(this._PartialZContext.Employees.Where(e => e.Email == emailID && e.Password == password).Any())
                {
                    var existingdata = await this._PartialZContext.Employees.Where(e => e.Email == emailID && e.Password == password).FirstAsync();
                    if (existingdata!=null && existingdata.IsVerified ==0) {
                        return "Your account is inactive,please check your email to activate your account";
                    }
                    else
                    {
                        return "Invalid credentials";
                    }
                }
                else
                {
                    return "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

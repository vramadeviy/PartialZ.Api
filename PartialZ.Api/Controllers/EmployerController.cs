using Microsoft.AspNetCore.Mvc;
using PartialZ.Api.Dtos;
using PartialZ.Api.Services.Interfaces;

namespace PartialZ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : Controller
    {
        private IEmployer _employer;
        public EmployerController(IEmployer employer)
        {
            this._employer = employer;
        }
     
        [HttpPost]
        public async Task<AffidavitDto> RegsregisterEmployer(AuthorizationDto authorizationDto)
        {
            var result = await this._employer.RegsregisterEmployer(authorizationDto.Eannumber, authorizationDto.Feinnumber);
            return result;
        }
        [HttpPost]
        [Route("AffidavitRegistration")]
        public async Task<IActionResult> AffidavitRegistration(AffidavitDto affidavitDto)
        {
            var result = await this._employer.AffidavitRegistration(affidavitDto);
            return Ok(result);
        }
    }
}

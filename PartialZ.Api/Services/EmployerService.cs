using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PartialZ.Api.Dtos;
using PartialZ.Api.Services.Interfaces;
using PartialZ.DataAccess.PartialZDB;

namespace PartialZ.Api.Services
{
    public class EmployerService : IEmployer
    {
        private PartialZContext _PartialZContext;
        private IEmployee _employee;
        public EmployerService(PartialZContext PartialZContext, IEmployee employee)
        {
            this._PartialZContext = PartialZContext;
            this._employee = employee;
        }
        public async Task<AffidavitDto> RegsregisterEmployer(string eanNumber, string feinNumber)
        {
            try
            {
                if (this._PartialZContext.Employers.Where(e => e.Eannumber == eanNumber && e.Feinnumber == feinNumber).Any())
                {
                    //update
                    var existingdata = await this._PartialZContext.Employers.Where(e => e.Eannumber == eanNumber && e.Feinnumber == feinNumber).FirstAsync();
                    existingdata.Eannumber = eanNumber;
                    existingdata.Feinnumber = feinNumber;
                    existingdata.LastModifedDate = DateTime.UtcNow;
                }
                else
                {
                    var status = this._PartialZContext.Database.SqlQuery<string>($"EXEC dbo.sp_validateeanandfein {eanNumber}, {feinNumber}");
                   
                    if (status.ToList().First() == "VALID")
                    {
                        //insert
                        var data = new Employer()
                        {
                            Eannumber = eanNumber,
                            Feinnumber = feinNumber
                        };
                        await this._PartialZContext.Employers.AddAsync(data);
                        
                    }
                   
                }
               
                SqlParameter[] parameters = new[]
          {
                new SqlParameter("@EAN", string.IsNullOrEmpty(eanNumber) ? null : eanNumber),
                 new SqlParameter("@FEIN", string.IsNullOrEmpty(feinNumber) ? null : feinNumber)
            };

                var empData = this.CallStoredProcedure<AffidavitDto>("dbo.sp_Employer_Validate", parameters);

                AffidavitDto result = empData.FirstOrDefault();
                result.Eannumber = eanNumber;
                result.Feinnumber = feinNumber;
                
                return result;
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<TResult> CallStoredProcedure<TResult>(string storedProcedureName, params SqlParameter[] parameters)
        {
            var connectionString = _PartialZContext.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }



                    using (var reader = command.ExecuteReader())
                    {
                        var results = new List<TResult>();



                        while (reader.Read())
                        {
                            TResult result = MapDataReaderToTResult<TResult>(reader);
                            results.Add(result);
                        }
                        return results;
                    }
                }
            }
        }
        private TResult MapDataReaderToTResult<TResult>(SqlDataReader reader)
        {
            TResult result = Activator.CreateInstance<TResult>();

            if (reader.HasRows)
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var property = typeof(TResult).GetProperty(reader.GetName(i));
                    if (property != null && reader.IsDBNull(i) == false)
                    {
                        var value = reader.GetValue(i);
                        property.SetValue(result, value);
                    }
                }
            }


            return result;
        }
        public async Task<int> AffidavitRegistration(AffidavitDto affidavitDto)
        {
            try
            {
                int EmployerID = 0,
                    employeeID = 0;
                if (this._PartialZContext.Employers.Where(e => e.Eannumber == affidavitDto.Eannumber && e.Feinnumber == affidavitDto.Feinnumber).Any())
                {
                    //update
                    var existingdata = await this._PartialZContext.Employers.Where(e => e.Eannumber == affidavitDto.Eannumber && e.Feinnumber == affidavitDto.Feinnumber).FirstAsync();
                    existingdata.Eannumber = affidavitDto.Eannumber;
                    existingdata.Feinnumber = affidavitDto.Feinnumber;
                    existingdata.EmployerEmail = affidavitDto.EmployerEmail;
                    existingdata.Name = affidavitDto.EmployerName;
                    existingdata.Address = affidavitDto.Address;
                    existingdata.City = affidavitDto.City;
                    existingdata.State = affidavitDto.State;
                    existingdata.ZipCode = affidavitDto.ZIP;
                    existingdata.LastModifedDate = DateTime.UtcNow;
                    await this._PartialZContext.SaveChangesAsync();
                    EmployerID = existingdata.EmployerId;
                }
                else
                {
                    //insert
                    var data = new Employer()
                    {
                        Eannumber = affidavitDto.Eannumber,
                        Feinnumber = affidavitDto.Feinnumber,
                        EmployerEmail = affidavitDto.EmployerEmail,
                        Name = affidavitDto.EmployerName,
                        Address = affidavitDto.Address,
                        City = affidavitDto.City,
                        State = affidavitDto.State,
                        ZipCode = affidavitDto.ZIP
                    };
                    await this._PartialZContext.Employers.AddAsync(data);
                    await this._PartialZContext.SaveChangesAsync();
                    EmployerID = data.EmployerId;
                }                
                //save employee details
                employeeID = await this._employee.EmplpyeeDetailRegistration(affidavitDto);
                if (employeeID > 0 && employeeID > 0)
                {
                    //data saved successfully, map the data into EmployeeWorkHistory 
                    if (this._PartialZContext.EmployeeWorkHistories.Where(e => e.EmployeeId == employeeID && e.EmployerId == EmployerID).Any())
                    {
                        var existingdata = await this._PartialZContext.EmployeeWorkHistories.Where(e => e.EmployeeId == employeeID && e.EmployerId == EmployerID).FirstAsync();
                        existingdata.EmployeeId = employeeID;
                        existingdata.EmployerId = EmployerID;
                        existingdata.PayrollEndDay = affidavitDto.PayrollEndDay;
                        existingdata.LastModifedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        var data = new EmployeeWorkHistory()
                        {
                            EmployeeId = employeeID,
                            EmployerId = EmployerID,
                            PayrollEndDay = affidavitDto.PayrollEndDay
                        };
                        this._PartialZContext.EmployeeWorkHistories.Add(data);
                    }
                   return await this._PartialZContext.SaveChangesAsync();
                }
                else
                {
                    // somethig went wrong
                    return 0;
                }

                //send mail sync
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

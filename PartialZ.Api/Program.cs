using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PartialZ.Api.Services;
using PartialZ.Api.Services.Interfaces;
using PartialZ.DataAccess.PartialZDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PartialZContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("PartialZDB"));
}); 
builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<IEmployer, EmployerService>();
builder.Services.AddScoped<ICryptographyService, CryptographyService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddCors(options => {

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("X-Pagination-Page", "X-Pagination-Count", "X-Total-Count", "Content-Disposition");
    });
});
//builder.Services.AddCors(options => {

//    options.AddPolicy("CorsPolicy", builder =>
//    {
//        builder.WithOrigins("http://localhost:4200/")
//        .SetIsOriginAllowedToAllowWildcardSubdomains()
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials()
//        .WithExposedHeaders("X-Pagination-Page", "X-Pagination-Count", "X-Total-Count", "Content-Disposition");
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.UseCors("CorsPolicy");
app.UseCors();

app.Run();

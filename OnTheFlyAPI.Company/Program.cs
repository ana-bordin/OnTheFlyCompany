using Microsoft.Extensions.Options;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            //
            builder.Services.AddControllers();

            builder.Services.Configure<CompanyAPIDataBaseSettings>(
                           builder.Configuration.GetSection(nameof(CompanyAPIDataBaseSettings)));

            builder.Services.AddSingleton<ICompanyAPIDataBaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CompanyAPIDataBaseSettings>>().Value);

            builder.Services.AddSingleton<CustomerService>();
            builder.Services.AddSingleton<AddressService>();


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.Extensions.Options;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Address.Utils;

namespace OnTheFlyAPI.Address
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.Configure<CompanyAPIDataBaseSettings>(
builder.Configuration.GetSection(nameof(CompanyAPIDataBaseSettings)));

            builder.Services.AddSingleton<ICompanyAPIDataBaseSettings>(sp =>
            sp.GetRequiredService<IOptions<CompanyAPIDataBaseSettings>>().Value);

            builder.Services.AddSingleton<AddressService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

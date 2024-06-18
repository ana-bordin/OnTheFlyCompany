using Microsoft.Extensions.Options;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.Configure<CompanyAPIDataBaseSettings>(
    builder.Configuration.GetSection(nameof(CompanyAPIDataBaseSettings)));

            builder.Services.AddSingleton<ICompanyAPIDataBaseSettings>(sp =>
            sp.GetRequiredService<IOptions<CompanyAPIDataBaseSettings>>().Value);

            builder.Services.AddSingleton<Get>();
            builder.Services.AddSingleton<Post>();
            builder.Services.AddSingleton<CompanyService>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

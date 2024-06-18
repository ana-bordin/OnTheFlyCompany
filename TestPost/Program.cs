using Microsoft.Extensions.Options;
using TestPost.Services;
using TestPost.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<CompanyAPIDataBaseSettings>(
builder.Configuration.GetSection(nameof(CompanyAPIDataBaseSettings)));

builder.Services.AddSingleton<ICompanyAPIDataBaseSettings>(sp =>
sp.GetRequiredService<IOptions<CompanyAPIDataBaseSettings>>().Value);

builder.Services.AddSingleton<Post>();
builder.Services.AddSingleton<AddressService>();

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

app.Run();

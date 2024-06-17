using Newtonsoft.Json;
using System.Text;

namespace OnTheFlyAPI.Company.Services
{
    public class CompanyContext
    {
        private static readonly HttpClient _httpCliente = new HttpClient();

        public async Task<Models.Company> PostCarro(Models.Company company)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await CompanyService._httpCliente.PostAsync("https://localhost:5005/api/Carro", content);
                response.EnsureSuccessStatusCode();
                string carroReturn = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(companyReturn);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

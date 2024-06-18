using MongoDB.Driver;
using Newtonsoft.Json;
using OnTheFlyAPI.Address.Utils;

namespace OnTheFlyAPI.Address.Services
{
    public class AddressesService
    {
        private readonly IMongoCollection<Models.Address> _addressCollection;

        public AddressesService(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _addressCollection = database.GetCollection<Models.Address>(settings.AddressCollectionName);
        }

        public async Task<Models.Address> RetrieveAdressAPI(Models.AddressDTO addressDTO)
        {
            Models.Address address = new Models.Address();
            Models.AddressAPI addressAPI;
            string _url = "https://viacep.com.br/ws";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{_url}/{addressDTO.ZipCode}/json/";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        addressAPI = JsonConvert.DeserializeObject<Models.AddressAPI>(json);
                        address.ZipCode = addressDTO.ZipCode;
                        address.Number = addressDTO.Number;
                        address.Complement = addressAPI.Complement;
                        address.City = addressAPI.City;
                        address.State = addressAPI.State;
                        address.Street = addressAPI.Street;

                    }
                    else
                    {
                        address = null;
                        Console.WriteLine("Erro no consumo do WS CEP.");
                        Console.WriteLine(response.StatusCode);
                    }
                }
                return address;
            }
            catch
            {
                throw;
            }
        }

    }
}

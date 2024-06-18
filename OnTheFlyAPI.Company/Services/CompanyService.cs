using MongoDB.Driver;
using Newtonsoft.Json;
using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Models.Company> _companyCollection;
        private readonly IMongoCollection<Models.Company> _companyHistoryCollection;

        public CompanyService(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Models.Company>(settings.CompanyCollectionName);
            _companyHistoryCollection = database.GetCollection<Models.Company>(settings.CompanyHistoryCollectionName);
        }

        public async Task<List<Models.Company>> GetAll(int param)
        {
            if (param == 0)
            {
                return _companyCollection.Find(c => true).ToList();
            }
            else if (param == 1)
            {
                return _companyHistoryCollection.Find(c => true).ToList();
            }
            return null;
        }
        public async Task<Models.Company> GetByCnpj(int param, string cnpj)
        {
            if (param == 0)
            {
                return await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<Models.Company> GetByName(int param, string name)
        {
            if (param == 0)
            {
                return await _companyCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
            }
            return null;
        }
        
        
        public async Task<Address.Models.Address> RetrieveAdressAPI(AddressDTO dto)
        {
            Address.Models.Address address;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://localhost:7065/api/address/";
                    string jsonAddress = JsonConvert.SerializeObject(dto);

                    var content = new StringContent(jsonAddress, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var ad = JsonConvert.DeserializeObject<Address.Models.Address>(responseBody);
                        address = ad;
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


        public async Task<Models.Company> PostCompany(Models.Company company)
        {
            if (company != null)
                _companyCollection.InsertOne(company);

            return company;
        }
        public async Task<Models.Company> PostHistoryCompany(Models.Company company)
        {
            if (company != null)
                _companyHistoryCollection.InsertOne(company);

            return company;
        }


        public async Task<Models.Company> Update(Models.CompanyPatchDTO DTO, string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            var company = await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            if (company != null)
            {
                var filter = Builders<Models.Company>.Filter.Eq("Cnpj", cnpj);

                UpdateDefinition<Models.Company> update = Builders<Models.Company>.Update
                    .Set("NameOpt", DTO.NameOpt)
                    .Set("Address.Complement", DTO.Complement);

                await _companyCollection.UpdateOneAsync(filter, update);

                return await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }
        public async Task<Models.Company> UpdateStatus(Models.CompanyPatchStatusDTO DTO, string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            var company = await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            if (company != null)
            {
                var filter = Builders<Models.Company>.Filter.Eq("Cnpj", cnpj);
                UpdateDefinition<Models.Company> update = Builders<Models.Company>.Update
                    .Set("Restricted", DTO.Restricted);

                await _companyCollection.UpdateOneAsync(filter, update);

                return await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }


        public bool DeleteCompany(string cnpj)
        {
            var result = _companyCollection.DeleteOne(c => c.Cnpj == cnpj);
            if (result.DeletedCount > 0)
                return true;
            return false;
        }
        public bool RestorageCompany(string cnpj)
        {
            var result = _companyHistoryCollection.DeleteOne(c => c.Cnpj == cnpj);
            if (result.DeletedCount > 0)
                return true;
            return false;
        }
    }
}

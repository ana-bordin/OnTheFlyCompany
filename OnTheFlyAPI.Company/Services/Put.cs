using MongoDB.Driver;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company.Services
{
    public class Put
    {
        private readonly IMongoCollection<Models.Company> _companyCollection;
        public Put(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Models.Company>(settings.CompanyCollectionName);

        }
        public async Task<Models.Company> Update(Models.CompanyPatchDTO DTO, string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            var filter = Builders<Models.Company>.Filter.Eq("Cnpj", cnpj);
            UpdateDefinition<Models.Company> update = Builders<Models.Company>.Update
                .Set("NameOpt", DTO.NameOpt)
                .Set("Restricted", DTO.Restricted)
                .Set("Address.Complement", DTO.Complement);

            var x = await _companyCollection.UpdateOneAsync(filter, update);
            return await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
        }

    }
}

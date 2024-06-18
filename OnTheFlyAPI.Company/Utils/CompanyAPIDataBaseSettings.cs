namespace OnTheFlyAPI.Company.Utils
{
    public class CompanyAPIDataBaseSettings : ICompanyAPIDataBaseSettings
    {
        public string CompanyCollectionName { get; set; }
        public string CompanyHistoryCollectionName { get; set; }
        public string AddressCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

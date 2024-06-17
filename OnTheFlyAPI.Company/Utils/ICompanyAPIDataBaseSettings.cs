namespace OnTheFlyAPI.Company.Utils
{
    public class ICompanyAPIDataBaseSettings
    {
        string CompanyCollectionName { get; set; }
        string CompanyHistoryCollectionName { get; set; }
        string AddressCollectionName { get; set; }

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

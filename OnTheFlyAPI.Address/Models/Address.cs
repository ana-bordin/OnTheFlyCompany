using Newtonsoft.Json;

namespace OnTheFlyAPI.Address.Models
{
    public class Address
    {
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("complement")]
        public string Complement { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }

        public Address()
        {
            
        }

        public Address(AddressDTO dto)
        {
            this.ZipCode = dto.ZipCode;
            this.Number = dto.Number;
        }

        public static string RemoveMask(string zipcode)
        {
            zipcode = zipcode.Replace(".", "");
            zipcode = zipcode.Replace("-", "");
            return zipcode;
        }

        public static string InsertMask(string zipcode)
        {
            return Convert.ToUInt64(zipcode).ToString(@"00\.000\-000");
        }
    }
}

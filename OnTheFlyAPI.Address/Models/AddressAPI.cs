using Newtonsoft.Json;

namespace OnTheFlyAPI.Address.Models
{
    public class AddressAPI
    {
        [JsonProperty("cep")]
        public string ZipCode { get; set; }
        [JsonProperty("logradouro")]
        public string Street { get; set; }
        public int Number { get; set; }
        [JsonProperty("complemento")]
        public string Complement { get; set; }
        [JsonProperty("localidade")]
        public string City { get; set; }
        [JsonProperty("uf")]
        public string State { get; set; }
    }
}

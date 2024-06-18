namespace ModelsAux
{
    public class Address
    {
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Address()
        {
            
        }

        public Address(AddressDTO dto)
        {
            this.ZipCode = dto.ZipCode;
            this.Number = dto.Number;
        }
    }
}

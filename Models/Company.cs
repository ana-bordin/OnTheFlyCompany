using MongoDB.Bson.Serialization.Attributes;

namespace ModelsAux
{
    public class Company
    {
        [BsonId]
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        //public DateOnly DtOpen { get; set; }
        public DateTime DtOpen { get; set; }
        public bool Restricted { get; set; }
        public Address Address { get; set; }

        public Company()
        {

        }

        public Company(CompanyDTO dto)
        {
            this.Cnpj = dto.Cnpj;
            this.Name = dto.Name;
            this.NameOpt = dto.NameOpt;
            this.DtOpen = dto.DtOpen;
            this.Restricted = dto.Restricted;
        }

        public static bool VerificarCnpj(string cnpj)
        {
            cnpj = string.Join("", cnpj.Where(Char.IsDigit));

            if (cnpj.Length != 14) return false;

            bool repetido = IsRepetido(cnpj);
            bool digitoUm = ValidacaoDigitoUm(cnpj);
            bool digitoDois = ValidacaoDigitoDois(cnpj);

            return !repetido && digitoUm && digitoDois;
        }

        private static bool ValidacaoDigitoUm(string str)
        {
            int total = 0;
            int[] nrosMultiplicadores = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 12; i++)
            {
                string digitoStr = str.Substring(i, 1);
                int digito = int.Parse(digitoStr);

                total += digito * nrosMultiplicadores[i];
            }

            int resto = total % 11;
            int digitoUm = int.Parse(str.Substring(12, 1));

            if ((resto == 0 || resto == 1) && digitoUm == 0)
                return true;

            if ((resto >= 2 && resto <= 10) && digitoUm == 11 - resto)
                return true;

            return false;
        }

        private static bool ValidacaoDigitoDois(string cnpj)
        {
            int total = 0;
            int[] nrosMultiplicadores = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < 13; i++)
            {
                string digitoStr = cnpj.Substring(i, 1);
                int digito = int.Parse(digitoStr);

                total += digito * nrosMultiplicadores[i];
            }

            int resto = total % 11;
            int digitoDois = int.Parse(cnpj.Substring(13, 1));

            if ((resto == 0 || resto == 1) && digitoDois == 0)
                return true;

            if ((resto >= 2 && resto <= 10) && digitoDois == 11 - resto)
                return true;

            return false;
        }

        private static bool IsRepetido(string cnpj)
        {
            cnpj = RemoverCaractere(cnpj);
            int nroRepetidos = 0;
            for (int i = 0; i < cnpj.Length - 1; i++)
            {
                int n1 = int.Parse(cnpj.Substring(i, 1));
                int n2 = int.Parse(cnpj.Substring(i + 1, 1));

                if (n1 == n2)
                {
                    nroRepetidos++;
                }
            }
            return nroRepetidos == cnpj.Length - 1;
        }

        public static string RemoverCaractere(string cnpj)
        {
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace("-", "");
            return cnpj;
        }

    }
}

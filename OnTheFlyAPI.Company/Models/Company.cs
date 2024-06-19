﻿using MongoDB.Bson.Serialization.Attributes;

namespace OnTheFlyAPI.Company.Models
{
    public class Company
    {
        [BsonId]
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateTime DtOpen { get; set; }
        public bool Restricted { get; set; }
        public OnTheFlyAPI.Address.Models.Address Address { get; set; }

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

        public static bool VerifyCNPJ(string cnpj)
        {
            cnpj = string.Join("", cnpj.Where(Char.IsDigit));

            if (cnpj.Length != 14) return false;

            bool repeated = IsRepeated(cnpj);
            bool firstDigit = ValidateFirstDigit(cnpj);
            bool secondDigit = ValidateSecondDigit(cnpj);

            return !repeated && firstDigit && secondDigit;
        }

        private static bool ValidateFirstDigit(string str)
        {
            int total = 0;
            int[] multiplierNumbers = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 12; i++)
            {
                string strDigit = str.Substring(i, 1);
                int digit = int.Parse(strDigit);

                total += digit * multiplierNumbers[i];
            }

            int rest = total % 11;
            int firstDigit = int.Parse(str.Substring(12, 1));

            if ((rest == 0 || rest == 1) && firstDigit == 0)
                return true;

            if ((rest >= 2 && rest <= 10) && firstDigit == 11 - rest)
                return true;

            return false;
        }

        private static bool ValidateSecondDigit(string cnpj)
        {
            int total = 0;
            int[] multiplierNumbers = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < 13; i++)
            {
                string strDigit = cnpj.Substring(i, 1);
                int digit = int.Parse(strDigit);

                total += digit * multiplierNumbers[i];
            }

            int rest = total % 11;
            int secondDigit = int.Parse(cnpj.Substring(13, 1));

            if ((rest == 0 || rest == 1) && secondDigit == 0)
                return true;

            if ((rest >= 2 && rest <= 10) && secondDigit == 11 - rest)
                return true;

            return false;
        }

        private static bool IsRepeated(string cnpj)
        {
            cnpj = RemoveMask(cnpj);
            int repeatedNumbers = 0;
            for (int i = 0; i < cnpj.Length - 1; i++)
            {
                int n1 = int.Parse(cnpj.Substring(i, 1));
                int n2 = int.Parse(cnpj.Substring(i + 1, 1));

                if (n1 == n2)
                {
                    repeatedNumbers++;
                }
            }
            return repeatedNumbers == cnpj.Length - 1;
        }

        public static string RemoveMask(string cnpj)
        {
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace("-", "");
            return cnpj;
        }

        public static string InsertMask(string cnpj)
        {
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}

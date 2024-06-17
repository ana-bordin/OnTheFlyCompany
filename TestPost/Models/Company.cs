using System.Runtime.ConstrainedExecution;
using TestPost.Models;

namespace TestPost
{
    public class Company
    {
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


        /// <summary>
        /// Verifica se o CNPJ é válido.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser verificado.</param>
        /// <returns>True se o CNPJ for válido, False caso contrário.</returns>
        public static bool VerificarCnpj(string cnpj)
        {
            cnpj = string.Join("", cnpj.Where(Char.IsDigit));

            if (cnpj.Length != 14) return false;

            bool repetido = IsRepetido(cnpj);
            bool digitoUm = ValidacaoDigitoUm(cnpj);
            bool digitoDois = ValidacaoDigitoDois(cnpj);

            return !repetido && digitoUm && digitoDois;
        }

        /// <summary>
        /// Valida o primeiro dígito verificador do CNPJ.
        /// </summary>
        /// <param name="str">O CNPJ a ser validado.</param>
        /// <returns>True se o dígito for válido, False caso contrário.</returns>
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

        /// <summary>
        /// Valida o segundo dígito verificador do CNPJ.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser validado.</param>
        /// <returns>True se o dígito for válido, False caso contrário.</returns>
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

        /// <summary>
        /// Verifica se há dígitos repetidos no CNPJ.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser verificado.</param>
        /// <returns>True se houver dígitos repetidos, False caso contrário.</returns>
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

        /// <summary>
        /// Remove caracteres especiais do CNPJ.
        /// </summary>
        /// <param name="cnpj">O CNPJ a ser formatado.</param>
        /// <returns>O CNPJ formatado.</returns>
        public static string RemoverCaractere(string cnpj)
        {
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace("-", "");
            return cnpj;
        }

    }
}

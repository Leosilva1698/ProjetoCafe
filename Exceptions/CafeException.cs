using System.IO;
using System.Text;

namespace ProjetoCafe.Exceptions
{
    public class CafeException : ApplicationException
    {
        private readonly string nomeArquivo = @"./Logs/logDeErro.txt";
        public string avisoErro { get; }

        public CafeException(string erro)
        {
            this.avisoErro = erro;
            DateOnly data = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly hora = TimeOnly.FromDateTime(DateTime.Now);
            string logErro = $"{data} - {hora}: {erro}\n";

            File.AppendAllText(nomeArquivo, logErro, Encoding.Default);
        }

        
    }
}
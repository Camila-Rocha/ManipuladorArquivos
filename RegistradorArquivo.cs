using System;
using System.Data;

namespace LeituraDeArquivosBackgraund
{
    public class RegistradorArquivo : ManipuladorArquivo
    {
        public RegistradorArquivo() : base("Relatorio") { }

        AnaliseArquivo analise = new AnaliseArquivo();
        public string ArquivoDiaAtual { get; set; } = "";

        public override void CriarPasta(string caminho)
        {
            caminho = Path.Combine(caminho, NomePasta);
            base.CriarPasta(caminho);
        }

        public string RetornaNomeRelatorio()
        {
            return NomePasta;
        }

        public string ArquivoDataAtual(string caminho)
        {
            DateTime dataAtual = DateTime.Today;
            return ArquivoDiaAtual = Path.Combine(caminho, NomePasta, dataAtual.ToString("dd.MM.yyyy") + ".txt");
        }

        public void CriarArquivoRelatorioDia(string caminho, string[] array )
        {                       
            if (!File.Exists(ArquivoDataAtual(caminho)))
            {
                using (FileStream criarArquivo = File.Create(ArquivoDataAtual(caminho))){};
                DadosIniciaisPadraoArquivoRelatorio(caminho, array);
            }
        }
        
        public void DadosIniciaisPadraoArquivoRelatorio(string caminho, string[] array)
        {
            using (StreamWriter writer = new(ArquivoDataAtual(caminho), true))
            {
                writer.WriteLine($"{RetornaHoraAtual()} - Pasta contém {array.Length} arquivos\n");
                writer.WriteLine("Registros relevantes:");
            }
        }

        public void RegistraArquivosVaziosEComCriterio(string texto, string caminho) 
        {
            string ultimaLinha = string.Empty;
            string ultimoItemDaLinha = "";
            DateTime horaAtual = DateTime.Now;

            if (analise.VerificaCriterio(texto))
            {
                using (StreamReader leitor = new(texto))
                {
                    while (!leitor.EndOfStream)
                    {
                        ultimaLinha = leitor.ReadLine()!;
                    }

                    if (ultimaLinha != string.Empty)
                    {
                        string[] ultimaLinhaSeparada = ultimaLinha.Split(";");
                        ultimoItemDaLinha = ultimaLinhaSeparada[^1];
                    }
                }

                if (File.Exists(ArquivoDataAtual(caminho)))
                    {
                    using (StreamWriter writer = new(ArquivoDataAtual(caminho), true))
                    {
                        writer.WriteLine($"{horaAtual:HH:mm:ss} - Último ítem do arquivo \"{Path.GetFileNameWithoutExtension(texto)}\": {ultimoItemDaLinha}");
                    }
                }                
            }

            if (VerificaSeArquivoEstaVazio(texto))
            {
                using (StreamWriter writer = new(ArquivoDataAtual(caminho), true))
                {
                    writer.WriteLine($"{horaAtual:HH:mm:ss} - Arquivo \"{Path.GetFileNameWithoutExtension(texto)}\" está vazio! ");
                }
            }             
        }

        public void VerificaArquivosExcluidos(string[] array, string caminho)
        {
            string[] arrayAtualizado = analise.RetornaArrayDeArquivo(caminho);

            foreach (string arquivo in array)
            {
                if (!arrayAtualizado.Contains(arquivo))
                {                   
                    using (StreamWriter writer = new(ArquivoDataAtual(caminho), true))
                    {
                        writer.WriteLine($"{RetornaHoraAtual()} - Arquivo \"{Path.GetFileNameWithoutExtension(arquivo)}\" foi excluido da pasta Analise.");
                    }
                }
            }
        }

        public void VerificaArquivosAdicionados(string[] array, string caminho)
        {
            string[] arrayAtualizado = analise.RetornaArrayDeArquivo(caminho);          

            foreach (string arquivo in arrayAtualizado)
            {
                if (!array.Contains((arquivo)))
                {                   
                    using (StreamWriter writer = new(ArquivoDataAtual(caminho), true))
                    {
                        writer.WriteLine($"{RetornaHoraAtual()} - Arquivo \"{Path.GetFileNameWithoutExtension(arquivo)}\" foi adicionado a pasta Analise.");
                    }
                   
                    RegistraArquivosVaziosEComCriterio(arquivo, caminho);
                }
            }
        }              
    }
}

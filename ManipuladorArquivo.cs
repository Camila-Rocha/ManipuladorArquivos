namespace LeituraDeArquivosBackgraund
{
    public abstract class ManipuladorArquivo
    {
        public string NomePasta { get; set; }

        protected ManipuladorArquivo(string nomePasta) 
        { 
            NomePasta = nomePasta;
        }

        public string CaminhoPadrao()
        {
            string DiretorioPadrao;           
            DiretorioPadrao = AppDomain.CurrentDomain.BaseDirectory;          
            return DiretorioPadrao;
        }    
        
        public virtual void CriarPasta(string caminho)
         {
            if (Directory.Exists(caminho))
            {
                Console.WriteLine($"Pasta já existe! Vamos manter pasta {NomePasta} já existente em local escolhido.");
            }    
            
            else
            {               
                Directory.CreateDirectory(caminho);
            }
        }

        public bool VerificaSeTemArquivosNaPasta(string caminho)
        {      
            if (RetornaArrayDeArquivo(caminho).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] RetornaArrayDeArquivo(string caminho)
        {                      
            caminho = Path.Combine(caminho, NomePasta);
            string[] array = Directory.GetFiles(caminho, "*.txt");
            return array;      
        }

        public bool VerificaSeArquivoEstaVazio(string arquivo)
        {
            FileInfo fileInfo = new FileInfo(arquivo);
            if (fileInfo.Length > 0)
            {
                return false;
            }
            else { return true; }
        }

        public string RetornaHoraAtual()
        {
            DateTime hora = DateTime.Now;
            return hora.ToString("HH:mm:ss");
        }
    }
}

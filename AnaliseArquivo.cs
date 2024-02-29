namespace LeituraDeArquivosBackgraund
{
    public class AnaliseArquivo : ManipuladorArquivo
    {
        public AnaliseArquivo() : base("Analise") { }

        public string RetornaNomeAnalise()
        {
            return NomePasta;
        }

        public override void CriarPasta(string caminho)
        {
            caminho = Path.Combine(caminho, NomePasta);
            base.CriarPasta(caminho);
        }

        public bool VerificaCriterio(string arquivo)
        {
            using (StreamReader leitor = File.OpenText(arquivo))
            {
                string primeiraLinha = leitor.ReadLine();
                string palavraCritério = "EsseAqui";

                if (!VerificaSeArquivoEstaVazio(arquivo))
                {
                    if (primeiraLinha != null)
                    {
                        if (primeiraLinha.Contains(palavraCritério))
                        {
                            return true;
                        }                      
                    }
                }               
                
                return false;                            
            }
        }
    }
}

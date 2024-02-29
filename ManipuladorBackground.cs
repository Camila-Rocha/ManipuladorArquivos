using Microsoft.Extensions.Hosting;
using System.Media;

namespace LeituraDeArquivosBackgraund
{
    public class ManipuladorBackground : BackgroundService 
    {      
       protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {          
            AnaliseArquivo analise = new();
            RegistradorArquivo relatorio = new();          
            string caminhoPasta;
            string resposta;

            Console.WriteLine("\nDeseja especificar uma pasta para a guardar a análise e " +
                "registro de arquivos? \nCaso não seja informado, a pasta será criada no local " +
                "do programa.");
            Console.Write("\nResponda: \"S\" para informar o caminho da pasta ou \"N\" para criar em local padrão: ");           
 
            resposta = Console.ReadLine()!;

            for (caminhoPasta = ""; !Directory.Exists(caminhoPasta);)
            {
                if (resposta == "N" || resposta == "n")
                {
                    caminhoPasta = analise.CaminhoPadrao();
                    analise.CriarPasta(caminhoPasta);
                    relatorio.CriarPasta(caminhoPasta);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nConcluído com sucesso!\nCaminho da pasta: {caminhoPasta}.\n");
                    Console.ResetColor();
                } 

                else if (resposta == "S" || resposta == "s")
                {
                    if (Directory.Exists(Path.Combine(analise.CaminhoPadrao(), analise.RetornaNomeAnalise())) ||
                      Directory.Exists(Path.Combine(relatorio.CaminhoPadrao(), relatorio.RetornaNomeRelatorio())))
                    {
                        Console.WriteLine("\nPasta existente em local padrão. Deseja manter ou informar novo local?");                        
                        Console.Write("\nPara informar um novo local \"S\" para manter caminhoPasta existente digite \"N\": ");
                        resposta = Console.ReadLine()!;

                        if (resposta == "S" || resposta == "s")
                        {
                            do
                            {
                                Console.Write("\nInforme o caminho da pasta: ");
                                caminhoPasta = Console.ReadLine()!;

                                if (!Directory.Exists(caminhoPasta))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\ncaminho inválido!\n");
                                    Console.ResetColor();
                                    Console.Write("Responda: \"S\" para informar o caminho da pasta ou \"N\" para criar em local padrão: ");
                                    resposta = Console.ReadLine();
                                }

                                if (Directory.Exists(caminhoPasta))
                                { 
                                    analise.CriarPasta(caminhoPasta);
                                    relatorio.CriarPasta(caminhoPasta);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nConfigurações realizadas com sucesso! \nCaminho da pasta: {caminhoPasta}\n");
                                    Console.ResetColor();
                                }

                                else
                                {
                                    break;
                                }
                            }

                            while ((!Directory.Exists(caminhoPasta)) && (resposta != "n") && (resposta != "N"));
                        }
                    }
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nResposta inválida");
                    Console.ResetColor();
                    Console.Write("\nDigite \"S\" para informar caminho da pasta ou \"N\" para criar pasta em local Padrão: ");                  
                    resposta = Console.ReadLine()!;
                }
            }                  

            string[] arrayAnalise = analise.RetornaArrayDeArquivo(caminhoPasta);
            relatorio.CriarArquivoRelatorioDia(caminhoPasta, arrayAnalise);       

            if (analise.VerificaSeTemArquivosNaPasta(caminhoPasta))
            {
                foreach (string arquivo in arrayAnalise)
                {
                    relatorio.RegistraArquivosVaziosEComCriterio(arquivo, caminhoPasta);
                }
            }         
            
            while (!stoppingToken.IsCancellationRequested)
            {
                if(!Directory.Exists(Path.Combine(caminhoPasta, analise.RetornaNomeAnalise())))
                {
                    SystemSounds.Asterisk.Play();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pasta Análise foi apagada ou alterada indevidamente para seguir com a execução a recriamos no mesmo local.");
                    Console.ResetColor();
                    analise.CriarPasta(caminhoPasta);
                }

                if(!Directory.Exists(Path.Combine(caminhoPasta, relatorio.RetornaNomeRelatorio())))
                {
                    SystemSounds.Asterisk.Play();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pasta Relatório foi apagada ou alterada indevidamente, para seguir com a execução a recriamos no mesmo local.");
                    Console.ResetColor();
                    relatorio.CriarPasta(caminhoPasta);
                }

                relatorio.CriarArquivoRelatorioDia(caminhoPasta, arrayAnalise);
                relatorio.VerificaArquivosExcluidos(arrayAnalise, caminhoPasta);
                relatorio.VerificaArquivosAdicionados(arrayAnalise, caminhoPasta);
                arrayAnalise = analise.RetornaArrayDeArquivo(caminhoPasta);
                await Task.Delay(1000, stoppingToken);
            }          
        }
    }
}

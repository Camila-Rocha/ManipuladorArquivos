using LeituraDeArquivosBackgraund;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost variavel = Host.CreateDefaultBuilder()
    .ConfigureServices(service => 
    { 
        service.AddHostedService<ManipuladorBackground>();
    }).Build();

await variavel.RunAsync(); 
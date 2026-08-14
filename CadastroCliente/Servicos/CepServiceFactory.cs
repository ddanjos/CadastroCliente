using CadastroCliente.Enum;
using CadastroCliente.Infraestrutura.Services;
using CadastroCliente.Interfaces;

namespace CadastroCliente.Servicos
{
    public static class CepServiceFactory
    {
        public static ICepService CriarServico(TipoServicoCep tipo, HttpClient httpClient)
        {
            return tipo switch
            {
                TipoServicoCep.ViaCep => new ViaCepService(httpClient),
                TipoServicoCep.BrasilApi => new BrasilApiService(httpClient),
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), "Serviço de CEP desconhecido.")
            };
        }
    }
}
using CadastroCliente.DTOs;
using CadastroCliente.Interfaces;
using System.Net.Http.Json;

namespace CadastroCliente.Servicos
{
    public class BrasilApiService : ICepService
    {
        private readonly HttpClient _httpClient;

        public BrasilApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EnderecoDto?> ConsultarCepAsync(string cep)
        {
            string url = $"https://brasilapi.com.br/api/cep/v1/{cep}";

            try
            {
                var resposta = await _httpClient.GetFromJsonAsync<BrasilApiResponse>(url);

                if (resposta == null) return null;

                return new EnderecoDto(
                    resposta.Cep,
                    resposta.Street,
                    resposta.Neighborhood,
                    resposta.City,
                    resposta.State
                );
            }
            catch
            {
                return null;
            }
        }

        private class BrasilApiResponse
        {
            public string Cep { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Neighborhood { get; set; } = string.Empty;
            public string Street { get; set; } = string.Empty;
        }
    }
}
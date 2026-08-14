using CadastroCliente.DTOs;
using CadastroCliente.Interfaces;
using System.Net.Http.Json;

namespace CadastroCliente.Infraestrutura.Services;
public class ViaCepService : ICepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EnderecoDto?> ConsultarCepAsync(string cep)
    {
        string url = $"https://viacep.com.br/ws/{cep}/json/";

        var resposta = await _httpClient.GetFromJsonAsync<ViaCepResponse>(url);

        if (resposta == null || resposta.Erro)
            return null;

        return new EnderecoDto(
            resposta.Estado,
            resposta.Logradouro,
            resposta.Bairro,
            resposta.Localidade,
            resposta.Uf
        );
    }

  
    private class ViaCepResponse
    {
        public string Cep { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public bool Erro { get; set; }
    }
}
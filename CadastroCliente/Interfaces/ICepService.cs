using CadastroCliente.DTOs;

namespace CadastroCliente.Interfaces
{
    public interface ICepService
    {
        Task<EnderecoDto?> ConsultarCepAsync(string cep);
    }
}

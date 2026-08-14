using CadastroCliente.Entidades;

namespace CadastroCliente.Interfaces
{
    public interface IPessoaRepository
    {
        void Inserir(Pessoa pessoa);
        Pessoa? ConsultarUsuario(string cpf);
    }
}

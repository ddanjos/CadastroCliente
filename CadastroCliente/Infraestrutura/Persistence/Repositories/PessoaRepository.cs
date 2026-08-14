using CadastroCliente.Entidades;
using MySqlConnector;

namespace CadastroCliente.Infraestrutura.Persistence.Repositories
{
    public class PessoaRepository
    {
        public void Inserir(Pessoa pessoa)
        {
            using var connection = DatabaseConnection.Instance.GetConnection();

            string query = @"
                INSERT INTO Pessoa (Nome, Cpf, Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado) 
                VALUES (@Nome, @Cpf, @Cep, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @Estado);";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Nome", pessoa.Nome);
            command.Parameters.AddWithValue("@Cpf", pessoa.Cpf);
            command.Parameters.AddWithValue("@Cep", pessoa.Cep);
            command.Parameters.AddWithValue("@Logradouro", pessoa.Logradouro);
            command.Parameters.AddWithValue("@Numero", pessoa.Numero);
            command.Parameters.AddWithValue("@Complemento", string.IsNullOrEmpty(pessoa.Complemento) ? DBNull.Value : pessoa.Complemento);
            command.Parameters.AddWithValue("@Bairro", pessoa.Bairro);
            command.Parameters.AddWithValue("@Cidade", pessoa.Cidade);
            command.Parameters.AddWithValue("@Estado", pessoa.Estado);

            command.ExecuteNonQuery();
            Console.WriteLine("Pessoa cadastrada com sucesso utilizando a conexão Singleton!");
        }

        public Pessoa? ConsultarUsuario(string cpf)
        {
            using var connection = DatabaseConnection.Instance.GetConnection();

            string query = @"SELECT Nome, Cpf, Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado 
                             FROM pessoa 
                             WHERE cpf = @cpf";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@cpf", cpf);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var pessoa = new Pessoa();

                pessoa.DefinirNome(reader.GetString("Nome"));
                pessoa.DefinirCpf(reader.GetString("Cpf"));

                pessoa.DefinirEndereco(
                    reader.GetString("Cep"),
                    reader.GetString("Logradouro"),
                    reader.GetInt32("Numero"),
                    reader.IsDBNull(reader.GetOrdinal("Complemento")) ? string.Empty : reader.GetString("Complemento"),
                    reader.GetString("Bairro"),
                    reader.GetString("Cidade"),
                    reader.GetString("Estado")
                );

                return pessoa;
            }

            return null;
        }


        public List<Pessoa> ListarTodos()
        {
            var lista = new List<Pessoa>();
            using var connection = DatabaseConnection.Instance.GetConnection();

            string query = @"SELECT Nome, Cpf, Cep, Logradouro, Numero, Complemento, Bairro, Cidade, Estado 
                             FROM pessoa";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var pessoa = new Pessoa();

                pessoa.DefinirNome(reader.GetString("Nome"));
                pessoa.DefinirCpfBanco(reader.GetString("Cpf"));

                pessoa.DefinirEndereco(
                    reader.GetString("Cep"),
                    reader.GetString("Logradouro"),
                    reader.GetInt32("Numero"),
                    reader.IsDBNull(reader.GetOrdinal("Complemento")) ? string.Empty : reader.GetString("Complemento"),
                    reader.GetString("Bairro"),
                    reader.GetString("Cidade"),
                    reader.GetString("Estado")
                );

                lista.Add(pessoa);
            }

            return lista;
        }
    }
}
using CadastroCliente.Entidades;
using CadastroCliente.Enum;
using CadastroCliente.Infraestrutura.Persistence.Repositories;
using CadastroCliente.Interfaces;
using CadastroCliente.Servicos;

class Program
{
    static async Task Main(string[] args)
    {
        await Menu();
    }

    public static async Task Menu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("Lojão Progresso");
            Console.WriteLine("1 - Cadastrar novo cliente");
            Console.WriteLine("2 - Consultar Cliente por CPF");
            Console.WriteLine("3 - Consultar todos os clientes");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine() ?? string.Empty;


            switch(opcao)
            {
                case "1": 
                    await CadastroAsync();
                break;

                case "2": 
                    await ConsultarPorCpf(); 
                break;

                case "3":
                    await ConsultarTodos();
                break;

                default: Console.WriteLine("Opção Invalida");
                break;
            }

           
        }
    }

    public static async Task CadastroAsync()
    {
        Console.Clear();
        Console.WriteLine("Cadastro de Pessoas");
        var novaPessoa = new Pessoa();

        while (true)
        {
            try
            {
                Console.Write("Digite o Nome: ");
                string nomeInput = Console.ReadLine() ?? string.Empty;
                novaPessoa.DefinirNome(nomeInput);
                break;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: {ex.Message} Tente novamente.\n");
                Console.ResetColor();
            }
        }
        while (true)
        {
            try
            {
                Console.Write("Digite o CPF (apenas números ou com pontos): ");
                string cpfInput = Console.ReadLine() ?? string.Empty;

                novaPessoa.DefinirCpf(cpfInput);

                var repoVerificacao = new PessoaRepository();
                if (repoVerificacao.ConsultarUsuario(novaPessoa.Cpf) != null)
                {
                    throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF.");
                }

                break;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: {ex.Message} Tente novamente.\n");
                Console.ResetColor();
            }
        }

        string logradouro = "", bairro = "", cidade = "", estado = "", cepInformado = "";

        while (true)
        {
            try
            {
                Console.WriteLine("\n--- ESCOLHA O PROVEDOR DE CEP ---");
                Console.WriteLine("1 - ViaCEP");
                Console.WriteLine("2 - BrasilAPI");
                Console.Write("Opção: ");

                if (!int.TryParse(Console.ReadLine(), out int escolhaServico) || (escolhaServico != 1 && escolhaServico != 2))
                {
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    continue;
                }

                Console.Write("Digite o CEP (apenas números): ");
                cepInformado = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(cepInformado) || cepInformado.Length < 8)
                {
                    Console.WriteLine("CEP inválido para consulta.");
                    continue;
                }

                using HttpClient httpClient = new HttpClient();
                TipoServicoCep tipo = (TipoServicoCep)escolhaServico;
                ICepService cepService = CepServiceFactory.CriarServico(tipo, httpClient);

                Console.WriteLine($"Consultando via {tipo}...");
                var enderecoDto = await cepService.ConsultarCepAsync(cepInformado);

                if (enderecoDto == null || string.IsNullOrWhiteSpace(enderecoDto.Logradouro))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("RN06: CEP não encontrado ou serviço indisponível. Tente outro CEP ou provedor.");
                    Console.ResetColor();
                    continue;
                }

                logradouro = enderecoDto.Logradouro;
                bairro = enderecoDto.Bairro;
                cidade = enderecoDto.Cidade;
                estado = string.IsNullOrWhiteSpace(enderecoDto.Uf) ? enderecoDto.Estado : enderecoDto.Uf;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Endereço encontrado: {logradouro}, {bairro} - {cidade}/{estado}");
                Console.ResetColor();
                break;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"RN07: Falha na comunicação com a API ({ex.Message}). Tente novamente.");
                Console.ResetColor();
            }
        }
        int numero = 0;
        while (true)
        {
            try
            {
                Console.Write("Digite o Número do endereço: ");
                if (!int.TryParse(Console.ReadLine(), out numero))
                    throw new FormatException("O número deve ser um valor inteiro válido.");
                break;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: {ex.Message}");
                Console.ResetColor();
            }
        }

        Console.Write("Digite o Complemento (opcional): ");
        string complemento = Console.ReadLine() ?? string.Empty;

        novaPessoa.DefinirEndereco(cepInformado, logradouro, numero, complemento, bairro, cidade, estado);

        try
        {
            var repo = new PessoaRepository();
            repo.Inserir(novaPessoa);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCESSO] Cliente cadastrado e gravado no banco com sucesso!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERRO NO BANCO] Não foi possível salvar o registro: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
    }

    public static async Task ConsultarPorCpf()
    {
        Console.Clear();
        Console.WriteLine("CONSULTA DE CLIENTE");
        Console.Write("Digite o CPF que deseja buscar (apenas números): ");
        string cpfBusca = Console.ReadLine() ?? string.Empty;

        try
        {
            var repositorio = new PessoaRepository();
            var pessoa = repositorio.ConsultarUsuario(cpfBusca);

            if (pessoa != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[CLIENTE ENCONTRADO]");
                Console.WriteLine($"Nome: {pessoa.Nome}");
                Console.WriteLine($"CPF: {pessoa.Cpf}");
                Console.WriteLine($"Endereço: {pessoa.Logradouro}, Nº {pessoa.Numero} {(!string.IsNullOrEmpty(pessoa.Complemento) ? $"({pessoa.Complemento})" : "")}");
                Console.WriteLine($"Bairro: {pessoa.Bairro}");
                Console.WriteLine($"Cidade/UF: {pessoa.Cidade} - {pessoa.Estado}");
                Console.WriteLine($"CEP: {pessoa.Cep}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhum cliente encontrado com este CPF.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nErro ao consultar o banco de dados: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();

}
    public static async Task ConsultarTodos()
    {
        try
        {
            var repositorio = new PessoaRepository();
            var clientes = repositorio.ListarTodos();

            if (clientes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nenhum cliente cadastrado no banco de dados.");
                Console.ResetColor();
            }
            else
            {
                foreach (var pessoa in clientes)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"--------------------------------------------------");
                    Console.ResetColor();
                    Console.WriteLine($"Nome: {pessoa.Nome}");
                    Console.WriteLine($"CPF: {pessoa.Cpf}");
                    Console.WriteLine($"Endereço: {pessoa.Logradouro}, Nº {pessoa.Numero} {(!string.IsNullOrEmpty(pessoa.Complemento) ? $"({pessoa.Complemento})" : "")}");
                    Console.WriteLine($"Bairro: {pessoa.Bairro} | Cidade/UF: {pessoa.Cidade} - {pessoa.Estado} | CEP: {pessoa.Cep}");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"--------------------------------------------------");
                Console.ResetColor();
                Console.WriteLine($"\nTotal de registros encontrados: {clientes.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro ao consultar o banco de dados: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
    }
}

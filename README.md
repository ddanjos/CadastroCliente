# CadastroCliente

Tecnologias Utilizadas
Linguagem: C# (.NET 10)

Banco de Dados: MySQL (MySqlConnector)

Comunicação HTTP: HttpClient / System.Net.Http.Json

Gerenciamento de Configurações: Microsoft.Extensions.Configuration.Json


Como Configurar e Rodar
Passo 1: Clonar o Repositório e Configurar o Banco
Clone o repositório em sua máquina.

Certifique-se de ter o MySQL rodando localmente.

Crie um banco de dados cadastrocliente e execute o script SQL de criação da tabela Pessoa.

é necessário configurar um arquivo json appsettings.json
com a variavel de conexão do banco de dados:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=cadastroc_db;Uid=seu_usuario;Pwd=sua_senha;"
  }
}

[ Console / Program.cs ]
         │
         ├──► [ CepServiceFactory ] ──► Cria dinamicamente ──► [ ICepService ]
         │                                                            │
         │                                      ┌─────────────────────┴─────────────────────┐
         │                                      ▼                                           ▼
         │                                [ ViaCepService ]                          [ BrasilApiService ]
         │                                      │                                           │
         │                                      └──────────────────► [ EnderecoDto ] ◄──────┘
         │                                                           (Formato Único)
         │
         ├──► [ Entidade Pessoa ] ◄── (Aplica Regras de Negócio e Validações de CPF)
         │
         └──► [ PessoaRepository ] ──► [ DatabaseConnection (Singleton) ] ──► [ Banco MySQL ]


Program.cs: Orquestra o menu e o fluxo de interação com o usuário.

Factory & Strategy: Permitem escolher dinamicamente qual API de CEP chamar, convertendo o resultado para o EnderecoDto.

Entidade Pessoa: Concentra as regras de negócio e protege suas invariantes.

Repository & Singleton: Gerenciam o acesso persistente ao banco de dados MySQL de forma centralizada.

. Justificativa das Decisões de Arquitetura
Princípios SOLID & Separação de Responsabilidades:

Design Pattern - Strategy & Factory (para as APIs de CEP):
Para atender à regra de negócio de suportar múltiplos fornecedores e permitir a troca em tempo de execução sem alterar o código.

Design Pattern - Adapter/DTO (EnderecoDto):
Como cada API externa retorna estruturas de JSON diferentes, foi criado o EnderecoDto como um formato único intermediário. Isso impede que os detalhes específicos de implementação de terceiros fiquem espalhados pelo sistema.

Design Pattern - Repository:
Isola toda a lógica de persistência e comandos SQL (MySqlCommand, MySqlDataReader). O domínio da aplicação (Pessoa) não possui conhecimento direto de infraestrutura ou banco de dados.

Design Pattern - Singleton:
Utilizado no gerenciamento de conexões com o banco de dados (DatabaseConnection) para centralizar a criação do canal de comunicação de forma controlada, evitando o consumo excessivo de recursos da máquina.

Encapsulamento Forte na Entidade:
A classe Pessoa não é um modelo anêmico. Ela possui construtores e métodos validadores (DefinirCpf, DefinirNome), garantindo que nenhum cliente seja criado em estado inválido na aplicação.

Banco de dados:

CREATE TABLE Pessoa (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(150) NOT NULL,
    Cpf VARCHAR(11) NOT NULL UNIQUE,
    Cep VARCHAR(8) NOT NULL,
    Logradouro VARCHAR(200) NOT NULL,
    Numero INT NOT NULL,
    Complemento VARCHAR(100) NULL,
    Bairro VARCHAR(100) NOT NULL,
    Cidade VARCHAR(100) NOT NULL,
    Estado CHAR(2) NOT NULL
);


# Sistema de Cadastro de Clientes

Aplicação em C# (Console Application) desenvolvida para gerenciar o cadastro de clientes de forma segura, aplicando princípios de Orientação a Objetos, SOLID e Padrões de Projeto. O grande diferencial é a consulta automatizada de endereços via CEP, suportando múltiplos provedores de forma dinâmica.

---

##  Tecnologias Utilizadas
* **Linguagem:** C# (.NET)
* **Banco de Dados:** MySQL (`MySqlConnector`)
* **Comunicação HTTP:** `HttpClient` / `System.Net.Http.Json`
* **Gerenciamento de Configurações:** `Microsoft.Extensions.Configuration.Json`

---

##  Como Configurar e Rodar

### Passo 1: Clonar e Configurar o Banco de Dados
1. Clone este repositório em sua máquina.
2. Certifique-se de ter o MySQL rodando localmente.
3. Crie um banco de dados chamado `cadastroc_db`.
4. Execute o script SQL abaixo para criar a tabela necessária:

```sql
CREATE TABLE Pessoa (
    Id INT AUTO_INCREMENT PRIMARY KEY,
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

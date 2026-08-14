using CadastroCliente.Servicos;
using System;

namespace CadastroCliente.Entidades
{
    public class Pessoa
    {
        public string Nome { get; private set; } = string.Empty;
        public string Cpf { get; private set; } = string.Empty;
        public string Cep { get; private set; } = string.Empty;
        public string Logradouro { get; private set; } = string.Empty;
        public int Numero { get; private set; }
        public string Complemento { get; private set; } = string.Empty;
        public string Bairro { get; private set; } = string.Empty;
        public string Cidade { get; private set; } = string.Empty;
        public string Estado { get; private set; } = string.Empty;
        public Pessoa() { }

        public void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome não pode ser vazio.");

            Nome = nome.Trim();
        }

        public void DefinirCpf(string cpf)
        {
            if (!CpfValidador.CpfValido(cpf))
                throw new ArgumentException("CPF inválido!");

            Cpf = cpf.Trim();
        }

        public void DefinirCpfBanco(string cpf)
        {
            Cpf = cpf.Trim();
        }


        public void DefinirEndereco(string cep, string logradouro, int numero, string complemento, string bairro, string cidade, string estado)
        {
            if (string.IsNullOrWhiteSpace(cep) || cep.Length < 8)
                throw new ArgumentException("CEP inválido.");

            Cep = cep.Trim();
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento ?? string.Empty;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }
    }
}
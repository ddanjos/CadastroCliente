namespace CadastroCliente.DTOs
{
    public class EnderecoDto
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty; 
        public string Uf { get; set; } = string.Empty;     

        public EnderecoDto() { }
        public EnderecoDto(string cep, string logradouro, string bairro, string cidade, string uf)
        {
            Cep = cep;
            Logradouro = logradouro;
            Bairro = bairro;
            Cidade = cidade;
            Estado = uf;
            Uf = uf;
        }
    }
}
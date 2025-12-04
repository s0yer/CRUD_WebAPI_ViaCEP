using System.ComponentModel.DataAnnotations;

namespace CRUD_WebAPI_ViaCEP.Models
{
    public class Endereco
    {
        // 1. Encapsulamento: Propriedade Id com setter privado
        public int Id { get; private set; }

        // Campos de Endereço
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty; // Cidade
        public string Uf { get; set; } = string.Empty; // Estado
        public string Ibge { get; set; } = string.Empty; // Código IBGE

        // Construtor privado para o EF Core e um público para criação/mapeamento
        private Endereco() { }

        public Endereco(string cep, string logradouro, string complemento, string bairro, string localidade, string uf, string ibge)
        {
            Cep = cep;
            Logradouro = logradouro;
            Complemento = complemento;
            Bairro = bairro;
            Localidade = localidade;
            Uf = uf;
            Ibge = ibge;
        }

        // Método para simular a Polimorfismo/Encapsulamento
        // Pode ser usado para mapear a partir de um DTO do ViaCEP
        public void Atualizar(EnderecoDto dto)
        {
            Logradouro = dto.Logradouro;
            Complemento = dto.Complemento;
            Bairro = dto.Bairro;
            Localidade = dto.Localidade;
            Uf = dto.Uf;
            Ibge = dto.Ibge;
        }
    }
}

using System.Text.Json.Serialization;

namespace CRUD_WebAPI_ViaCEP.Models
{
    public class EnderecoDto
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; } = string.Empty;

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; } = string.Empty;

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; } = string.Empty;

        [JsonPropertyName("localidade")]
        public string Localidade { get; set; } = string.Empty; 

        [JsonPropertyName("uf")]
        public string Uf { get; set; } = string.Empty; 

        [JsonPropertyName("ibge")]
        public string Ibge { get; set; } = string.Empty;

        // se for true, o CEP não foi encontrado
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
    }
}

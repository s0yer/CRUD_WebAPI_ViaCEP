using System.ComponentModel.DataAnnotations;

namespace CRUD_WebAPI_ViaCEP.Models
{
    public class EnderecoAPI
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Cep { get; set; } = string.Empty;

        [Required]
        public string Logradouro { get; set; } = string.Empty;

        public string? Complemento { get; set; }

        [Required]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        public string Localidade { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string Uf { get; set; } = string.Empty;

        // Opcionais:
        public string? Ibge { get; set; }
        public string? Gia { get; set; }
        // sobrescrita 
        public override string ToString()
        {
            return $"{Logradouro}, {Bairro}, {Localidade}-{Uf} - CEP: {Cep}";
        }
    }
}

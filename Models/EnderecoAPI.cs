namespace CRUD_WebAPI_ViaCEP.Models
{
    public class EnderecoAPI
    {
        public int Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty; // cidade
        public string Uf { get; set; } = string.Empty; // estado

        // sobrescrita 
        public override string ToString()
        {
            return $"{Logradouro}, {Bairro}, {Localidade}-{Uf} - CEP: {Cep}";
        }
    }
}

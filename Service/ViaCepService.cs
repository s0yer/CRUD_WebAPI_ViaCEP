using CRUD_WebAPI_ViaCEP.Models;

namespace CRUD_WebAPI_ViaCEP.Service
{
    // Interface para injeção de dependência
    public interface IViaCepService
    {
        Task<EnderecoDto?> ConsultarCepAsync(string cep);
    }

    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        // Injeção de dependência do HttpClient
        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Configura a URL base
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        public async Task<EnderecoDto?> ConsultarCepAsync(string cep)
        {
            // 4. Tratamento de Exceções e Prevenção de Erros
            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
            {
                // Retorna nulo ou DTO com erro se a validação básica falhar
                return null;
            }

            try
            {
                // Formato da requisição: /ws/{cep}/json/
                var response = await _httpClient.GetAsync($"{cep}/json/");
                response.EnsureSuccessStatusCode(); // Lança exceção para códigos 4xx/5xx

                var endereco = await response.Content.ReadFromJsonAsync<EnderecoDto>();

                // A API ViaCEP retorna um objeto JSON com "erro": true para CEPs não encontrados.
                if (endereco != null && endereco.Erro)
                {
                    return null; // CEP não encontrado ou inválido.
                }

                return endereco;
            }
            catch (HttpRequestException ex)
            {
                // Trata erros de rede ou de API
                // Logger (não implementado aqui, mas essencial)
                throw new Exception($"Erro ao consultar ViaCEP para o CEP {cep}.", ex);
            }
            // Outras exceções podem ser tratadas aqui (ex: JsonException)
        }
    }
}

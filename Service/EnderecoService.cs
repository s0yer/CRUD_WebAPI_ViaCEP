using CRUD_WebAPI_ViaCEP.Data;
using CRUD_WebAPI_ViaCEP.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_WebAPI_ViaCEP.Service
{
    public interface IEnderecoService
    {
        Task<Endereco?> CadastrarNovoEnderecoPorCepAsync(string cep);
        Task<Endereco?> ConsultarEnderecoPorIdAsync(int id);
        Task<Endereco?> AtualizarEnderecoPorCepAsync(int id, string novoCep);
        Task<bool> DeletarEnderecoAsync(int id);
        Task<bool> ValidarCepExternoAsync(string cep); // Para validar CEP via API externa
    }

    // Orquestra o Repository e o ViaCepService.
    public class EnderecoService : IEnderecoService
    {
        private readonly AppDbContext _context; 
        private readonly IViaCepService _viaCepService;

        // inject via constructor
        public EnderecoService(AppDbContext context, IViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
        }

        // --- Operações CRUD ---

        // Regra de Negócio: Buscar CEP na API pública e cadastrar o endereço retornado.
        public async Task<Endereco?> CadastrarNovoEnderecoPorCepAsync(string cep)
        {
            var dadosCep = await _viaCepService.ConsultarCepAsync(cep);

            if (dadosCep == null)
            {
                // 4. Tratamento de exceções: Se o CEP for inválido/não encontrado
                throw new ApplicationException($"O CEP '{cep}' é inválido ou não foi encontrado pela API ViaCEP.");
            }

            // Verifica se o CEP já existe no banco (para evitar duplicidade)
            var existe = await _context.Enderecos.AnyAsync(e => e.Cep == dadosCep.Cep);
            if (existe)
            {
                throw new ApplicationException($"O endereço com o CEP '{dadosCep.Cep}' já está cadastrado.");
            }

            // Mapeia o DTO para a Entidade de Domínio
            var novoEndereco = new Endereco(
                dadosCep.Cep,
                dadosCep.Logradouro,
                dadosCep.Complemento,
                dadosCep.Bairro,
                dadosCep.Localidade,
                dadosCep.Uf,
                dadosCep.Ibge
            );

            _context.Enderecos.Add(novoEndereco);
            await _context.SaveChangesAsync();

            return novoEndereco;
        }

        public async Task<Endereco?> ConsultarEnderecoPorIdAsync(int id)
        {
            // Consultar no registro gravado no banco de dados.
            return await _context.Enderecos.FindAsync(id);
        }

        // Regra de Negócio: Consulta API Externa (ViaCEP) e atualiza o registro no banco.
        public async Task<Endereco?> AtualizarEnderecoPorCepAsync(int id, string novoCep)
        {
            var enderecoExistente = await _context.Enderecos.FindAsync(id);

            if (enderecoExistente == null)
            {
                throw new NotFoundException($"Endereço com ID {id} não encontrado para atualização.");
            }

            var dadosCep = await _viaCepService.ConsultarCepAsync(novoCep);

            if (dadosCep == null)
            {
                throw new ApplicationException($"O novo CEP '{novoCep}' é inválido ou não foi encontrado pela API ViaCEP.");
            }

            // 2. Orientação a Objeto: Utiliza o método 'Atualizar' da entidade (encapsulamento/polimorfismo)
            enderecoExistente.Atualizar(dadosCep);

            _context.Enderecos.Update(enderecoExistente);
            await _context.SaveChangesAsync();

            return enderecoExistente;
        }

        public async Task<bool> DeletarEnderecoAsync(int id)
        {
            var enderecoExistente = await _context.Enderecos.FindAsync(id);

            if (enderecoExistente == null)
            {
                return false;
            }

            _context.Enderecos.Remove(enderecoExistente);
            await _context.SaveChangesAsync();

            return true;
        }

        // --- Rota Adicional (Validação de CEP) ---

        public async Task<bool> ValidarCepExternoAsync(string cep)
        {
            var dadosCep = await _viaCepService.ConsultarCepAsync(cep);
            return dadosCep != null;
        }
    }
}
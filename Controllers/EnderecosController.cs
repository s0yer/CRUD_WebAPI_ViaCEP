using CRUD_WebAPI_ViaCEP.Models;
using CRUD_WebAPI_ViaCEP.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_WebAPI_ViaCEP.Controllers
{
    [Route("api_viacep/[controller]")]
    [ApiController]
    public class EnderecosController : ControllerBase
    {
        private readonly IEnderecoService _enderecoService;

        public EnderecosController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        //  GET
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Endereco), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var endereco = await _enderecoService.ConsultarEnderecoPorIdAsync(id);

            if (endereco == null)
            {
                return NotFound($"Endereço com ID {id} não encontrado.");
            }

            return Ok(endereco);
        }

        //  POST
        // buscar o endereço na API pública ViaCEP e cadastra.
        [HttpPost("{cep}")]
        [ProducesResponseType(typeof(Endereco), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Endereco endereco)
        {
            try
            {
                var novoEndereco = await _enderecoService.CadastrarNovoEnderecoPorCepAsync(endereco.Cep);

                // Retorna 201 Created com o recurso criado
                return CreatedAtAction(nameof(Get), new { id = novoEndereco!.Id }, novoEndereco);
            }
            // tratamento de Exceções
            catch (ApplicationException ex)
            {
                // Exceção de negócio (CEP inválido, já existe, etc.)
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Rota Adicional: Consulta CEP na API externa para validar
        [HttpGet("ValidarCep/{cep}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidarCep(string cep)
        {
            var valido = await _enderecoService.ValidarCepExternoAsync(cep);
            return Ok(new { Cep = cep, Valido = valido });
        }


        // PUT
        // atualizar o CEP de um registro existente.
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Endereco), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put( [FromBody] Endereco endereco)
        {
            try
            {
                var enderecoAtualizado = await _enderecoService.AtualizarEnderecoPorCepAsync(endereco.Id, endereco.Cep);
                return Ok(enderecoAtualizado);
            }
            // Tratamento de Exceções
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                // Exceção de negócio (novo CEP inválido)
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletado = await _enderecoService.DeletarEnderecoAsync(id);

            if (!deletado)
            {
                return NotFound($"Endereço com ID {id} não encontrado para exclusão.");
            }

            // 204 No Content para exclusão bem-sucedida
            return NoContent();
        }
    }
}

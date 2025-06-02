using Microsoft.AspNetCore.Mvc;
using GestaoPessoalApi.Data;
using GestaoPessoalApi.Models;

namespace GestaoPessoalApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FuncionariosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public FuncionariosController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/funcionarios
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
		{
			return await _context.Funcionarios.ToListAsync();
		}

		// GET: api/funcionarios/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
		{
			var funcionario = await _context.Funcionarios.FindAsync(id);

			if (funcionario == null)
			{
				return NotFound();
			}

			return funcionario;
		}

		// POST: api/funcionarios
		[HttpPost]
		public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
		{
			_context.Funcionarios.Add(funcionario);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, funcionario);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
		{
			if (id != funcionario.Id)
			{
				return BadRequest();
			}

			var funcionarioExistente = await _context.Funcionarios.FindAsync(id);
			if (funcionarioExistente == null)
			{
				return NotFound();
			}

			// Comparar campos e registrar alterações
			if (funcionarioExistente.Nome != funcionario.Nome)
				await RegistrarAlteracao(id, "Nome", funcionarioExistente.Nome, funcionario.Nome);

			if (funcionarioExistente.Cargo != funcionario.Cargo)
				await RegistrarAlteracao(id, "Cargo", funcionarioExistente.Cargo, funcionario.Cargo);

			if (funcionarioExistente.Salario != funcionario.Salario)
				await RegistrarAlteracao(id, "Salario", funcionarioExistente.Salario.ToString(), funcionario.Salario.ToString());

			// Atualiza os dados
			funcionarioExistente.Nome = funcionario.Nome;
			funcionarioExistente.Cargo = funcionario.Cargo;
			funcionarioExistente.Salario = funcionario.Salario;
			funcionarioExistente.Status = funcionario.Status;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteFuncionario(int id)
		{
			var funcionario = _context.Funcionarios.Find(id);
			if (funcionario == null)
			{
				return NotFound();
			}

			// Deletar funcionário
			_context.Funcionarios.Remove(funcionario);
			_context.SaveChanges();

			// Registrar a alteração
			RegistrarAlteracao(
				acao: "Delete",
				usuario: usuarioLogado,
				detalhes: $"Funcionário ID {id} deletado."
			);

			return NoContent();
		}

		private async Task RegistrarAlteracao(int funcionarioId, string campo, string valorAntigo, string valorNovo)
		{
			var historico = new HistoricoAlteracao
			{
				FuncionarioId = funcionarioId,
				CampoAlterado = campo,
				ValorAntigo = valorAntigo,
				ValorNovo = valorNovo,
				DataHora = DateTime.Now
			};

			_context.Historicos.Add(historico);
			await _context.SaveChangesAsync();
		}
	}
}

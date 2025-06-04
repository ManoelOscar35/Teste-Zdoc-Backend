using Microsoft.EntityFrameworkCore;
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

		// PUT: api/Funcionarios/{id}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
		{
			if (id != funcionario.Id)
				return BadRequest("O ID da URL e do corpo não conferem.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Busca a entidade existente no banco
			var existente = await _context.Funcionarios
										  .AsNoTracking()
										  .FirstOrDefaultAsync(f => f.Id == id);
			if (existente == null)
				return NotFound();

			// Aqui, façamos comparações campo a campo para registrar histórico
			// Exemplo: se o nome mudou, registrar em historico
			if (!string.Equals(existente.Nome, funcionario.Nome, StringComparison.OrdinalIgnoreCase))
			{
				await RegistrarAlteracao(
					funcionarioId: id,
					campo: "Nome",
					valorAnterior: existente.Nome,
					valorNovo: funcionario.Nome
				);
			}

			if (!string.Equals(existente.Cargo, funcionario.Cargo, StringComparison.OrdinalIgnoreCase))
			{
				await RegistrarAlteracao(
					funcionarioId: id,
					campo: "Cargo",
					valorAnterior: existente.Cargo,
					valorNovo: funcionario.Cargo
				);
			}

			if (existente.DataAdmissao != funcionario.DataAdmissao)
			{
				await RegistrarAlteracao(
					funcionarioId: id,
					campo: "DataAdmissao",
					valorAnterior: existente.DataAdmissao.ToString("yyyy-MM-dd"),
					valorNovo: funcionario.DataAdmissao.ToString("yyyy-MM-dd")
				);
			}

			if (existente.Salario != funcionario.Salario)
			{
				await RegistrarAlteracao(
					funcionarioId: id,
					campo: "Salario",
					valorAnterior: existente.Salario.ToString("F2"),
					valorNovo: funcionario.Salario.ToString("F2")
				);
			}

			if (!string.Equals(existente.Status, funcionario.Status, StringComparison.OrdinalIgnoreCase))
			{
				await RegistrarAlteracao(
					funcionarioId: id,
					campo: "Status",
					valorAnterior: existente.Status,
					valorNovo: funcionario.Status
				);
			}

			// Atualiza o funcionário propriamente dito
			_context.Entry(funcionario).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FuncionarioExiste(id))
					return NotFound();

				throw;
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFuncionario(int id)
		{
			var funcionario = _context.Funcionarios.Find(id);
			if (funcionario == null)
			{
				return NotFound();
			}

			await RegistrarAlteracao(
				id,
				"Status",
				funcionario.Status,
				"Excluído"
			);

			// Opcional: marcar como inativo ou realmente excluir
			_context.Funcionarios.Remove(funcionario);

			await _context.SaveChangesAsync();

			return NoContent();

		}

		private bool FuncionarioExiste(int id)
		{
			return _context.Funcionarios.Any(e => e.Id == id);
		}

		private async Task RegistrarAlteracao(int funcionarioId, string campo, string valorAnterior, string valorNovo)
		{
			var historico = new HistoricoAlteracao
			{
				FuncionarioId = funcionarioId,
				CampoAlterado = campo,
				ValorAnterior = valorAnterior,
				ValorNovo = valorNovo,
				DataHora = DateTime.UtcNow
			};

			_context.Historicos.Add(historico);
			await _context.SaveChangesAsync();
		}

		// GET: api/funcionarios/media-salario
		[HttpGet("media-salario")]
		public async Task<ActionResult<decimal>> GetSalarioMedio()
		{
			// Se não houver funcionários cadastrados, evitamos erro de AverageAsync
			var quantidade = await _context.Funcionarios.CountAsync();
			if (quantidade == 0)
				return Ok(0m); // ou NotFound(), a critério do seu design

			// Calcula a média de todos os salários
			var media = await _context.Funcionarios.AverageAsync(f => f.Salario);
			return Ok(media);
		}
	}
}
